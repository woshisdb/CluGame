// ===== 阶段一：区域划分结果 =====

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GptRegionGenerateResult
{
    public List<RegionCreator> regions;
}

public class RegionCreator
{
    public string name;                 // 区域名（城镇 / 地区）
    public string detail;               // 区域说明
    public List<string> contains;       // 原始 SpaceCardConfig.title
}


public class SpaceNode
{
    public string name;
    public string detail;
    // 叶子空间（原始 SpaceCard）
    public List<string> leafSpaces = new();
}


public class GptClusterResult
{
    public List<SpaceNode> clusters;
}


public class KPSpaceGen
{
    public static async Task<GptClusterResult> AskGptToCluster(
        List<SpaceCardConfig> spaces)
    {
        var sb = new StringBuilder();
        foreach (var s in spaces)
        {
            sb.AppendLine($"名字：{s.title}｜描述：{s.descirption}");
        }

        var messages = new List<QwenChatMessage>
        {
            new()
            {
                role = "system",
                content =
                    @"你是《克苏鲁的呼唤》模组的【地理区域划分器】。

你的职责是：
- 根据地点的【地理位置、空间相邻性、现实可达性】
- 将一组地点划分为【少量合理的大区域】

你不是在写剧情，而是在做地图结构决策。"
            },
            new()
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【原始地点（不可新增 / 不可修改）】
━━━━━━━━━━━━━━━━━━━━
{sb}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━
- 将以上地点划分为【1～3 个】地理区域
- 区域粒度由你自行判断（城市 / 地区 / 地带等）
- 每个区域必须在【现实地理上合理连通】
- 所有地点必须且只能归属一个区域

━━━━━━━━━━━━━━━━━━━━
【严格规则】
━━━━━━━━━━━━━━━━━━━━
- ❌ 不允许生成新的地点
- ❌ 不允许拆分已有地点
- ❌ 不允许遗漏任何地点
- ❌ 不允许输出未给定的名字
- 区域名称必须像真实地图中的地名或区域名

━━━━━━━━━━━━━━━━━━━━
【返回数据结构（严格遵守）】
━━━━━━━━━━━━━━━━━━━━

public class GptClusterResult
{{
    public List<SpaceNode> clusters;
}}

public class SpaceNode
{{
    public string name;
    public string detail;
    public List<string> leafSpaces;
}}

━━━━━━━━━━━━━━━━━━━━
【输出要求】
━━━━━━━━━━━━━━━━━━━━
- 只输出【纯 JSON】
- 不要解释
- 不要代码块
- 可直接反序列化
"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GptClusterResult>(messages);
    }
    
    public static async Task<GptClusterResult> ClusterRegionToSubAreas(
    SpaceNode regionNode,
    List<SpaceCardConfig> allSpaces)
{
    // 取出该区域内的 SpaceCard
    var regionSpaces = allSpaces
        .Where(s => regionNode.leafSpaces.Contains(s.title))
        .ToList();

    if (regionSpaces.Count <= 2)
    {
        return new GptClusterResult
        {
            clusters = new List<SpaceNode>
            {
                new SpaceNode
                {
                    name = regionNode.name,
                    detail = "区域规模较小，未进一步细分",
                    leafSpaces = regionNode.leafSpaces
                }
            }
        };
    }

    var sb = new StringBuilder();
    foreach (var s in regionSpaces)
    {
        sb.AppendLine($"名字：{s.title}｜描述：{s.descirption}");
    }

    var messages = new List<QwenChatMessage>
    {
        new()
        {
            role = "system",
            content =
@"你是一名【TRPG 地图绘制员】而不是作家。

你负责给调查员和 NPC 使用的【实体地图分区命名】。
你生成的名字，必须像“路牌 / 地图标注”，而不是章节标题。"
        },
        new()
        {
            role = "user",
            content = $@"
━━━━━━━━━━━━━━━━━━━━
【所属大区域】
━━━━━━━━━━━━━━━━━━━━
{regionNode.name}
{regionNode.detail}

━━━━━━━━━━━━━━━━━━━━
【区域内地点（不可新增 / 不可删除）】
━━━━━━━━━━━━━━━━━━━━
{sb}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━
- 将以上地点划分为更紧密的【子区域】
- 子区域必须是【现实中可步行理解的地理范围】
- 每个地点必须且只能属于一个子区域
- 如果某子区域包含地点超过 5 个，必须继续拆分

━━━━━━━━━━━━━━━━━━━━
【子区域命名强制规则（极其重要）】
━━━━━━━━━━━━━━━━━━━━
子区域 name 必须满足以下所有条件：

✅ 是【具体、可画在地图上的地理名词】
✅ 能被 NPC 用在一句话中指路  
   （例如：『他在 **XX 农场北侧田地** 被看到过』）

❌ 禁止使用以下类型词汇：
- 抽象 / 心理 / 主题词（如：神圣、创伤、信仰、生活、休憩）
- 叙事 / 剧情词（如：行动、遗存、阶段、双域）
- 规划用语（功能区、作业带、复合区）
- 破折号 / 双概念拼接命名

✅ 推荐命名模式（示例）：
- XX 农场主屋区
- XX 农场田地区
- 杂货店后仓
- 镇中心街区
- 河岸林地
- 森林边缘区

━━━━━━━━━━━━━━━━━━━━
【严格规则】
━━━━━━━━━━━━━━━━━━━━
- ❌ 不允许生成新的地点
- ❌ 不允许室内 / 房间级空间
- ❌ 不允许任何剧情、象征或情绪描述
- 子区域必须在现实地理上【可行走、可定位】

━━━━━━━━━━━━━━━━━━━━
【返回结构（严格遵守）】
━━━━━━━━━━━━━━━━━━━━

public class GptClusterResult
{{
    public List<SpaceNode> clusters;
}}

public class SpaceNode
{{
    public string name;
    public string detail;
    public List<string> leafSpaces;
}}

━━━━━━━━━━━━━━━━━━━━
【输出要求】
━━━━━━━━━━━━━━━━━━━━
- 只输出【纯 JSON】
- 不要解释
- 不要代码块
- 必须可直接反序列化
"
        }
    };

    return await GameFrameWork.Instance.GptSystem
        .ChatToGPT<GptClusterResult>(messages);
}

    
    public static async Task<List<SpaceNode>> GeneratePlayableMap(
        List<SpaceCardConfig> allSpaces)
    {
        var retAll = new List<SpaceNode>();
        var ret = await AskGptToCluster(allSpaces);
        var root = new SpaceNode();
        root.name = "世界";
        root.detail = "根节点";
        retAll.Add(root);
        foreach (var x in ret.clusters)
        {
            root.leafSpaces.Add(x.name);
            retAll.Add(new SpaceNode()
            {
                name = x.name,
                detail = x.detail
            });
        }
        foreach (var spc in ret.clusters)
        {
            var t=await ClusterRegionToSubAreas(spc, allSpaces);
            var nowRet = retAll.Find(e =>
            {
                return e.name == spc.name;
            });
            foreach (var x in t.clusters)
            {
                nowRet.leafSpaces.Add(x.name);
            }
            retAll.AddRange(t.clusters);
            foreach (var x in t.clusters)
            {
                foreach (var y in x.leafSpaces)
                {
                    var tt = allSpaces.Find(e =>
                    {
                        return e.title == y;
                    });
                    retAll.Add(new SpaceNode()
                    {
                        name = y,
                        detail = tt.descirption
                    });
                }
            }
            Debug.Log(2);
        }
        Debug.Log(11);
        return retAll;
    }
    
    public static List<SpaceCardConfig> BuildSpaceConfig(List<SpaceNode> spaceNodes)
    {
        var bindRet = new List<SpaceCreatorRef>();
        foreach (var x in spaceNodes)
        {
            var nex = new SpaceCreatorRef()
            {
                name = x.name,
                detail = x.detail,
            };
            bindRet.Add(nex);
        }

        foreach (var x in spaceNodes)
        {
            var y = bindRet.Find(e => { return e.name == x.name; });
            if (y!=null)
            {
                foreach (var node in x.leafSpaces)
                {
                    var spMap = bindRet.Find(e =>
                    {
                        return e.name == node;
                    });
                    if (spMap!=null)
                    {
                        y.spaces.Add(spMap);
                    }
                }
            }
        }

        var realRet = new List<SpaceCardConfig>();
        foreach (var x in bindRet)
        {
            realRet.Add(x.CreateCfg());
        }
        Debug.Log(111);
        return realRet;
    }
    public static async Task<List<SpaceNode>> ReThinkSpace(
    List<SpaceNode> nodes,
    List<NpcCreateInf> npcs)
{
    // 用于快速查重
    var spaceNameSet = nodes.Select(n => n.name).ToHashSet();

    foreach (var npc in npcs)
    {
        // === 1️⃣ 构造地图摘要 ===
        var mapSummary = new StringBuilder();
        foreach (var n in nodes)
        {
            mapSummary.AppendLine($"- {n.name}：{n.detail}");
        }

        // === 2️⃣ GPT 判断缺失地点 ===
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是 CoC 模组的【世界一致性审查员】。

你的职责是：
判断 NPC 的后续故事发展是否缺少【必要但合理的地点】。

你不是补全现实世界，而是保证故事能继续发生。"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【NPC 信息】
━━━━━━━━━━━━━━━━━━━━

姓名：{npc.name}
描述：{npc.historyBehave}

━━━━━━━━━━━━━━━━━━━━
【当前地图已有地点】
━━━━━━━━━━━━━━━━━━━━
{mapSummary}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━
- 判断：在后续故事中，这名 NPC 是否会合理地需要某些地点
- 这些地点当前是否在地图中缺失
- 只在【缺失会影响故事推进】时，才提出新增地点

━━━━━━━━━━━━━━━━━━━━
【严格规则】
━━━━━━━━━━━━━━━━━━━━
- ❌ 不要为了“完整人生”而补地点
- ❌ 不要因为现实常识强行补（如：人人都有家）
- ✅ 只考虑剧情可能实际发生的场景
- ❌ NPC 死亡后不补新地点

━━━━━━━━━━━━━━━━━━━━
【返回格式（严格 JSON）】
━━━━━━━━━━━━━━━━━━━━

{{
  ""missingSpaces"": [
    {{
      ""name"": ""地点名称"",
      ""detail"": ""为什么这个地点对后续故事是必要的""
    }}
  ]
}}

- 允许 missingSpaces 为空数组
- 不要解释
- 不要代码块
"
            }
        };


        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<MissingSpaceResult>(messages);

        if (result?.missingSpaces == null)
            continue;

        // === 3️⃣ 安全合并到地图 ===
        foreach (var ms in result.missingSpaces)
        {
            if (spaceNameSet.Contains(ms.name))
                continue;

            var newNode = new SpaceNode
            {
                name = ms.name,
                detail = ms.detail,
                leafSpaces = new List<string>()
            };

            nodes.Add(newNode);
            spaceNameSet.Add(ms.name);
        }
    }

    return nodes;
}
    
    //     public async Task<string> DetailSpaceInfo(string spaceTxt, string name,CocDicItem npcItem)
//     {
//         var messages = new List<QwenChatMessage>
//         {
//             new QwenChatMessage
//             {
//                 role = "system",
//                 content =
//                     @"你是《克苏鲁的呼唤（Call of Cthulhu）》跑团中的【NPC 场景补全分析器】。
//
// 你的任务不是扩写世界，而是判断：
// 在【已有地形结构】下，
// 某个 NPC 是否存在【合理但缺失的可跑团地点】。"
//             },
//             new QwenChatMessage
//             {
//                 role = "user",
//                 content = $@"━━━━━━━━━━━━━━━━━━━━
// 【输入：既有地点结构（只作为信息来源）】
// ━━━━━━━━━━━━━━━━━━━━
// 以下文本描述了当前世界中已经存在的地点。
//
// ⚠️ 这些地点必须【全部出现在最终输出中】，
// 但你【可以调整层级呈现方式】以符合结构化输出要求。
//
// 原文开始：
// --------------------
// {spaceTxt}
// --------------------
// 原文结束
//
// ━━━━━━━━━━━━━━━━━━━━
// 【NPC 信息】
// ━━━━━━━━━━━━━━━━━━━━
// 姓名：{name}
// 描述：
// {npcItem.description}
//
// ━━━━━━━━━━━━━━━━━━━━
// 【你的任务】
// ━━━━━━━━━━━━━━━━━━━━
// 1. 判断该 NPC 在跑团中是否存在【不可替代但缺失的地点】
//    - 用于生活、行动、秘密、事件触发或后果
//    - 不能被现有地点合理替代
//
// 2. 在【不删除任何已有地点】的前提下：
//    - 将已有地点 + NPC 新增地点
//    - 整合为一个【完整的、层级清晰的地点结构】
//
// ━━━━━━━━━━━━━━━━━━━━
// 【输出格式（必须严格遵守，纯文本）】
// ━━━━━━━━━━━━━━━━━━━━
// X地区名：
// - Y城镇名：
//   - 功能区名：
//     - 子区域名：
//       - 具体地点名
//     - 具体地点名
// Z地区名...
//
// ━━━━━━━━━━━━━━━━━━━━
// 【输出要求（不可违反）】
// ━━━━━━━━━━━━━━━━━━━━
// - 必须只生成场景的名字，不允许其他的任何东西
// - 只输出结构化文本
// - 不要代码块
// - 不要解释
// - 不要总结
// - 不要合并地点
// - 所有 spaceTxt 中的地点必须出现
// - NPC 新地点只能是“不可替代”的，数量极少
// - 保持层级稳定、清晰、可解析
// "
//
//             }
//         };
//
//         var result = await GameFrameWork.Instance.GptSystem
//             .ChatToGPT(messages);
//
//         return result;
//     }

    public static async Task<string> CombineSpaceInfo(string addPlaceTxt, string finalPlaceTxt)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是跑团地图“空间标准化引擎”。
你的任务是处理添加的地点，但必须严格控制添加的粒度。
"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【当前完整结构】
━━━━━━━━━━━━━━━━━━━━
{finalPlaceTxt}

━━━━━━━━━━━━━━━━━━━━
【待标准化地点列表】
━━━━━━━━━━━━━━━━━━━━
{addPlaceTxt}

━━━━━━━━━━━━━━━━━━━━
【标准化规则】
━━━━━━━━━━━━━━━━━━━━

第一步：去人物标签
例如：
“约翰·梅里特居住地点（溪畔松木小屋）”
→ 溪畔松木小屋

第二步：删除行为类抽象地点
例如：
- 常拦车处
- 醉卧点
- 目标地点
- 经常活动地点
- 巡查路径

如果没有明确物理结构，删除。

第三步：
对于要添加的空间，尝试进行合并
例如：
- 某某棚屋前廊
- 某某棚屋客厅
→ 某某棚屋

- 某某农舍卧室
→ 某某农舍

第四步：去重
如果标准化后的地点已经存在于当前完整结构中，则进行合并。

第五步：输出
只输出四步之后的完整地点结构
不输出解释
不输出说明

━━━━━━━━━━━━━━━━━━━━
【最终输出格式】
━━━━━━━━━━━━━━━━━━━━

保持与原结构完全一致的层级格式：

X地区名：
- Y城镇名：
  - 功能区名：
    - 子区域名：
      - 具体地点名
    - 具体地点名
Z地区名：
- W城镇名：
  - 功能区名：
...

只输出结构。
不要输出每一步你做了什么，只需要输出结构
"
            }
        };

        return await GameFrameWork.Instance.GptSystem.ChatToGPT(messages);
    }

    public static async Task<string> OutPersonPlaceInfo(string finalPlaceTxt, string name, CocDicItem npcItem)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是跑团中的 NPC 地点归属分析器。
你不能创建新地点，只能从已有结构中选择。"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
【完整世界结构】
{finalPlaceTxt}

【NPC 信息】
姓名：{name}
描述：
{npcItem.description}

任务：

根据描述匹配：
- 居住地点
- 工作地点
- 经常去的地点
- 当前所在
- 目标地点

如果该角色已死亡或为不重要角色：
输出：无需生成地点信息

【输出格式】

姓名：{name}
- 居住地：XXX
- 工作地：XXX
- 常去地点：
  - XXX
  - XXX
- 当前所在：XXX
- 目标地点：XXX（若无写 无）

禁止新增地点。
禁止解释。"
            }
        };

        return await GameFrameWork.Instance.GptSystem.ChatToGPT(messages);
    }
    
    public static async Task<string> DetailSpaceInfo(string finalPlaceTxt, string name, CocDicItem npcItem)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是《克苏鲁的呼唤》跑团世界中的【NPC 地点缺失分析器】。
你只负责判断地点是否缺失，不负责合并。"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
【当前世界完整结构】
{finalPlaceTxt}

【NPC 信息】
姓名：{name}
描述：
{npcItem.description}

【任务】

请判断以下地点是否存在于当前结构中：
- 工作地点
- 居住地点
- 经常活动地点
- 目标地点
- 隐秘地点

规则：
1. 已存在 → 不输出
2. 不存在但对故事不可替代 → 输出
3. 已死亡 / 不重要角色 → 输出：无新增地点
4. 地点对剧情不重要 → 可不新增

【输出格式】

若无新增地点：
无新增地点

若有：
- 完整层级路径｜具体地点名｜用途或风险说明
- 完整层级路径｜具体地点名｜用途或风险说明

禁止解释。"
            }
        };

        return await GameFrameWork.Instance.GptSystem.ChatToGPT(messages);
    }
    
    public static async Task<List<SpaceNode>> GenerateSpaceNodesFromGPT(string spaceTxt)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个数据结构转换器。

任务：
将给定的【层级地点文本】转换为标准 JSON 结构：

规则：
1. 输出必须是 List<SpaceNode>
2. 结构必须是合法 JSON
3. 不允许输出任何解释
4. 不允许添加字段
5. leafSpaces 必须是数组（即使为空）
6. 如果没有子节点，leafSpaces 返回 []"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
以下是层级地点文本：

{spaceTxt}

请转换为：

List<SpaceNode>

结构定义：

public class SpaceNode
{{
    public string name;
    public string detail;
    // 叶子空间（原始 SpaceCard）
    public List<string> leafSpaces = new();
}}
其中leafSpaces的每个字符串只对应SpaceNode的name，不要有其他的
只输出 JSON。
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<SpaceNode>>(messages);
        // var retAll = new List<SpaceCreator>();
        // foreach (var x in result)
        // {
        //     retAll.Add(new SpaceCreator()
        //     {
        //         name = x.name,
        //         detail = x.detail,
        //         spaces = x.leafSpaces
        //     });
        // }
        return result;
    }
    
    public static async Task<string> GenerateMoreDetail()
    {
        var finalPlaceDetails = KPSystem.Load("生成地图的原始数据");
        var cocTxt = KPSystem.Load("模组精简");

        var prompt = $@"
===== 原始地图结构 =====
{finalPlaceDetails}

===== 模组文本 =====
{cocTxt}
====

你是一名COC模组结构分析器。

你的任务不是“删除”地点。
你的任务是：

从原始地图中筛选出：
- 模组中真正发生剧情的地点
- 开场发生的地点
- 玩家可以实际到达的地点
- 支线调查可进入的地点
- 有明确文本描写的地点
- 调查员有可能去的地方

然后：
重新组织这些有效地点，
输出【最终有效场景树】。

注意：

1. 你只输出保留下来的地点。
2. 不要描述删除。
3. 不要提及删除。
4. 不要保留无剧情意义地点。
5. 开场地点必须保留。
6. 可选支线或可到达地点必须保留。
7. 输出必须是最终版本。
8. 有信息的地点必须要保留。

输出格式（严格遵守缩进）：

地区名：｜备注
- 城镇名：｜备注
  - 场景名：｜剧情功能
    - 子场景名：｜功能

禁止输出任何解释或分析。

现在输出最终有效场景树：
";


        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(new[]
            {
                new QwenChatMessage()
                {
                    role = "system",
                    content = "你是一个严格执行删除规则的结构优化器。输出必须是最终结构，不允许包含被删除节点。"
                },
                new QwenChatMessage()
                {
                    role = "user",
                    content = prompt
                }
            });
        
        return result;
    }
}

public class PlaceImportance
{
    public string PlaceName;
    public int Score;          // 0-10
    public string Reason;      // 简短说明
    public bool ExistsInModule; // 是否出现在模组中
}
