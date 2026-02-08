using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
