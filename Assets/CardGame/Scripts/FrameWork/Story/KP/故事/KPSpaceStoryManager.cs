using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class KPSpaceStoryManager
{
    public string context;
    public List<string> availableNpcs;
    public Dictionary<string, NpcCardModel> sceneNpcs = new Dictionary<string, NpcCardModel>();
    public Dictionary<string, GptChatSession> npcChatSessions = new Dictionary<string, GptChatSession>();
    public GptChatSession narratorSession;

    /// <summary>
    /// 这里的重要信息有哪些
    /// </summary>
    public string importantThings;

    /// <summary>
    /// 已经发现的信息
    /// </summary>
    public string hasFindThings;

    [Button("开始故事")]
    public async void StartSpaceStory()
    {
        if (string.IsNullOrEmpty(context))
        {
            Debug.LogError("Context is empty!");
            return;
        }

        var player = GameFrameWork.Instance.playerManager.nowPlayer;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        var playerChat = player.GetComponent<ChatComponent>();
        if (playerChat == null)
        {
            Debug.LogError("ChatComponent not found on player!");
            return;
        }

        ParseSceneNpcs();

        if (sceneNpcs.Count == 0)
        {
            Debug.LogWarning("No NPCs found in scene!");
        }

        InitNarratorSession();

        foreach (var npcPair in sceneNpcs)
        {
            InitNpcSession(npcPair.Key, npcPair.Value);
        }


        GameFrameWork.Instance.ChatPanel.Init(playerChat, null, new KPStoryListener(async e =>
        {
            e.panel.submitBtn.gameObject.SetActive(false);
            await HandleUserInput(e);
            e.panel.submitBtn.gameObject.SetActive(true);
        }, () => { }));

        if (availableNpcs != null && availableNpcs.Count > 0)
        {
            GameFrameWork.Instance.ChatPanel.UpdateDropdownOptions(availableNpcs);
        }
    }

    private void ParseSceneNpcs()
    {
        sceneNpcs.Clear();
        var allNpcs = GameFrameWork.Instance.playerManager.allNpc;
        foreach (var x in availableNpcs)
        {
            var npc = allNpcs.Find(n => n.npcId == x);
            if (npc != null && !sceneNpcs.ContainsKey(x))
            {
                sceneNpcs[x] = npc;
            }
        }
    }

    private void InitNarratorSession()
    {
        var prompt = "你是《克苏鲁的呼唤》跑团的【KP（主持人）画外音解说】。\n" +
                     "\n" +
                     "你的职责是：\n" +
                     "- 描述场景氛围、环境变化\n" +
                     "- 旁白式的剧情推进\n" +
                     "- 回应玩家的非对话性输入\n" +
                     "⚠️ 重要约束：\n" +
                     "- 你不扮演任何具体角色\n" +
                     "- 你只做客观的、氛围化的描述\n" +
                     "- 不推进剧情，不替你做决定\n" +
                     "- 使用第三人称、描述性语言\n" +
                     "\n" +
                     "场景背景" +
                     context + "\n" +
                     "【输出格式要求】\n" +
                     "你必须严格按以下 JSON 输出：\n" +
                     "{\n" +
                     "  \"message\": \"你的画外音描述内容\"\n" +
                     "}";

        narratorSession = new GptChatSession(prompt);
    }

    private void InitNpcSession(string npcName, NpcCardModel npc)
    {
        var prompt = "你是一个【NPC 对话结果的 JSON 转译工具】。\n" +
                     "你的职责是：根据提供的 NPC 设定，生成该 NPC 在当前情境下可能给出的【一句回应】，并用 JSON 输出。\n" +
                     "\n" +
                     "NPC ID: " + npcName + "\n" +
                     "NPC 描述: " + (npc.npcId ?? "未知") + "\n" +
                     "场景描述: " + context + "\n" +
                     "\n" +
                     "⚠️ 重要约束：\n" +
                     "- 你不是 NPC 本人\n" +
                     "- 你不进行角色扮演\n" +
                     "- 你不沉浸、不叙事、不扩写背景\n" +
                     "- 你只负责模拟 NPC 会说什么，并将其格式化为 JSON\n" +
                     "- 除 JSON 外，不得输出任何内容\n" +
                     "\n" +
                     "【输出格式要求】\n" +
                     "你必须严格按以下 JSON 输出：\n" +
                     "{\n" +
                     "  \"message\": \"NPC 的一句回应内容\"\n" +
                     "}";

        npcChatSessions[npcName] = new GptChatSession(prompt);
    }


    private async Task HandleUserInput(ChatInput input)
    {
        var userStr = input.Content.Trim();
        var selectedNpc = input.SelectedNpcName;

        if (!string.IsNullOrEmpty(selectedNpc) && selectedNpc != "KP")
        {
            await HandleNpcDialogue(selectedNpc, userStr, input);
        }
        else
        {
            await HandleFreeInput(userStr, input);
        }
    }

    private async Task HandleNpcDialogue(string npcName, string userStr, ChatInput input)
    {
        if (sceneNpcs.ContainsKey(npcName))
        {
            var npc = sceneNpcs[npcName];
            var npcChat = npc.GetComponent<ChatComponent>();

            if (npcChat != null && npcChatSessions.ContainsKey(npcName))
            {
                var session = npcChatSessions[npcName];

                var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                                GptSchemaBuilder.BuildSchema(typeof(ChatContext));
                var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(session, userStr,
                    constrain);

                session.AddChatHistory("NPC", evt.message);
                input.panel.AddMessage($"【{npcName}】{evt.message}");

                await CheckForNewInformation(npcName, userStr, evt.message);
            }
            else
            {
                input.panel.AddMessage($"【系统】无法与 {npcName} 对话，该 NPC 没有 ChatComponent");
            }
        }
        else
        {
            input.panel.AddMessage($"【系统】场景中不存在角色：{npcName}");
        }
    }

    private async Task HandleFreeInput(string userStr, ChatInput input)
    {
        var parsedInput = ParseUserInput(userStr);

        if (parsedInput.IsDialogue)
        {
            if (sceneNpcs.ContainsKey(parsedInput.TargetNpc))
            {
                var npc = sceneNpcs[parsedInput.TargetNpc];
                var npcChat = npc.GetComponent<ChatComponent>();

                if (npcChat != null && npcChatSessions.ContainsKey(parsedInput.TargetNpc))
                {
                    var session = npcChatSessions[parsedInput.TargetNpc];

                    var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                                    GptSchemaBuilder.BuildSchema(typeof(ChatContext));
                    var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(session,
                        parsedInput.Message, constrain);

                    session.AddChatHistory("NPC", evt.message);
                    input.panel.AddMessage($"【{parsedInput.TargetNpc}】{evt.message}");

                    await CheckForNewInformation(parsedInput.TargetNpc, parsedInput.Message, evt.message);
                }
                else
                {
                    input.panel.AddMessage($"【系统】无法与 {parsedInput.TargetNpc} 对话，该 NPC 没有 ChatComponent");
                }
            }
            else
            {
                input.panel.AddMessage($"【系统】场景中不存在角色：{parsedInput.TargetNpc}");
            }
        }
        else
        {
            narratorSession.AddChatHistory("Player", userStr);

            var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                            GptSchemaBuilder.BuildSchema(typeof(ChatContext));
            var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(narratorSession, userStr,
                constrain);

            narratorSession.AddChatHistory("Narrator", evt.message);
            input.panel.AddMessage($"【画外音】{evt.message}");

            await CheckForNewInformation("画外音", userStr, evt.message);
        }
    }

    private async Task CheckForNewInformation(string target, string playerInput, string npcResponse)
    {
        if (string.IsNullOrEmpty(importantThings))
            return;

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【新信息发现器】。

你的职责是：
判断对话中是否发现了【重要信息】。

规则：
- 只判断，不创作
- 基于已知的 importantThings 进行判断
- 不重复已发现的信息"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【已知重要信息列表】
{importantThings}

【已发现的信息】
{hasFindThings}

【对话目标】
{target}

【玩家输入】
{playerInput}

【NPC/画外音回复】
{npcResponse}

【你的任务】

判断这次对话是否发现了【新的重要信息】。

判断标准：
- 信息必须不在""已发现的信息""中
- 信息必须在""已知重要信息列表""中
- 信息必须是具体的、可观察的

输出要求：
- 如果发现了新信息，返回新信息的描述
- 如果没有发现新信息，返回空字符串
- 只输出纯文本，不使用 JSON"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        if (!string.IsNullOrEmpty(result))
        {
            hasFindThings += $"\\n{result}";
        }
    }

    private ParsedInput ParseUserInput(string input)
    {
        var result = new ParsedInput();

        var match1 = Regex.Match(input, @"对([\\u4e00-\\u9fa5a-zA-Z0-9_]+)说[：:](.+)");
        if (match1.Success)
        {
            result.IsDialogue = true;
            result.TargetNpc = match1.Groups[1].Value;
            result.Message = match1.Groups[2].Value.Trim();
            return result;
        }

        var match2 = Regex.Match(input, @"^([\\u4e00-\\u9fa5a-zA-Z0-9_]+)[：:](.+)");
        if (match2.Success && sceneNpcs.ContainsKey(match2.Groups[1].Value))
        {
            result.IsDialogue = true;
            result.TargetNpc = match2.Groups[1].Value;
            result.Message = match2.Groups[2].Value.Trim();
            return result;
        }

        var match3 = Regex.Match(input, @"@([\\u4e00-\\u9fa5a-zA-Z0-9_]+)\\s+(.+)");
        if (match3.Success)
        {
            result.IsDialogue = true;
            result.TargetNpc = match3.Groups[1].Value;
            result.Message = match3.Groups[2].Value.Trim();
            return result;
        }

        result.IsDialogue = false;
        result.Message = input;
        return result;
    }
}

public class ParsedInput
{
    public bool IsDialogue;
    public string TargetNpc;
    public string Message;
}

public class KPStoryListener : IChatPanelListener
{
    private Func<ChatInput, Task> onSubmit;
    private Action onClose;
    
    public KPStoryListener(Func<ChatInput, Task> onSubmit, Action onClose)
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
