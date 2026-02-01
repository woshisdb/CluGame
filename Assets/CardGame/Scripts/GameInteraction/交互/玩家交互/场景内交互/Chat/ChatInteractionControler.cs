using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 基础聊天
/// </summary>
public class NpcChatListener:IChatPanelListener
{
    private Func<ChatInput,Task> onSubmit;
    private Action onClose;
    public NpcChatListener(Func<ChatInput,Task> onSubmit, Action onClose)
    {
        this.onSubmit = onSubmit;
        this.onClose = onClose;
    }
    public async Task OnSubmit(ChatInput input)
    {
        await onSubmit?.Invoke(input);
    }

    public void OnClose()
    {
        onClose?.Invoke();
    }
}

public class ChatContext
{
    public string message;
}

public class ChatInteractionControler:GetWorldMapInteraction
{
    public string GetKey()
    {
        return "交流";
    }
    public static string Build(AINpcMindCfg cfg)
    {
        var sb = new StringBuilder();

        sb.AppendLine("你是一个【NPC 对话结果的 JSON 转译工具】。");
        sb.AppendLine("你的职责是：根据提供的 NPC 设定，生成该 NPC 在当前情境下可能给出的【一句回应】，并用 JSON 输出。");
        sb.AppendLine();
        sb.AppendLine("⚠️ 重要约束：");
        sb.AppendLine("- 你不是 NPC 本人");
        sb.AppendLine("- 你不进行角色扮演");
        sb.AppendLine("- 你不沉浸、不叙事、不扩写背景");
        sb.AppendLine("- 你只负责“模拟 NPC 会说什么”，并将其格式化为 JSON");
        sb.AppendLine("- 除 JSON 外，不得输出任何内容");
        sb.AppendLine();

        sb.AppendLine("【NPC 设定（被模拟对象）】");
        sb.AppendLine();

        sb.AppendLine("【基本信息】");
        sb.AppendLine(cfg.npcInfo);
        sb.AppendLine();

        sb.AppendLine($"【性别】{cfg.sex}");
        sb.AppendLine($"【工作】{cfg.work}");
        sb.AppendLine($"【住所】{cfg.belong}");
        sb.AppendLine();

        sb.AppendLine("【人生经历】");
        sb.AppendLine(cfg.myHistory);
        sb.AppendLine();

        sb.AppendLine("【当前目标】");
        sb.AppendLine(cfg.myAimInStory);
        sb.AppendLine();

        sb.AppendLine("【擅长的事情】");
        sb.AppendLine(cfg.skillDetail);
        sb.AppendLine();

        sb.AppendLine("【心理状态】");
        sb.AppendLine(cfg.mentalState);
        sb.AppendLine();

        sb.AppendLine("【与他人的关系】");
        if (cfg.relationships != null && cfg.relationships.Count > 0)
        {
            foreach (var kv in cfg.relationships)
            {
                sb.AppendLine($"- 对 {kv.Key}");
                sb.AppendLine($"  关系：{kv.Value.relation}");
                sb.AppendLine($"  态度：{kv.Value.attitude}");
            }
        }
        else
        {
            sb.AppendLine("该 NPC 对他人的关系尚不明确。");
        }

        sb.AppendLine();
        sb.AppendLine("【回应生成规则】");
        sb.AppendLine("- 回应必须符合该 NPC 的认知、立场和动机");
        sb.AppendLine("- NPC 只知道自己经历或被明确告知的事情");
        sb.AppendLine("- NPC 可以撒谎、回避、隐瞒或带情绪");
        sb.AppendLine("- 不推进剧情，不替玩家做决定");
        sb.AppendLine("- 不使用现代术语或跑团术语");
        sb.AppendLine();

        sb.AppendLine("【输出格式要求】");
        sb.AppendLine("你必须严格按以下 JSON 输出：");
        sb.AppendLine("{");
        sb.AppendLine("  \"message\": \"NPC 的一句回应内容\"");
        sb.AppendLine("}");

        return sb.ToString();
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var mec = me.GetComponent<ChatComponent>();
            var otherc =beObj.GetComponent<ChatComponent>();
            mec.StartChat(otherc);
            otherc.StartChat(mec);
            var str = Build(me.GetComponent<AIMindComponent>().AINpcMindCfg);
            var constrain = "。【硬性规则 - 不可违背】\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\n你不能输出任何 JSON 以外的内容。" + GptSchemaBuilder.BuildSchema(typeof(ChatContext));
            var GptChatSession = new GptChatSession(str);
            // 保存初始角色设定到历史记录（以便三段式对话 history 组装）
            // GptChatSession.AddChatHistory("NPC", str);
            GameFrameWork.Instance.ChatPanel.Init(mec,otherc,new NpcChatListener(async e =>
            {
                e.panel.submitBtn.gameObject.SetActive(false);
                // 记录玩家输入到对话历史（不污染主 Messages）
                var userstr = e.Content;
                // GptChatSession.AddChatHistory("Player", userstr);
                var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(GptChatSession, userstr, constrain);
                // 将 NPC 的回复记录到历史
                GptChatSession.AddChatHistory("NPC", evt.message);
                e.panel.AddMessage(evt.message);
                e.panel.submitBtn.gameObject.SetActive(true);
            }, () =>
            {
                ChatComponent.EndChat(mec, otherc);
            }));
        }));
        return ret;
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (me.IsSatComponent<ChatComponent>()&&beObj.IsSatComponent<ChatComponent>())
        {
            if (me.GetComponent<ChatComponent>().CanChat()&&beObj.GetComponent<ChatComponent>().CanChat())
            {
                return true;
            }
        }

        return false;
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        return;
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.Talk;
    }
}
