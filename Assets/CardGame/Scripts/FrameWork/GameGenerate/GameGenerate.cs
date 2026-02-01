using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

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
    /// <summary>
    /// 列出基本的npc信息
    /// </summary>
    /// <returns></returns>
    [Button]
    public async Task<Dictionary<string, string>> GenerateBaseNpcInfs(string coc)
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptNpcBaseInfoResult));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》跑团模组的角色解析器。
你的职责是【信息抽取】，不是创作。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【模组文本】
{coc}

【任务要求】
- 从模组中识别所有【被提及】的 NPC
- 只基于文本内容，不允许添加任何原创设定
- 不要推断隐藏真相，不要补全未明确出现的信息
- 描述应是中立、事实导向、可作为世界设定锚点
- 若信息不足，请保持描述简短

【NPC 选择规则】
- 包括：剧情关键人物、事件推动者、重要线索来源
- NPC 总数通常在 15~30 人之间
- NPC 必须是单个角色，必须拆分为单人
【输出要求】
- 严格使用 JSON
- 严格符合以下 Schema
- 不要输出任何额外说明文字

Schema：
{schema}"
            }
        };

        GptNpcBaseInfoResult result =
            await GameFrameWork.Instance.GptSystem
                .ChatToGPT<GptNpcBaseInfoResult>(messages);

        // 转成你真正要的 Dictionary
        Dictionary<string, string> dict = new Dictionary<string, string>();

        if (result?.Npcs != null)
        {
            foreach (var npc in result.Npcs)
            {
                if (string.IsNullOrEmpty(npc?.Name)) continue;
                dict[npc.Name] = npc.Description ?? string.Empty;
            }
        }

        return dict;
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
    public async Task<(Dictionary<string, string>, Dictionary<string, string>)> GetNpcs(string coc)
    {
        var res = await GenerateBaseNpcInfs(coc);
        var res2 = await getMoreNpc(coc, res);
        return (res, res2);
    }

    [Button]
    public async Task<(Dictionary<string, NpcCreateInf>,Dictionary<string, NpcCreateInf>,List<SpaceCreatorRef>)> GetNpcDetails()
    {
        var coc = KPSystem.Load("模组精简");
        var res = await GetNpcs(coc);
        var detailRes1 = await CreateNpcInfo(coc,res.Item1);
        var detailRes2 = await CreateNpcInfo(coc, res.Item2);
        var spaces = await GenerateSpaces(coc,(detailRes1,detailRes2));
        GameFrameWork.Instance.data.saveFile.AddCfgSaveData(detailRes1, detailRes2, spaces);
        return (detailRes1, detailRes2,spaces);
    }
    public class GptSpaceGenerateResult
    {
        public List<SpaceCreator> spaces;
    }

    public async Task<List<SpaceCreatorRef>> GenerateSpaces(
    string cocText,(Dictionary<string, NpcCreateInf>, Dictionary<string, NpcCreateInf>) npcs)
    {
        // ========= 1. 合并 NPC =========
        var npcs1 = npcs.Item1;
        var npcs2 = npcs.Item2;

        var allNpcs = new Dictionary<string, NpcCreateInf>();
        foreach (var kv in npcs1)
            allNpcs[kv.Key] = kv.Value;

        foreach (var kv in npcs2)
            allNpcs[kv.Key] = kv.Value; // 后者覆盖前者

        // ========= 2. 整理 NPC 约束摘要（给 GPT 用） =========
        var npcConstraintText = string.Join("\n", allNpcs.Values.Select(n =>
    $@"- 姓名：{n.name}
      描述：{n.npcInfo}
      居住地线索：{(string.IsNullOrEmpty(n.belong) ? "未明确" : n.belong)}
      工作或常去地点：{(string.IsNullOrEmpty(n.work) ? "未明确" : n.work)}"));

        // ========= 3. GPT Schema =========
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptSpaceGenerateResult));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
    @"你是一名克苏鲁跑团模组的【世界结构构建器】。
    你的职责是生成地点结构（Space），而不是剧情或事件。
    你必须保证所有 NPC 的生活与行动在地点结构中形成闭包。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
    $@"【CoC 模组文本】
    {cocText}

    【NPC 约束列表（必须被地点完全覆盖）】
    {npcConstraintText}

    你的任务：
1. 基于 CoC 模组文本生成所有重要地点
2. 如果模组中出现“大地点”（如：小镇、医院、学校、矿区、庄园）
   必须将其拆分为多个【可独立行动的小地点】
3. 对每一个 NPC：
   - 必须存在至少一个符合其身份的【居住地点】
   - 若其描述中暗示了工作或常去地点，这些地点必须存在
4. 地点之间必须具有明确、合理、可推理的相邻关系（可步行 / 可到达）

====================
【空间结构强制规则】
====================

1. 地点必须以【n 级展开的目录结构】来组织：
   - 大地点本身不可作为最终行动地点
   - 大地点必须拆分为更具体的子地点
   - 子地点可以继续向下拆分，直到形成“可以发生具体行动”的地点

   示例（仅示意结构，不是让你照抄）：
   - 某某庄园
     - 主楼
       - 客厅
       - 书房
     - 后院
     - 厨房

2. 所有地点必须处于某一层级之中：
   - 不允许出现层级来源不明的孤立地点
   - 不允许出现“逻辑上属于某地点，但未挂载”的地点

====================
【路径与相邻关系规则】
====================

1. spaces 表示【物理上可直接移动到的地点】
   - 移动必须符合空间层级逻辑
   - 不允许跨越多个层级直接相连

   示例：
   - 可以：书房 ↔ 客厅 ↔ 主楼
   - 不可以：书房 ↔ 庄园大门（跳过中间层）

2. 同一父级下的地点可以互相连接  
   上下层地点只能与其直接父级或子级相连

3. 任意地点的移动路径必须是：
   - 连续的
   - 可被推理的
   - 不依赖隐含或未说明的通道

====================
【输出要求】
====================

返回一系列的地点，对每个地点，请提供：
- name：地点名称（应体现其层级归属，例如包含上级语义）
- detail：客观、静态描述（不推进剧情）
- spaces：从此地点可以直接前往的其他地点名称，只是一个name用来索引

====================
【禁止事项】
====================

- 不生成 NPC
- 不推进剧情
- 不使用第一人称
- 不生成模组文本中完全不存在或无法合理推断的地点
- 地点集合必须形成对 NPC 行为的【完整闭包】
- 不要超过30个
请严格使用 JSON 返回，不要添加解释性文字。
返回的JSON结构必须能被映射成GptSpaceGenerateResult。
其中
public class GptSpaceGenerateResult
{{
    public List<SpaceCreator> spaces;
}}
public class SpaceCreator
{{
    public string name;
    public string detail;
    /// <summary>
    /// 所有相邻可以去的区域
    /// </summary>
    public List<string> spaces=new();
}}

    请严格使用 JSON 返回：
    {schema}"
            }
        };

        // ========= 4. 调 GPT =========
        var gptResult = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GptSpaceGenerateResult>(messages);

        if (gptResult?.spaces == null || gptResult.spaces.Count == 0)
            return new List<SpaceCreatorRef>();

        // ========= 5. 构建 SpaceCreator 对象 =========
        var spaceMap = new Dictionary<string, SpaceCreatorRef>();

        foreach (var node in gptResult.spaces)
        {
            if (!spaceMap.ContainsKey(node.name))
            {
                spaceMap[node.name] = new SpaceCreatorRef
                {
                    name = node.name,
                    detail = node.detail
                };
            }
        }

        // ========= 6. 处理相邻关系 =========
        foreach (var node in gptResult.spaces)
        {
            var current = spaceMap[node.name];

            if (node.spaces == null) continue;

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
    
    public async Task<Dictionary<string, NpcCreateInf>> CreateNpcInfo(string cocText,Dictionary<string, string> allNpcs)
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptNpcCreateResult));

        var npcBaseList = string.Join("\n", allNpcs.Select(kv =>
            $@"- 姓名：{kv.Key}
  模组中的描述：{kv.Value}"));
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁跑团模组中的人物设定解析器。
你的任务是：
- 基于 CoC 模组文本
- 在【不新增 NPC】的前提下
- 补全每个 NPC 的完整人物信息
你必须保持人物设定与模组文本一致，不得编造违背原文的重要事实。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【已存在的 NPC（禁止新增或删除）】
{npcBaseList}
public struct RelationData
{{
    public string relation;
    public string attitude;
}}
请为每一个 NPC 生成完整信息，字段说明如下：

- name：NPC 的名字
- npcInfo：对 NPC 的整体客观描述
- sex：性别
- aim：当前最重要的个人目标或执念
- historyBehave：过去发生过的关键行为或事件
- relationships：与其他 NPC 或调查员的已知关系数据结构为Dictionary<string,RelationData>,key表示npc的名字，value中的relation参数为和他的关系，attitude参数为对他的态度
- skillDetail：CoC 能力或擅长领域的文字描述
- belong：居住地或长期停留地点
- work：职业或社会角色
- mentalState：当前心理状态（偏执、恐惧、冷漠等）

规则：
- 不要生成新的 NPC
- 不要加入模组中未暗示的重要背景
- 允许使用“不明确”“未知”等描述
- 不要推进剧情
- 不使用第一人称

请严格使用 JSON 格式返回：
{schema}"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GptNpcCreateResult>(messages);

        return result?.npcs ?? new Dictionary<string, NpcCreateInf>();
    }

}