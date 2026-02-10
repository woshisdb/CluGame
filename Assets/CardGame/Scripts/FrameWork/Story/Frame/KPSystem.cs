using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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
/// 字典信息
/// </summary>
public class CocDicItem
{
    public string description;
    /// <summary>
    /// 
    /// </summary>
    public string type;

    public List<string> expectbehave;
}

/// <summary>
/// KP 框架：负责从模组文本中生成“玩家未介入时的世界主线”
/// </summary>
public class KPSystem
{
    public static string HappenedStory = "已经发生";
    public static string ExpectStory = "预期事件";
    public static string cocSimpleText = "模组精简";
    public static string database_Type = "数据字典_typed";
    public static string database = "数据字典";
    
    /// <summary>
    /// 当前世界主线（key = npc）
    /// </summary>
    public Dictionary<string, List<WorldStorySegment>> npcStory
        = new Dictionary<string, List<WorldStorySegment>>();

    public WorldMainStory WorldMainStory;

    public VillainCoreRet VillainCoreRet;
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
    public class StoryHappenResult
    {
        public List<string> events;
    }
    public static async Task<List<string>> GptExtractStoryHappen(
        string str
    )
    {

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【世界事件候选提取器】。

你的职责是：
从文本中找出【可能构成世界事件的事实描述】。

你不需要保证去重准确性。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【新的模组文本（节选）】
{str}

【你的任务】
从【当前模组文本】中，提取【可能构成世界事件的事实描述】。

你只负责“候选提取”，不需要考虑去重或排序。

━━━━━━━━━━━━━━━━━━━━
【事件分类铁律（必须严格遵守）】

【最高优先级规则 A（调查员依赖规则）】

❗ 凡是事件是否发生【依赖调查员 / 玩家 / 外来者的行为】，
无论语法、时态、表述方式如何，
一律判定为【条件触发事件】。

包括但不限于：
- “当调查员……”
- “如果有人进入……”
- “调查员到达……后……”
- “他们发现……将会……”
- “若被调查 / 被打开 / 被触碰”

━━━━━━━━━━━━━━━━━━━━
【最高优先级规则 B（时间字段纯净规则）】

❗ 【发生时间】字段中：
【严禁】出现任何与调查员或玩家有关的内容。

以下内容【绝对不能】出现在 [发生时间： ] 中：
- 调查员 / 玩家 / 外来者
- 到达 / 进入 / 调查 / 发现 / 触发
- “当……时”“之后”“一旦……”

❗ 如果一个事件的“时间描述”依赖调查员行为，
即使写在 [发生时间] 中，
也【必须】改判为【条件触发事件】。

━━━━━━━━━━━━━━━━━━━━
【事件类型与格式】

一、已经发生的事件（既成事实）

判定条件（必须全部满足）：
- 事件在文本中被明确描述为【调查员介入前已发生】
- 完全不依赖调查员或玩家行为
- 【发生时间】为客观时间（日期 / 年代 / 时间段 / 社会阶段）
- 即使玩家不存在，事件依然成立

格式：
～[发生时间：客观时间描述] 事件内容

━━━━━━━━━━━━━━━━━━━━
二、条件触发的事件（尚未发生）

判定条件（满足任一即可）：
- 事件是否发生取决于调查员行为
- 时间或条件描述中涉及调查员
- 描述的是潜在后果、触发结果、隐藏真相

格式：
*[条件：触发条件描述] 事件内容

━━━━━━━━━━━━━━━━━━━━
【明确示例（用于判定，不得违反）】

❌ 错误（绝对禁止）：
～[发生时间：在调查员到达镇上时] 听到哭声

✅ 正确：
*[条件：调查员到达镇上] 调查员能听到母亲的哭声

━━━━━━━━━━━━━━━━━━━━
【禁止事项】

- 禁止将条件伪装为时间
- 禁止将调查员相关事件归为已发生
- 禁止推断文本未明确写出的事实
- 禁止改写背景设定为事件
- 禁止书写玩家行为本身
- 禁止评价性语言
- 不确定的事情就不要写，需要写好前因后果
- 只写一些比较重要的内容，其他内容不要写
━━━━━━━━━━━━━━━━━━━━
【输出要求】

- 严格 JSON
- 只输出事件字符串数组
- 若没有任何可构成事件的内容 → 返回 []

返回结构：
List<string>"


            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? new List<string>();
    }

    
    [Button("过滤信息")]
    public async Task FilterGptText()
    {
        Dictionary<string,string> infoDictionary = new Dictionary<string, string>();
        List<string> storyHappen = new();
        var rawText = Load("模组精简");
        var chunks = GptLongTextProcessor.SplitText(rawText,2000);
        int index = 1;
        foreach (var str in chunks)
        {
            Debug.Log(index+"/"+chunks.Count+".....");
            index++;
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
            var events = await GptExtractStoryHappen(str);
            storyHappen = events;
        }

        Save<Dictionary<string, string>>("数据字典", infoDictionary);
        return;
    }
    public static async Task<List<string>> GptCombine(
        List<string> x,
        List<string> y
    )
    {
        if ((x == null || x.Count == 0) && (y == null || y.Count == 0))
            return new List<string>();

        var merged = new List<string>();
        if (x != null) merged.AddRange(x);
        if (y != null) merged.AddRange(y);

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【世界事件归并器】。

你的职责是：
对世界事件进行【去重、合并、排序】，而不是创作新内容。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"以下是两批世界事件，请将它们合并成一个规范列表。

【事件格式说明】
- 已发生事件：
  [发生时间：时间描述] 事件内容
- 条件触发事件：
  [条件：触发条件描述] 事件内容

【合并规则（必须遵守）】
1. 语义重复的事件只保留一条（保留信息更完整者）
2. 不新增原文中不存在的事件
3. 不改写事实含义
4. 已发生事件排在前，条件事件排在后
5. 同类事件按自然时间 / 条件顺序排列
6. 若无法判断先后，保持原有顺序

【待合并事件】
{string.Join("\n", merged.Select(e => "- " + e))}

【输出要求（非常重要）】
1. 只能输出【合法 JSON】
2. JSON 根结构必须是【数组】
3. 数组的每一个元素必须是【字符串】
4. 不允许输出 a,b,c 这种非 JSON 内容
5. 不允许输出解释、注释或多余文字

【正确示例】
[
  ""[发生时间：1926年春] 黑水溪开始出现异常水质"",
  ""[条件：深入矿区调查] 发现地下洞穴系统""
]

请严格按照示例格式输出。"

            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? merged;
    }
    
    public static List<List<string>> Chunk(List<string> source, int size)
    {
        var result = new List<List<string>>();
        for (int i = 0; i < source.Count; i += size)
        {
            result.Add(source.Skip(i).Take(size).ToList());
        }
        return result;
    }
    
    public static async Task<List<string>> CombineAll(
        List<string> storyHappen,
        int size
    )
    {
        // 第一步：分块
        var blocks = Chunk(storyHappen, size);

        // 第二步：像归并排序一样两两合并
        while (blocks.Count > 1)
        {
            var next = new List<List<string>>();

            for (int i = 0; i < blocks.Count; i += 2)
            {
                if (i + 1 < blocks.Count)
                {
                    var combined = await GptCombine(blocks[i], blocks[i + 1]);
                    next.Add(combined);
                }
                else
                {
                    next.Add(blocks[i]);
                }
            }

            blocks = next;
        }

        return blocks.Count > 0 ? blocks[0] : new List<string>();
    }
    public class StoryHappenSplitResult
    {
        public List<string> Happened = new();
        public List<string> Conditional = new();
        public List<string> Unknown = new(); // 防止数据污染
    }

    public static StoryHappenSplitResult SplitStoryHappen(List<string> storyHappen)
    {
        var result = new StoryHappenSplitResult();

        if (storyHappen == null)
            return result;

        foreach (var line in storyHappen)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var firstChar = line[0];

            switch (firstChar)
            {
                case '～':   // 已发生
                case '~':    // 兼容英文波浪号
                    result.Happened.Add(line);
                    break;

                case '*':    // 条件 / 未来
                    result.Conditional.Add(line);
                    break;

                default:
                    result.Unknown.Add(line);
                    break;
            }
        }

        return result;
    }

    [Button("生成故事信息")]
    public async Task StoryFilterGptText()
    {
        List<string> storyHappen = new();
        var rawText = Load("模组精简");
        var chunks = GptLongTextProcessor.SplitText(rawText,4000);
        int index = 1;
        foreach (var str in chunks)
        {
            Debug.Log(index+"/"+chunks.Count+".....");
            index++;
            var events = await GptExtractStoryHappen(str);
            
            if (events.Count > 0)
            {
                foreach (var e in events)
                {
                    if (!storyHappen.Contains(e))
                    {
                        storyHappen.Add(e);
                    }
                }
            }
        }
        var ret = SplitStoryHappen(storyHappen);
        int size = 10; // 你可以调 5 / 10 / 20
        var conds = await CombineAll(ret.Conditional, size);
        var happened = await CombineAll(ret.Happened, size);
        Save("预期事件", conds);
        Save("已经发生", happened);
        return;
    }

    [Button("生成带类型字典")]
    public static async Task BuideHasTypeDic()
    {
        var infoDictionary = Load<Dictionary<string, string>>("数据字典");
        Dictionary<string, CocDicItem> typedDict =
            new Dictionary<string, CocDicItem>();

        int typeIndex = 1;
        foreach (var kv in infoDictionary)
        {
            Debug.Log($"TypeCheck {typeIndex}/{infoDictionary.Count} : {kv.Key}");
            typeIndex++;

            var item = await GptCheckCocDicItemType(kv.Key, kv.Value);
            if (item != null)
            {
                typedDict[kv.Key] = item;
            }
        }
        Save<Dictionary<string, CocDicItem>>("数据字典_typed", typedDict);
        return;
    }
    public class CocTypeCheckResult
    {
        public string Type;        // monster / character / rule / item / knowledge
    }
    public static async Task<CocDicItem> GptCheckCocDicItemType(
        string key,
        string description
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(CocTypeCheckResult));

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【对象类型判定器】。
你只做分类判断，不做创作、不补全、不推理隐藏信息。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【对象名称】
{key}

【对象描述】
{description}

【可选类型（只能选一个）】
- monster（怪物、非人威胁、异界存在）
- character（角色、NPC、人类）
- rule（规则、异常机制、非现实法则）
- item（物品、道具）
- knowledge（知识、传闻、世界设定）
- place (地点，区域)
【规则】
- 只能从上述类型中选择
- 如果是人物，一律为 character
- 不要解释原因

【输出要求】
- 严格使用 JSON
- 严格符合 Schema
- 不要输出任何额外文本

Schema：
{schema}"
            }
        };

        var result =
            await GameFrameWork.Instance.GptSystem
                .ChatToGPT<CocTypeCheckResult>(messages);

        if (result == null || string.IsNullOrEmpty(result.Type))
            return null;

        return new CocDicItem
        {
            description = description,
            type = result.Type
        };
    }

    
    [Button("获取信息")]
    public void GetAllGptDict()
    {
        var data = Load<Dictionary<string, string>>("数据字典");
        Debug.Log(1);
    }
    public class FilterReturn
    {
        public Dictionary<string,string> mapInfo;
    }
    public static async Task<FilterReturn> GptFilterInfo(
        Dictionary<string, string> infoDictionary,
        string str
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(FilterReturn));

        // 只给 GPT key，不给 value
        var knownKeysText = infoDictionary.Count == 0
            ? "（当前尚无已知对象）"
            : string.Join("、", infoDictionary.Keys);

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content = $@"你是一个信息分类提取器，正在对一个【分段 CoC 模组文本】进行解析。
你的任务是：
从输入文本中提取【被明确描述的对象】及其对应描述，
并【优先合并到已有对象】中。

【已存在的对象名称列表】
{knownKeysText}

⚠️ 极其重要的合并规则（必须遵守）：

在创建新 key 之前，你必须执行以下判断流程：

【步骤 1：语义对齐】
- 判断文本中出现的名称，是否是以下情况之一：
  - 已有对象的简称
  - 别称 / 绰号
  - 指代性称呼（如：母神、她、那个存在）
  - 翻译差异或表述差异
- 如果是，则【必须使用已有 key】，禁止新建

【步骤 2：确认新对象】
- 只有在以下条件同时满足时，才允许创建新 key：
  1. 该对象在已知列表中找不到语义对应
  2. 文本中对其有独立、明确、持续的描述
  3. 它不是已有对象的一部分、阶段、化身或结果

对象范围包括但不限于：
- 人物
- 怪物 / 外神 / 异常存在
- 物品
- 地点
- 仪式、组织、事件
- 非现实规则

输出规则：
1. key 必须是【最完整、最正式、最稳定】的对象名称  
   （优先使用已存在的 key）
2. value 是该段文本中关于该对象的【原始描述整理】
3. 不总结、不概括、不改写含义
4. 不生成原文中不存在的对象
5. 如果本段文本没有可提取对象，返回空 mapInfo
6. 只输出 JSON，不输出任何解释

返回格式：
{schema}"
            },
            new QwenChatMessage
            {
                role = "user",
                content = str
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<FilterReturn>(messages);
    }


    public static async Task<string> GptCombineInfo(
        string obj,
        string befStr,
        string newStr
    )
    {
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个文本合并器。

你的任务是：
将同一个对象的【两段描述】合并为一段完整描述。

规则：
1. 对象名称：" + obj + @"
2. 不删除任何有效信息
3. 不重复相同内容
4. 保留原有叙事顺序
5. 不总结、不改写含义
6. 只整理文本结构
7. 不添加标题
8. 不输出解释性内容
9. 只输出合并后的正文文本"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【已有描述】
{befStr}

【新增描述】
{newStr}"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);
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
    public static void Save<T>(string fileName,T data)
    {
        var filePath = "Assets/Resources/Coc模组/" +fileName+ ".dat";
        Debug.Log(filePath);
        var json = SerializationUtility.SerializeValue<T>(data, DataFormat.JSON);
        File.WriteAllBytes(filePath, json);
        Debug.Log("Data Saved: " + json);
    }
    public static T Load<T>(string fileName)
    {
        var filePath = "Assets/Resources/Coc模组/" +fileName+ ".dat";
        var json = File.ReadAllBytes(filePath);
        return SerializationUtility.DeserializeValue<T>(json, DataFormat.JSON);
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
    public static List<string> SplitText(string text,int size = -1)
    {
        if (size == -1)
        {
            size = MAX_CHUNK_LENGTH;
        }
        var result = new List<string>();

        if (string.IsNullOrEmpty(text))
            return result;

        int index = 0;
        int length = text.Length;

        while (index < length)
        {
            int remaining = length - index;
            int take = Math.Min(size, remaining);

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
