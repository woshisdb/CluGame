// ===== 阶段一：区域划分结果 =====

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

public class KPSpaceGen
{
    public static async Task<GameGenerate.GptSpaceGenerateResult>
        GenerateWorldRegions(List<SpaceCardConfig> allSpaces)
    {
        var dump = new StringBuilder();
        foreach (var s in allSpaces)
        {
            dump.AppendLine($"名字：{s.title}｜描述：{s.descirption}");
        }

        var messages = new List<QwenChatMessage>
        {
            new()
            {
                role = "system",
                content =
                    @"你是《克苏鲁的呼唤》模组的【世界地图区域生成器】。
你的任务是：从一组地点中，生成【可行走的大地图区域结构】。"
            },
            new()
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【已有地点（不可删除）】
━━━━━━━━━━━━━━━━━━━━
{dump}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━
1️⃣ 构建世界级地图结构  
2️⃣ 只允许生成：
   - 世界
   - 区域 / 城镇 / 地区
3️⃣ 不允许生成建筑、房间、室内空间  

━━━━━━━━━━━━━━━━━━━━
【强制规则】
━━━━━━━━━━━━━━━━━━━━
- 所有原始地点必须【归属到某个区域】
- 每个区域必须包含至少一个原始地点
- 地图必须是联通的
- 允许新增“世界”作为根节点

━━━━━━━━━━━━━━━━━━━━
【数据结构（严格遵守）】
━━━━━━━━━━━━━━━━━━━━

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

━━━━━━━━━━━━━━━━━━━━
【严格输出规则】
━━━━━━━━━━━━━━━━━━━━
- 只允许输出【纯 JSON】
- ❌ 不允许出现 ```、```json、代码块标记
- ❌ 不允许任何解释性文字
- ❌ 不允许输出示例
- 输出内容必须可以被直接反序列化

━━━━━━━━━━━━━━━━━━━━
【spaces 字段特别说明】
━━━━━━━━━━━━━━━━━━━━
- spaces 只能是字符串数组
- ❌ 禁止在 spaces 中输出对象
"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GameGenerate.GptSpaceGenerateResult>(messages);
    }

    public static async Task<GameGenerate.GptSpaceGenerateResult>
        GenerateRegionDetailMap(
            string regionName,
            List<SpaceCardConfig> regionSpaces)
    {
        var dump = new StringBuilder();
        foreach (var s in regionSpaces)
        {
            dump.AppendLine($"名字：{s.title}｜描述：{s.descirption}");
        }

        var messages = new List<QwenChatMessage>
        {
            new()
            {
                role = "system",
                content =
                    @"你是《克苏鲁的呼唤》模组的【区域地图细化器】。
你负责把一个区域拆解为可调查、可进入的物理空间结构。"
            },
            new()
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【当前区域】
━━━━━━━━━━━━━━━━━━━━
{regionName}

━━━━━━━━━━━━━━━━━━━━
【区域内已有地点（不可删除）】
━━━━━━━━━━━━━━━━━━━━
{dump}

━━━━━━━━━━━━━━━━━━━━
【构建规则】
━━━━━━━━━━━━━━━━━━━━
- 父空间必须在物理上包含子空间
- spaces 只表示【直接可到达】
- 禁止抽象 / 剧情 / 时间关系
- 禁止循环结构
- 地图必须是联通的
- 不允许任何地点成为孤立节点
- 允许新增【街区 / 建筑群 / 容器空间】

━━━━━━━━━━━━━━━━━━━━
【数据结构（严格遵守）】
━━━━━━━━━━━━━━━━━━━━

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

━━━━━━━━━━━━━━━━━━━━
【严格输出规则】
━━━━━━━━━━━━━━━━━━━━
- 只允许输出【纯 JSON】
- ❌ 不允许出现 ```、```json、代码块标记
- ❌ 不允许任何解释性文字
- ❌ 不允许输出示例
- 输出内容必须可以被直接反序列化

━━━━━━━━━━━━━━━━━━━━
【spaces 字段特别说明】
━━━━━━━━━━━━━━━━━━━━
- spaces 只能是字符串数组
- 每个字符串必须等于某个 SpaceCreator.name
- ❌ 禁止在 spaces 中输出对象
"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GameGenerate.GptSpaceGenerateResult>(messages);
    }

    public static async Task<GameGenerate.GptSpaceGenerateResult>
        RebuildCocMapHierarchy(
            string cocText,
            List<SpaceCardConfig> allSpaces)
    {
        // ===== 1. 生成世界 → 区域 =====
        var worldMap = await GenerateWorldRegions(allSpaces);

        if (worldMap?.spaces == null || worldMap.spaces.Count == 0)
            return new GameGenerate.GptSpaceGenerateResult { spaces = new List<SpaceCreator>() };

        var finalMap = new Dictionary<string, SpaceCreator>();

        // ===== 2. 收集区域节点 =====
        var regionNodes = worldMap.spaces
            .Where(s => s.name != "世界")
            .ToList();

        // ===== 3. 建立世界根节点 =====
        var world = worldMap.spaces.FirstOrDefault(s => s.name == "世界")
                    ?? new SpaceCreator
                    {
                        name = "世界",
                        detail = "模组世界地图根节点",
                        spaces = new List<string>()
                    };

        finalMap[world.name] = world;

        // ===== 4. 逐区域细化 =====
        foreach (var region in regionNodes)
        {
            finalMap[region.name] = region;
            world.spaces.Add(region.name);

            // 找到该区域包含的原始地点
            var regionSpaceCards = allSpaces
                .Where(s => region.spaces.Contains(s.title))
                .ToList();

            var regionDetailMap = await GenerateRegionDetailMap(
                region.name,
                regionSpaceCards
            );

            if (regionDetailMap?.spaces == null)
                continue;

            // 合并节点
            foreach (var sp in regionDetailMap.spaces)
            {
                if (!finalMap.ContainsKey(sp.name))
                    finalMap[sp.name] = sp;
            }

            // 找区域内根节点
            var childSet = regionDetailMap.spaces
                .SelectMany(s => s.spaces ?? new List<string>())
                .ToHashSet();

            var roots = regionDetailMap.spaces
                .Where(s => !childSet.Contains(s.name))
                .Select(s => s.name);

            region.spaces = roots.ToList();
        }

        // ===== 5. 返回最终地图 =====
        return new GameGenerate.GptSpaceGenerateResult
        {
            spaces = finalMap.Values.ToList()
        };
    }

}