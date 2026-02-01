using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UglyToad.PdfPig;
using UnityEngine;

/// <summary>
/// 世界主线中的一个事件段（留有时间余地）
/// </summary>
public class WorldStorySegment
{
    /// <summary>
    /// 发生的时间段描述（如：第1-2天 夜晚）
    /// 必须是“区间”，不是点
    /// </summary>
    public string TimeWindow;
    /// <summary>
    /// 当前npc
    /// </summary>
    public string nowNpc;
    /// <summary>
    /// 涉及的除当前 NPC的其他npc
    /// </summary>
    public List<string> otherNpcs;

    /// <summary>
    /// 在该时间段内 NPC 的核心行为
    /// </summary>
    public string Behavior;

    /// <summary>
    /// 如果无人干预，该事件的结果
    /// </summary>
    public string Outcome;
}

/// <summary>
/// KP 生成的完整主线故事
/// </summary>
public class WorldMainStory
{
    public Dictionary<string,List<WorldStorySegment>> npcStory;
}
/// <summary>
/// 反派计划的一步
/// </summary>
public class VillainStep
{
    /// <summary>
    /// 当前一步要做什么
    /// </summary>
    public string step;
    /// <summary>
    /// 满足的条件（可以是时间也可以是满足什么特殊条件）
    /// </summary>
    public string condition;
}

/// <summary>
/// 反派模块，用来创建反派的故事
/// </summary>
public class VillainCore
{
    /// <summary>
    /// 反派的计划，每一步要做什么
    /// </summary>
    public List<VillainStep> steps;
    /// <summary>
    /// 当前反派是谁
    /// </summary>
    public string npcName;
    /// <summary>
    /// 他要做的事情
    /// </summary>
    public string aim;
    /// <summary>
    /// 他要做这些的理由
    /// </summary>
    public string why;
}

public class VillainCoreRet
{
    public List<VillainCore> Villains;
}

/// <summary>
/// KP 框架：负责从模组文本中生成“玩家未介入时的世界主线”
/// </summary>
public class KPSystem
{
    /// <summary>
    /// 当前世界主线（key = npc）
    /// </summary>
    public Dictionary<string, List<WorldStorySegment>> npcStory
        = new Dictionary<string, List<WorldStorySegment>>();

    public WorldMainStory WorldMainStory;

    public VillainCoreRet VillainCoreRet;
    // /// <summary>
    // /// 从 CoC 模组文本中生成 NPC 的默认行为时间序列
    // /// </summary>
    // public async Task<Dictionary<string, List<WorldStorySegment>>> GenerateStory(
    //     string cocText,
    //     List<string> npcs
    // )
    // {
    //     var result = await AskGptForWorldStory(cocText, npcs);
    //     npcStory = result.npcStory;
    //     return npcStory;
    // }
    [Button]
    public async Task CreateStory(int time)
    {
        var cocText = Load("模组精简");
        var npcs = new List<string>();
        var npcsCfg = GameFrameWork.Instance.data.saveFile.ConfigSaveData.mainNpcCfg;
        foreach (var x in npcsCfg)
        {
            npcs.Add(x.name);
        }
        var ret = await GptLongTextProcessor.Retry<VillainCoreRet>(async () =>
        {
            return await AskGptForVillains(cocText, npcs,time);
        });
        this.VillainCoreRet = ret;
        Debug.Log(111);
    }
    // =====================================================
    // GPT：真正的“守密人判断”
    // =====================================================

    private async Task<WorldMainStory> AskGptForWorldStory(string cocText,List<string> npcs)
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(WorldMainStory));

        var npcListText = string.Join("、", npcs);

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁神话跑团的守密人（KP）。
你的任务不是讲故事，而是解析模组文本中的【默认世界运作逻辑】。

规则：
- 玩家不存在
- 不要引入模组之外的新角色
- 不要扩写剧情细节
- 只描述“NPC 在什么时间段内做什么”
- 所有时间必须是【区间】，给玩家介入留下余地
- 行为必须符合模组原文设定
- 每个 NPC 至少一个行为段
- NPC 的生活地点、工作地点必须被隐含满足"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【已知 NPC 列表】
{npcListText}

请你解析该模组，在【玩家未介入】的前提下，
为每个 NPC 生成一个【当前世界主线行为段】。
npcStory的结构为
public Dictionary<string,List<WorldStorySegment>> npcStory;

WorldStorySegment行为段必须包含：
- TimeWindow（时间区间，如：第1-2天 夜晚）
- nowNpc
- otherNpcs（如涉及）
- Behavior（该时间段内的核心行为）
- Outcome（如果无人干预将导致的结果）

请严格使用 JSON 格式返回：
{schema}"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<WorldMainStory>(messages);
    }
    
    
    
    public async Task<VillainCoreRet> AskGptForVillains(string cocText,List<string> npcs,int allTime)
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(VillainCoreRet));
        var npcListText = string.Join("、", npcs);

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁神话跑团的【守密人（KP）】。

你的职责不是写故事，而是：
【解析：从模组开始时间起，如果玩家完全不介入，世界将如何按时间顺序走向不可逆的灾难】。

时间与结构是你的第一优先级。

【时间基准】
- 模组文本描述的是【模组开始时已经成立的世界状态】
- 你生成的所有 steps，必须【全部发生在模组开始之后】

【整体要求】
- 反派的行为必须形成一条【逐步升级、不可逆的因果链】
- 时间越往后，世界局势必须越严峻
- 允许前期隐蔽，但后期必须公开失控

禁止：
- 新增模组中不存在的人物
- 描写或假设玩家存在
- 使用跑团术语
- 使用抽象行为描述（调查、准备、推进、影响等）
"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文（模组开始时的世界状态）】
{cocText}

【已知 NPC 列表】
{npcListText}

【时间约束】
- 从【模组开始时间】算起
- 如果无人干预，反派将在【约 {allTime} 天内】完成其最终目标
- 最终目标完成后，世界进入【不可逆的灾难状态】

请识别模组中的【反派核心】（可以是一个或多个）。

每个反派必须包含：

- npcName  
  执行该计划的 NPC（必须来自 NPC 列表）

- aim  
  在【不超过 {allTime} 天】时，想要完成的最终结果  
  ⚠️ aim 描述的是【已发生的世界状态】，而不是“想做什么”  
  必须满足至少一项：
  - 大规模死亡
  - 群体异变或感染
  - 长期或永久性现实/精神污染
  - 社会或自然秩序的不可逆崩坏

- why  
  反派在模组开始后持续推进该计划的根本动机

- steps  
  从【模组开始之后】到【aim 完成】的行为序列

【steps 强制结构规则（非常重要）】：

1. steps 必须按【时间顺序】排列  
2. steps 的【严重程度必须逐步升级】
   - 前期：局部、隐蔽、可被忽视
   - 中期：异常明显，造成多人受害
   - 后期：公开失控，社会或区域性危机
3. steps 中必须至少有一个步骤：
   - 明确标志【世界进入危机阶段】
   - 普通人已经无法忽视或控制
4. 最后一个 step 必须满足：
   - aim 所描述的结果已经完成
   - 灾难已经发生，而不是“即将发生”

【step 描述规则】：
- 每个 step 必须是【明确、具体、可观察的行为】
  例如：
  - 在某地杀死某人
  - 向水源投放某种物质
  - 在某地点完成一次献祭
  - 释放、解除或唤醒某个存在
- 禁止抽象或模糊动词
- 每个 step 都必须改变世界状态，并加重局势
- 每个 step 的行为时间都不超过{allTime}天
- condition  
  描述该 step 发生的时间条件  
  必须以【模组开始时间】为基准，例如：
  - 模组开始后的第 1-2 天夜晚
  - 模组开始后第 6 天清晨
  - 前一步完成后的 24 小时内

请严格使用 JSON 返回，格式如下：
{schema}"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<VillainCoreRet>(messages);
    }
    /// <summary>
    /// 生成故事的历史内容（时间序列）：
    /// 即在玩家介入之前，世界已经发生过的事情
    /// </summary>
    [Button("生成故事的历史背景")]
    public async Task<string> AskGptForHistory()
    {
        var cocText = Load("模组精简");
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁神话跑团的【守密人（KP）】。

你的任务不是创作剧情，而是：
【把模组中玩家介入之前已经发生过的事件，整理为严格的时间序列历史】

这些内容将被视为：
- 世界的既成事实
- NPC 已经经历过的过去
- 模组开始时的初始世界状态

严格规则：
- 只描述【已经发生过的事情】
- 使用过去时
- 不要暗示或预测未来
- 不要提出悬念、任务或线索
- 不要描述玩家或假设玩家存在
- 不要使用跑团术语
- 不要进行氛围或心理描写
"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

请将模组中【玩家介入之前】已经发生的历史事件，
整理为【按时间顺序排列的时间轴】。

输出要求：

1. 必须按照【从最早到最近】的顺序排列
2. 每一条历史事件都必须包含：
   - 时间段（相对时间即可，例如：数年前、模组开始前第 10 天）
   - 发生了什么事情
3. 最后一条事件必须是：
   - 【紧接模组开始之前】发生的状态或事件
4. 只允许描述已经发生的事实
5. 不要描述未来、不确定性或计划中的行为

输出格式示例（仅示例，不要照抄）：

【数年前】
某个家族开始控制土地并形成封闭的社会结构。

【模组开始前第 30 天】
某些异常现象首次被少数居民注意到。

【模组开始前第 1 天】
城镇处于表面平静但暗藏异常的状态。

请直接输出整理后的【时间序列历史文本】。
不要使用 JSON，不要使用编号列表。
"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<string>(messages);
    }
    [Button("异常字典生成")]
    public async Task<CocDic> AskGptForCoc()
    {
        var cocText = Load("模组精简");
        var schema = GptSchemaBuilder.BuildSchema(typeof(CocDic));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁神话跑团模组的【异常规则解析器】。

你的职责是：
【从模组文本中提取所有“非现实、超自然、异常”的存在或规则】
而不是创作故事或推进剧情。

你只关心：
- 现实世界中不存在，但在模组中成立的事物
- 它们的客观规则与影响

禁止行为：
- 不要编造模组中不存在的内容
- 不要描写玩家
- 不要推进剧情
- 不要写氛围、情绪或文学化描述
- 不要使用跑团术语"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

请你从模组文本中提取所有【非现实内容 / 异常规则】。

每一条必须满足：
- 在现实世界中不存在
- 在模组世界中是客观成立的
- 会对人物、环境或世界状态产生影响

可以包括：
- 异常实体或存在
- 超自然疾病、污染、诅咒
- 仪式、禁忌、规则性现象
- 模组特有的神话机制

对每一个异常，请提供：

- name  
  异常的名称（若模组未明确命名，请使用模组中最常用的指代）

- description  
  对该异常【客观规则与影响】的简要说明  
  不要描述剧情过程  
  不要写感受  
  不要写玩家行为  

注意：
- 不要拆分为事件或时间线
- 不要解释来源背景，除非模组文本明确说明其规则的一部分
- 如果某异常只是规则的一部分，不要单独重复生成
-【异常归并规则（必须遵守）】

- 如果多个异常现象由同一个异常源头直接导致：
  - 只能生成【一个】异常条目
  - 其 description 中应包含：
    - 传播方式
    - 转化或变化机制
    - 最终可能导致的不可逆后果


请严格使用 JSON 返回：
{schema}"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<CocDic>(messages);
    }
    [Button]
    public string ReadPdfText()
    {
        var path = "Assets/Resources/Coc模组/模组.pdf";
        var sb = new StringBuilder();
        using var doc = PdfDocument.Open(path);

        foreach (var page in doc.GetPages())
        {
            sb.AppendLine(page.Text);
        }
       var ret = sb.ToString();
       Save("模组",ret);
       return ret;
    }
    /// <summary>
    /// 输入 CoC 模组 PDF 解析后的文本，让 GPT 进行规范整理（支持超长文本，不丢细节）
    /// </summary>
    [Button]
    public async Task AskGptText()
    {
        var GptLongTextProcessor = new GptLongTextProcessor();
        var rawText = Load("模组");
        var str = await GptLongTextProcessor.ProcessLongText(rawText);
        Save("模组精简",str);
        Debug.Log("PDF 文本规范化整理完成（分段模式）");
    }
    [Button]
    public async Task FilterGptText()
    {
        Dictionary<string,string> infoDictionary = new Dictionary<string, string>();
        var rawText = Load("模组精简");
        var chunks = GptLongTextProcessor.SplitText(rawText);
        foreach (var str in chunks)
        {
            var data = await GptFilterInfo(infoDictionary,str);
            foreach (var kv in data.mapInfo)
            {
                if (infoDictionary.ContainsKey(kv.Key))
                {
                    infoDictionary[kv.Key]=await GptCombineInfo(kv.Key,infoDictionary[kv.Key],kv.Value);
                }
                else
                {
                    infoDictionary[kv.Key] = kv.Value;
                }
            }
        }
        return;
    }
    public class FilterReturn
    {
        public Dictionary<string,string> mapInfo;
    }
    public async Task<FilterReturn> GptFilterInfo(Dictionary<string,string> infoDictionary,string str)
    {
    }

    public async Task<string> GptCombineInfo(string obj,string befStr,string newstr)
    {
    }
    /// <summary>
    /// 保存字符串到本地（Unity 安全路径）
    /// </summary>
    public static void Save(string fileText,string data)
    {
        var filePath = "Assets/Resources/Coc模组/" + fileText+".txt";
        Debug.Log(filePath);
        File.WriteAllText(filePath, data);
    }

    public static string Load(string fileText)
    {
        var filePath = "Assets/Resources/Coc模组/"+fileText + ".txt";
        Debug.Log(filePath);
        return File.ReadAllText(filePath);
    }
}

public class GptLongTextProcessor
{
    /// <summary>
    /// 单段最大长度（字符数）
    /// 3000~3500 是稳定区间
    /// </summary>
    private const int MAX_CHUNK_LENGTH = 3000;

    /// <summary>
    /// GPT 用的 system prompt（非常关键，不要随便改）
    /// </summary>
    private const string SYSTEM_PROMPT =
        @"你正在对一个超长 CoC 模组 PDF 解析文本进行【分段规范整理】。
这是其中的一部分文本。

你的任务是：清理与故事内容无关的信息，并在不改变含义的前提下，使正文更连贯、更易读取。

整理规则：
1. 可以适度压缩冗余表达，但不得删除任何与剧情、设定、事件、人物、地点、异常、规则相关的细节
2. 不总结、不概括、不重写文本含义
3. 不新增原文中不存在的内容
4. 只允许进行以下修改：
   - 修复 PDF 断行、分页导致的句子破碎
   - 删除重复的页眉、页脚、页码、装饰线
   - 修正明显的 OCR 错字（不改变原意）
   - 删除多余空行、异常符号
   - 重新整理段落，使文本更易阅读
5. 必须删除以下【与故事无关】的信息：
   - 作者信息、版权声明、出版信息
   - 目录、索引、页码说明
   - 使用说明、给主持人的建议（除非其内容属于世界观或剧情）
   - 排版说明、插图说明、装饰性文本
6. 保留原有叙事顺序与逻辑结构
7. 不添加标题或编号
8. 不使用 Markdown 或任何格式标记
9. 只输出整理后的正文文本
10. 输入没有信息，输出也不要有信息
11. 不要输出任何解释、说明或附加内容";


    /// <summary>
    /// 对外主入口：处理完整长文本
    /// </summary>
    public async Task<string> ProcessLongText(string rawText)
    {
        if (string.IsNullOrEmpty(rawText))
            return string.Empty;

        var chunks = SplitText(rawText);
        var finalText = new StringBuilder();

        Debug.Log($"GPT 长文本分段数：{chunks.Count}");

        for (int i = 0; i < chunks.Count; i++)
        {
            Debug.Log($"处理第 {i + 1}/{chunks.Count} 段");

            string cleaned = await Retry(async () =>
            {
                return await NormalizeChunk(chunks[i]);
            });

            finalText.AppendLine(cleaned);
            finalText.AppendLine();
        }

        return finalText.ToString();
    }
    
    /// <summary>
    /// 将长文本按段落切分
    /// </summary>
    public static List<string> SplitText(string text)
    {
        var result = new List<string>();

        if (string.IsNullOrEmpty(text))
            return result;

        int index = 0;
        int length = text.Length;

        while (index < length)
        {
            int remaining = length - index;
            int take = Math.Min(MAX_CHUNK_LENGTH, remaining);

            // 先假定硬切
            int cut = index + take;

            // 尝试往前找一个“安全切点”
            int safeCut = FindSafeCut(text, index, cut);

            if (safeCut <= index)
            {
                // 实在找不到，只能硬切
                safeCut = cut;
            }

            result.Add(text.Substring(index, safeCut - index));
            index = safeCut;
        }

        return result;
    }
    
    public static int FindSafeCut(string text, int start, int end)
    {
        // 从后往前找
        for (int i = end - 1; i > start; i--)
        {
            char c = text[i];

            // 优先级从高到低
            if (c == '\n')
                return i + 1;

            if (c == '。' || c == '！' || c == '？')
                return i + 1;
        }

        return -1;
    }


    /// <summary>
    /// 调 GPT 处理单个文本分段
    /// </summary>
    private async Task<string> NormalizeChunk(string chunk)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content = SYSTEM_PROMPT
            },
            new QwenChatMessage
            {
                role = "user",
                content = chunk
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);
    }

    /// <summary>
    /// 简单可靠的重试机制（防 Cancel / 超时）
    /// </summary>
    public static async Task<T> Retry<T>(Func<Task<T>> action, int retryCount = 3)
    {
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                return await action();
            }
            catch (TaskCanceledException)
            {
                Debug.LogWarning($"GPT 请求被取消，重试 {i + 1}/{retryCount}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"GPT 异常：{e.Message}，重试 {i + 1}/{retryCount}");
            }

            await Task.Delay(500);
        }

        Debug.LogError("GPT 请求多次失败，跳过该段");
        return default(T);
    }
}

/// <summary>
/// 这是每个异常对象或规则的描述
/// </summary>
public class CocItem
{
    /// <summary>
    /// 异常的名字
    /// </summary>
    public string name;
    /// <summary>
    /// 异常的描述
    /// </summary>
    public string description;
}

/// <summary>
/// 这里保存所有的非现实内容，即模组自创的规则。
/// </summary>
public class CocDic
{
    private List<CocItem> cocItems;
}
