using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

// public class GptMapGenerateResult
// {
//     public List<MapNode> nodes;
// }
//
// public class MapNode
// {
//     public string name;              // 最终规范化后的地点名
//     public string originalName;      // 来自输入的原始地点名
//     public string level;             // world / area / building / room / hidden
//     public string description;       // 地点在 CoC 语境中的意义
//     public List<string> children;    // 下级地点
//     public List<string> connections; // 同层可直达地点
// }


public static class KPNPCDetail
{
    public static async Task<string> GPTGetNpcMoreDescription(
        string text,
        string name,
        string description,
        List<string> teams
    )
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
@"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【NPC 阵营与状态解析器】。

你的职责是：
基于故事文本，判断一个 NPC 在世界中的真实状态与立场。

你不写剧情，
你不补全设定，
你只解析已经存在的信息。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
$@"
【NPC 名称】
{name}

━━━━━━━━━━━━━━━━━━━━
【NPC 已知描述】
{description}

━━━━━━━━━━━━━━━━━━━━
【相关故事文本】
{text}

━━━━━━━━━━━━━━━━━━━━
【当前已知阵营（思想集合）】
{(teams == null || teams.Count == 0 ? "（暂无）" : string.Join("\n", teams.Select(t => "- " + t)))}

━━━━━━━━━━━━━━━━━━━━
【你的任务（严格执行）】

基于文本中【明确、直接出现的信息】判断并输出：

1. NPC 当前是否存活
2. NPC 明确属于哪些阵营
3. NPC 在每个阵营中的位置
4. NPC 对该阵营的真实态度
5. 若文本中【明确出现但未在已有阵营列表中的阵营名称】，列为“新增阵营”

━━━━━━━━━━━━━━━━━━━━
【阵营判定铁律（必须遵守）】

⚠ 阵营【必须】满足以下条件，否则不得输出：

- 阵营名称在文本中被【直接提及或明确指代】
- 或与【当前已知阵营名称完全一致】
- 阵营具备【可持续行动能力】

⚠ 以下内容【绝对禁止】作为阵营：

- 职业、身份类别（如“私酒贩”“中间人”“行政人员”）
- 时代背景或制度（如“禁酒令时期”“学术体系”）
- 推断出的幕后结构
- 抽象社会压力或隐含秩序
- 未被文本命名的集合

⚠ 若无法确认阵营是否存在：
- 不要新增
- 不要概括
- 不要重命名
- 不要输出

━━━━━━━━━━━━━━━━━━━━
【新增阵营的唯一允许情况】

仅当满足以下【全部条件】时，才允许写入“新增阵营”：

1. 文本中出现了【明确的阵营名称或称呼】
2. 该名称不在当前已知阵营列表中
3. 该阵营在文本中有【明确行动或决策描述】

否则：
新增阵营=（无）

━━━━━━━━━━━━━━━━━━━━
【输出格式（必须严格符合，不得增删）】

NPC状态=存活/死亡/未知

所属阵营=
- 阵营名 | 阵营思想（一句功能性描述） | NPC位置 | 态度

新增阵营=
- 阵营名 | 阵营思想（一句功能性描述）
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        // ====== 轻量阵营回填逻辑（示意） ======
        if (!string.IsNullOrEmpty(result) && teams != null)
        {
            var lines = result.Split('\n');
            bool inNewTeam = false;

            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line.StartsWith("新增阵营"))
                {
                    inNewTeam = true;
                    continue;
                }

                if (inNewTeam && line.StartsWith("-"))
                {
                    var teamName = line.Substring(1).Split('|')[0].Trim();
                    if (!teams.Contains(teamName))
                        teams.Add(teamName);
                }
            }
        }

        return result ?? string.Empty;
    }
    
    public static async Task<GameGenerate.GptSpaceGenerateResult> RebuildCocMapHierarchy(
    string cocText,
    List<SpaceCardConfig> allSpaces)
{
    var spaceDump = new StringBuilder();
    foreach (var spc in allSpaces)
    {
        spaceDump.Append("名字：");
        spaceDump.Append(spc.title);
        spaceDump.Append("｜详细信息：");
        spaceDump.Append(spc.descirption);
        spaceDump.Append("\n");
    }

    var schema = GptSchemaBuilder.BuildSchema(typeof(GameGenerate.GptSpaceGenerateResult));

    var messages = new List<QwenChatMessage>
    {
        new QwenChatMessage
        {
            role = "system",
            content =
@"你是《克苏鲁的呼唤（Call of Cthulhu）》模组的【地图结构重构解析器】。

你的职责是：把一组已有地点，整理成【符合真实地理与物理逻辑、可直接用于 RPG 的地图结构】。
你不是在写故事，而是在构建一张“可以走、可以调查、可以被程序使用”的地图。"
        },

        new QwenChatMessage
        {
            role = "user",
            content =$@"
━━━━━━━━━━━━━━━━━━━━
【已有地点数据（不可删除）】
━━━━━━━━━━━━━━━━━━━━
{spaceDump}

━━━━━━━━━━━━━━━━━━━━
【你的任务】
━━━━━━━━━━━━━━━━━━━━
你需要将这些地点整理为一张【真实、可行走、符合物理世界的 CoC 地图结构】。

这不是故事，不是叙事，而是【空间拓扑结构】。

━━━━━━━━━━━━━━━━━━━━
【地图构建原则】
━━━━━━━━━━━━━━━━━━━━
1️⃣ 所有地点必须处于明确的物理层级中  
2️⃣ 父空间在物理上必须“包含”子空间  
3️⃣ spaces 只表示【可以直接进入的下级空间】  
4️⃣ 禁止抽象关系、时间关系、剧情关系  
5️⃣ 禁止循环结构（A → B → A）

━━━━━━━━━━━━━━━━━━━━
【完整性强制规则（极其重要）】
━━━━━━━━━━━━━━━━━━━━
- ❌ 绝对禁止删除任何已有地点
- ❌ 不允许任何已有地点在最终结果中缺失
- ✅ 允许新增【区域 / 过渡 / 容器型】空间
- ⚠️ 如果一个地点在物理上无法直接挂载：
     必须为其创建新的父级空间
- 每个地点必须是其他节点的子节点或父节点
- 整个地图节点必须是联通的

━━━━━━━━━━━━━━━━━━━━
【关键结果约束（用于防止空结构）】
━━━━━━━━━━━━━━━━━━━━
- 最终输出中：
  - 除了“世界 / 区域 / 地图根节点”外
  - ❗ 每一个 SpaceCreator：
       必须至少出现在某一个父空间的 spaces 中
- 不允许存在“未被引用的孤立地点”

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

    var result = await GameFrameWork.Instance.GptSystem
        .ChatToGPT<GameGenerate.GptSpaceGenerateResult>(messages);

    return result ?? new GameGenerate.GptSpaceGenerateResult
    {
        spaces = new List<SpaceCreator>()
    };
}
}
