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

}