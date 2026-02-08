using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GptNpcBaseInfoResult
{
    public List<GptNpcBaseInfo> Npcs;
}

[Serializable]
public class GptNpcBaseInfo
{
    public string Name;
    public string Description;
}

[Serializable]
public class GptSecondaryNpcResult
{
    public List<GptSecondaryNpc> Npcs;
}

[Serializable]
public class GptSecondaryNpc
{
    public string Name;
    public string Description;

    /// <summary>
    /// 他主要关联的核心 NPC 名字（必须来自传入 npcs）
    /// </summary>
    public string RelatedTo;
}

public class SpaceCreator
{
    public string name;
    public string detail;
    /// <summary>
    /// 所有相邻可以去的区域
    /// </summary>
    public List<string> spaces=new();
}


public class GameGenerate
{
    public class GptNpcCheckResult
    {
        public bool IsNpc;
        public string Name;
        public string Description;
    }
    /// <summary>
    /// 列出基本的npc信息
    /// </summary>
    /// <returns></returns>
    [Button]
    public async Task<Dictionary<string, string>> GenerateBaseNpcInfs(
        Dictionary<string, string> cocObjects
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptNpcCheckResult));
        var result = new Dictionary<string, string>();

        foreach (var kv in cocObjects)
        {
            var objectName = kv.Key;
            var objectDesc = kv.Value;

            var messages = new List<QwenChatMessage>
            {
                new QwenChatMessage
                {
                    role = "system",
                    content =
                        @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》跑团模组的【对象类型判定器】。
你的职责是判断一个对象是否为【NPC（非玩家角色）】。
你只做判断与抽取，不进行创作、不补全、不推理隐藏信息。"
                },

                new QwenChatMessage
                {
                    role = "user",
                    content =
                        $@"【对象名称】
{objectName}

【对象描述】
{objectDesc}

【判定规则】
- NPC 必须是明确的单一人物
- 必须在剧情中以“人”的身份出现
- 排除：怪物、神祇、组织、地点、物品、概念性存在
- 如果描述不足但可以确认是人，仍可判定为 NPC
- 如果无法确认，IsNpc 必须为 false

【输出要求】
- 严格使用 JSON
- 严格符合 Schema
- 不输出任何额外说明文字

Schema：
{schema}"
                }
            };

            var check =
                await GameFrameWork.Instance.GptSystem
                    .ChatToGPT<GptNpcCheckResult>(messages);

            if (check != null && check.IsNpc && !string.IsNullOrEmpty(check.Name))
            {
                result[check.Name] = check.Description ?? string.Empty;
            }
        }

        return result;
    }

    
    public async Task<Dictionary<string, string>> getMoreNpc(
        string coc,
        Dictionary<string, string> npcs
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptSecondaryNpcResult));

        // 把主 NPC 列表变成 GPT 可读文本
        StringBuilder npcContext = new StringBuilder();
        foreach (var kv in npcs)
        {
            npcContext.AppendLine($"- {kv.Key}：{kv.Value}");
        }

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤》世界中的社会关系扩展器。
你的任务是：在不改变既有世界事实的前提下，补充合理存在的次要人物。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【模组文本】
{coc}

【已确定的主要 NPC（不可修改）】
{npcContext}

【你的任务】
- 围绕上述主要 NPC，生成他们【现实中合理会认识的全部次要人物】
- 包括：全部的家人、全部朋友、全部下属、全部帮手、全部雇佣员工、长期往来者等等
- 不允许改变或补充主要 NPC 的既有行为和秘密

【次要 NPC 要求】
- 身份应贴近现实生活
- 描述应简洁、事实导向
- 不要描述他们的死亡或结局
- 总数为20-30个

【输出要求】
- 严格使用 JSON
- 严格符合以下 Schema
- 不要输出任何额外文字

Schema：
{schema}"
            }
        };

        GptSecondaryNpcResult result =
            await GameFrameWork.Instance.GptSystem
                .ChatToGPT<GptSecondaryNpcResult>(messages);

        Dictionary<string, string> dict = new Dictionary<string, string>();

        if (result?.Npcs != null)
        {
            foreach (var npc in result.Npcs)
            {
                if (string.IsNullOrEmpty(npc?.Name)) continue;

                // 防止名字冲突覆盖主 NPC
                if (npcs.ContainsKey(npc.Name)) continue;

                dict[npc.Name] = npc.Description;
            }
        }

        return dict;
    }
    [Button]
    public async Task<(Dictionary<string, string>, Dictionary<string, string>)> GetNpcs(Dictionary<string, string> coc)
    {
        var res = await GenerateBaseNpcInfs(coc);
        var res2 = new Dictionary<string, string>();//await getMoreNpc(coc, res);
        return (res, res2);
    }

    [Button]
    public async Task<(Dictionary<string, NpcCreateInf>,Dictionary<string, NpcCreateInf>,List<SpaceCreatorRef>)> GetNpcDetails()
    {
        var coc = KPSystem.Load<Dictionary<string, string>>("数据字典");
        var res = await GetNpcs(coc);
        Debug.Log(1111);
        return (null,null,null);
        // var detailRes1 = await CreateNpcInfo(coc,res.Item1);
        // var detailRes2 = await CreateNpcInfo(coc, res.Item2);
        // var spaces = await GenerateSpaces(coc,(detailRes1,detailRes2));
        // GameFrameWork.Instance.data.saveFile.AddCfgSaveData(detailRes1, detailRes2, spaces);
        // return (detailRes1, detailRes2,spaces);
    }
    public class GptSpaceGenerateResult
    {
        public List<SpaceCreator> spaces;
    }

    public static async Task<List<SpaceCreatorRef>> GenerateSpaces(
    string cocText,
    NpcCreateInf npc,
    List<SpaceCreatorRef> spaces)
{
    if (npc == null)
        return spaces ?? new List<SpaceCreatorRef>();

    spaces ??= new List<SpaceCreatorRef>();

    // ========= 1. 现有地点摘要（给 GPT 判断是否“够用”） =========
    var existingSpaceText = spaces.Count == 0
        ? "（当前尚未生成任何地点）"
        : string.Join("\n", spaces.Select(s =>
$@"- 地点名：{s.name}
  描述：{s.detail}
  可直达地点：{(s.spaces.Count == 0 ? "无" : string.Join("，", s.spaces.Select(x => x.name)))}"));

    // ========= 2. NPC 行动约束摘要 =========
    var npcConstraintText = $@"- NPC：{npc.name}
  性格特点：{npc.personality}
  行为决策核心：{npc.decisionCore}
  过去的重要行为：{npc.historyBehave}
  当前所处位置：{(string.IsNullOrEmpty(npc.nowPlace) ? "未知" : npc.nowPlace)}
  居住地：{(string.IsNullOrEmpty(npc.belong) ? "未明确" : npc.belong)}
  工作或职责相关地点：{(string.IsNullOrEmpty(npc.work) ? "未明确" : npc.work)}
  重要社会关系：{(
      npc.relationships == null || npc.relationships.Count == 0
          ? "未明确"
          : string.Join("；", npc.relationships.Keys)
  )}";

    // ========= 3. GPT Schema =========
    var schema = GptSchemaBuilder.BuildSchema(typeof(GptSpaceGenerateResult));

    var messages = new List<QwenChatMessage>
    {
        new QwenChatMessage
        {
            role = "system",
            content =
@"你是一名《克苏鲁的呼唤（Call of Cthulhu）》跑团模组的【地点补全分析器】。

你不会重写世界结构。
你不会优化、润色或重命名已有地点。
你只做一件事：

👉 判断【现有地点是否足以支撑该 NPC 的合理行动】
👉 若不足，仅补充【最少数量】的必要地点

如果现有地点已经足够：
- 返回空的 spaces 数组

你不描述剧情，不引入新设定，不使用修辞语言。"
        },

        new QwenChatMessage
        {
            role = "user",
            content =
$@"【CoC 角色与世界文本】
{cocText}

━━━━━━━━━━━━━━━━━━━━
【NPC 行动约束（必须被满足）】
━━━━━━━━━━━━━━━━━━━━
{npcConstraintText}

━━━━━━━━━━━━━━━━━━━━
【当前已存在的地点结构】
━━━━━━━━━━━━━━━━━━━━
{existingSpaceText}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━

1️⃣ 判断：现有地点是否已足以支持该 NPC 在后续故事中的合理行动  
2️⃣ 若不足，只补充【缺失的、不可替代的地点】  
3️⃣ 若不需要补充，返回空数组

━━━━━━━━━━━━━━━━━━━━
【补充地点强制规则】
━━━━━━━━━━━━━━━━━━━━

- 只能生成【新增地点】
- 不得重复或改写已有地点
- 新地点必须能明确说明：
  👉 该 NPC 为什么“可能会去”
- 新地点数量必须尽可能少（0 是完全合法结果）

━━━━━━━━━━━━━━━━━━━━
【结构与连接规则】
━━━━━━━━━━━━━━━━━━━━

- 新地点必须符合层级逻辑
- spaces 只填写“物理上可直接到达”的地点
- 可与已有地点建立连接

━━━━━━━━━━━━━━━━━━━━
【输出要求】
━━━━━━━━━━━━━━━━━━━━

请严格返回 JSON：

public class GptSpaceGenerateResult
{{
    public List<SpaceCreator> spaces;
}}

public class SpaceCreator
{{
    public string name;
    public string detail;
    public List<string> spaces;
}}

⚠️ 若无需补充，请返回：
{{
  ""spaces"": []
}}

⚠️ 不要添加解释性文字  
⚠️ 不要生成 NPC  
⚠️ 返回 JSON 必须可直接反序列化  

JSON Schema：
{schema}"
        }
    };

    // ========= 4. 调 GPT =========
    var gptResult = await GameFrameWork.Instance.GptSystem
        .ChatToGPT<GptSpaceGenerateResult>(messages);

    if (gptResult?.spaces == null || gptResult.spaces.Count == 0)
        return spaces;

    // ========= 5. 合并新增地点 =========
    var spaceMap = spaces.ToDictionary(s => s.name, s => s);

    foreach (var node in gptResult.spaces)
    {
        if (string.IsNullOrWhiteSpace(node.name))
            continue;

        if (!spaceMap.ContainsKey(node.name))
        {
            spaceMap[node.name] = new SpaceCreatorRef
            {
                name = node.name,
                detail = node.detail
            };
        }
    }

    // ========= 6. 处理相邻关系（允许连接到旧地点） =========
    foreach (var node in gptResult.spaces)
    {
        if (!spaceMap.TryGetValue(node.name, out var current))
            continue;

        if (node.spaces == null)
            continue;

        foreach (var neighborName in node.spaces)
        {
            if (!spaceMap.TryGetValue(neighborName, out var neighbor))
                continue;

            if (!current.spaces.Contains(neighbor))
                current.spaces.Add(neighbor);
        }
    }

    return spaceMap.Values.ToList();
}



    public class GptNpcCreateResult
    {
        public Dictionary<string, NpcCreateInf> npcs;
    }
    
    public static async Task<NpcCreateInf> CreateNpcInfo(
        string name,
        string description)
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(NpcCreateInf));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁跑团（CoC）模组中的【人物设定补全解析器】。
你的职责是：
- 基于提供的 NPC 名字与人物描述文本
- 在【不新增 NPC、不新增未暗示重要事实】的前提下
- 补全该 NPC 的结构化人物信息
你必须保持人物与原始描述一致，允许信息不完整。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
NPC 名字：
{name}

NPC 已知信息（来源文本）：
{description}

数据结构定义：
public struct RelationData
{{
    public string relation;   // 关系类型
    public string attitude;   // 对其态度
}}

public class NpcCreateInf
{{
    public string name;//姓名
    public string appearance;//外表
    public string sex;//性别
    public string decisionCore;//自己行动的核心逻辑
    public string historyBehave;//过去的经历
    public Dictionary<string,RelationData> relationships;//与其他人关系
    public string skillDetail;//自己的各种能力，例如特长和弱点
    public string belong;//自己的家在哪
    public string nowPlace;//当前所在地点
    public string work;//自己的工作
    public string personality;//人格特点
}}

生成规则：
- 只生成这一个 NPC
- 不新增其他 NPC（relationships 中只能引用文本中已出现的人物，否则为空）
- 不推进剧情
- 不使用第一人称
- 不确定的信息请使用“未知”“不明确”
- relationships 可以为空对象 {{}}，不要省略字段
- 所有字段必须存在


请严格返回 JSON，格式如下：
{schema}
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<NpcCreateInf>(messages);

        return result;
    }


}