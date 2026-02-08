using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 讲述故事的类
/// </summary>
public class KPStory
{
    [Button("输出已经发生的故事")]
    public static async Task GenerateAllStory()
    {
        var conds = KPSystem.Load<List<string>>("预期事件");
        var happened = KPSystem.Load<List<string>>("已经发生");
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【KP 时间轴叙述者】。

你的职责是：
将“已经发生的世界事件”整理成一段按时间顺序的背景叙述文本。

你不创造内容，只做整理与讲述。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【已确认的世界事件（全部为既成事实）】
{string.Join("\n", happened.Select(e => "- " + e))}

【你的任务】

1. 仅基于上述事件内容
2. 按【发生时间】从早到晚排序
3. 将它们整理为一段【连贯的世界背景叙述文本】

【叙述规则（必须遵守）】

- 不新增任何事件
- 不补充原文中不存在的因果
- 不出现调查员、玩家或“你们”
- 使用第三人称、过去时
- 语气克制、偏记录性，而非小说化
- 可以自然衔接句子，但不得改写事实

【输出要求】

- 输出为【纯文本】
- 可分为数段
- 不要列表，不要 JSON，不要解释
"
            }
        };
        var history = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);
        KPSystem.Save<string>("历史故事描述",history);
        // var storyLines = await GptGetInvestigatorStoryLines(conds, history);
        // KPSystem.Save<List<string>>("故事线描述",storyLines);
        var conflicts = await GptExtractWorldProcesses(history,conds,3);
        // Debug.Log(1111);
        // // var strs = await GptGetMainStory(conflicts, history, conds, 3);
        // Debug.Log(11);
        // var mainconf = await GptGetMainConflict(conflicts, history, conds, 3);
        // Debug.Log(1);
        var rawText = KPSystem.Load("模组精简");
        // var str = await GptExtractStoryStartTask(rawText);
        // Debug.Log(0);
        var typedDict = KPSystem.Load<Dictionary<string, CocDicItem>>("数据字典_typed");
        var teamDic = new Dictionary<string, string>();
        foreach (var dicItem in typedDict)
        {
            var value = dicItem.Value;
            if (value.type == "character")//如果是角色的话
            {
                var moreDetail = await KPNPCDetail.GPTGetNpcMoreDescription(rawText,dicItem.Key,value.description,conflicts);
                Debug.Log(moreDetail);
                teamDic[dicItem.Key] = moreDetail;
                var behave= GetNpcMainStory(dicItem.Key,value.description,3,moreDetail);
                value.description += "[--核心思想]" +behave;
            }
        }
        KPSystem.Save<Dictionary<string, CocDicItem>>("数据字典_typed",typedDict);
        // KPSystem.Save<Dictionary<string, string>>("角色身份字典",teamDic);
        var ret =  await GptGenerateWhatPlaces(typedDict);
        KPSystem.Save<GameGenerate.GptSpaceGenerateResult>("地点映射信息",ret);
    }

    public static async Task<GameGenerate.GptSpaceGenerateResult> GptGenerateWhatPlaces(
        Dictionary<string, CocDicItem> dics
    )
    {
        // 1. 提取所有 place
        var placeList = dics
            .Where(kv => kv.Value.type == "place")
            .Select(kv =>
                $@"- 地点名：{kv.Key}
  模组描述：{kv.Value.description}")
            .ToList();

        if (placeList.Count == 0)
            return new GameGenerate.GptSpaceGenerateResult { spaces = new List<SpaceCreator>() };

        var placeText = string.Join("\n", placeList);

        // 2. 生成 schema
        var schema = GptSchemaBuilder.BuildSchema(typeof(GameGenerate.GptSpaceGenerateResult));

        // 3. 构造 GPT 消息
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一名克苏鲁跑团模组中的【地图与空间结构设计解析器】。
你只负责解析与重组模组中已存在的地点，不新增世界观外的地点。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组中已存在的地点】
{placeText}

你的任务是：

1. 基于以上地点，生成一个【空间图结构】
2. 空间结构必须为【两层】：
   - 第一层：主要地点（如 公寓、医院、教堂）
   - 第二层：从属空间（如 卧室、洗手间、地下室）
3. 从属空间只能连接其父级空间
4. 第一层空间之间可以互相连接（表示可直接到达）
5. 不要凭空新增模组未暗示的重要地点
6. 地点名称要清晰、唯一、可读
7. detail 字段用于描述该空间的氛围或用途
8. spaces 字段表示【可以直接到达的空间名称列表】

返回结构要求：
- 所有空间（包括子空间）都必须是一个 SpaceCreator
- 连接关系必须是双向合理的
- 不推进剧情，不加入事件

输出要求（非常重要）：
- 只输出 JSON 对象本身
- 不要使用 ```json 或任何 Markdown 包裹
- 不要输出多余文字
示例如下：
{{
  ""spaces"": [
    {{
      ""name"": ""X公寓"",
      ""detail"": ""一栋老旧的公寓楼，空气中弥漫着潮湿与霉味"",
      ""spaces"": [""X公寓-卧室"", ""X公寓-洗手间"", ""Y公寓""]
    }},
    {{
      ""name"": ""X公寓-卧室"",
      ""detail"": ""凌乱的卧室，床单上有奇怪的污渍"",
      ""spaces"": [""X公寓""]
    }},
    {{
      ""name"": ""X公寓-洗手间"",
      ""detail"": ""狭小的洗手间，水管发出不安的声响"",
      ""spaces"": [""X公寓""]
    }},
    {{
      ""name"": ""Y公寓"",
      ""detail"": ""隔壁公寓，住户似乎很少露面"",
      ""spaces"": [""X公寓""]
    }}
  ]
}}
"
            }
        };

        // 4. 请求 GPT
        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GameGenerate.GptSpaceGenerateResult>(messages);

        return result ?? new GameGenerate.GptSpaceGenerateResult { spaces = new List<SpaceCreator>() };
    }
    /// <summary>
    /// npc想要做什么
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetNpcMainStory(
        string name,
        string description,
        int allDays,
        string moreDetail
    )
    {
        var npcDesc =
            $@"{name} 信息：
{description}

【补充细节】
{moreDetail}";

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组中的【NPC 行为决策模型生成器】。

你生成的是：
- NPC 的思考方式
- NPC 的行为取向
- NPC 在不同局势下的决策倾向

你不是在写文学描写，
也不是在写表演细节，
而是在为一个可模拟角色定义【稳定、可推理的决策模型】。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【NPC 名称】
{name}

━━━━━━━━━━━━━━━━━━━━
【NPC 已知描述（事实来源）】
{npcDesc}

━━━━━━━━━━━━━━━━━━━━
【生成目标（必须严格遵守）】

你要基于以上描述，生成一个【可用于后续行为模拟的 NPC 决策模型】。

模型必须：
- 明确该角色“现在想要什么”
- 明确他如何理解自己、他人和当前处境
- 明确他在做选择时的优先顺序

━━━━━━━━━━━━━━━━━━━━
【第一部分：角色行为的核心逻辑（必须首先输出）】

这一部分必须是【通俗、理性、可推理的描述】，
而不是文学描写或抽象口号。

请在一句话中，综合说明该 NPC：

- 当前的性格特征（如谨慎 / 执拗 / 依赖他人 / 回避冲突等）
- 他与周围人的基本关系态度（信任 / 利用 / 回避 / 依附）
- 他最近经历的事情对其判断的影响
- 他现在最想维持或达成的状态
- 他做决策时的基本方式（回避风险 / 强行推进 / 被动应对等）

⚠️ 必须基于描述中能推断出的信息  
⚠️ 不得使用神秘学、象征、隐喻或文学修辞  
⚠️ 目标是“让系统能用它推导行为”  

格式：

-角色行为的核心逻辑：  
一句完整、冷静、可理解的陈述

━━━━━━━━━━━━━━━━━━━━
【第二部分：行为规则列表】

基于上述核心逻辑，列出若干条【宽泛的行为倾向规则】。

每条规则必须：

- 描述该 NPC 在一类情境下“通常会怎么做”
- 不涉及具体动作或台词
- 与其当前状态和处境一致

格式固定为：

- 行为：  
描述一类长期或反复出现的行为倾向

- 触发条件：  
描述 KP 可以直接判断的情境变化（关系、风险、目标受阻等）

━━━━━━━━━━━━━━━━━━━━
【强制约束】

✅ 行为与条件必须：
- 足够宽泛，能覆盖多种具体表现
- 与角色当前状态高度一致
- 能被用于后续推理与状态机判断

🚫 严格禁止：
- 文学描写、情绪表演
- 身体细节、条件反射
- 神秘学或世界观解释
- 数值、时间、距离、次数
- 段子化、口语化表达
- JSON 或任何多余文本
"
            }
        };

        var gptResult = await GameFrameWork.Instance.GptSystem.ChatToGPT(messages);
        return gptResult ?? string.Empty;
    }



    
    [Button("生成角色故事")]
    public async Task<List<string>> GptGetNpcBehaveSequence(
        string npcname,
        string description,
        string history,
        int days
    )
    {
        if (days <= 0)
            return new List<string>();

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    $@"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组中的【KP 世界推进记录器】。

你的职责是：
在【没有任何调查员或外来者介入】的情况下，
以【KP 内部记录】的方式，
推演一个 NPC 在未来 {days} 天中的【行动安排与状态推进】。

你不是在写氛围文本，
你是在生成【可用于世界状态推进的行为日志】。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【NPC 名称】
{npcname}

━━━━━━━━━━━━━━━━━━━━
【NPC 描述（性格 / 立场 / 能力）】
{description}

━━━━━━━━━━━━━━━━━━━━
【既成历史（不可改变）】
{history}

━━━━━━━━━━━━━━━━━━━━
【时间跨度】
{days} 天

━━━━━━━━━━━━━━━━━━━━
【你的任务（核心）】

在【没有调查员介入】的情况下，
描述该 NPC 在接下来 {days} 天内：

- 每天【主要做了哪些事情】
- 是否在【推进某个既有计划或目标】
- 是否出现【状态变化，但只记录结果，不描写情绪】

━━━━━━━━━━━━━━━━━━━━
【重要行为优先级规则】

你只需要记录：
- 行动
- 决策
- 明确的计划推进
- 对世界状态有影响的变化

❌ 不要描写：
- 情绪化动作
- 象征性行为
- 文学化意象
- 氛围描写
- 内心独白

━━━━━━━━━━━━━━━━━━━━
【输出格式（严格）】

- 返回一个字符串数组
- 数组长度必须等于 {days}
- 每一项表示【该 NPC 当天的世界行为记录】
- 按当天时间顺序描述（早 → 晚）
- 使用第三人称、过去时

- 不要解释
- 不要输出 JSON 以外的内容

返回结构：
List<string>"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? new List<string>();
    }
    
    public async Task<List<string>> GptGetInvestigatorStoryLines(
        List<string> conds,
        string history
    )
    {
        if ((conds == null || conds.Count == 0) && string.IsNullOrEmpty(history))
            return new List<string>();

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【KP 调查路径构建者】。

你的职责是：
从 KP 的视角，构思【调查员可能经历的不同调查故事线】。

你不是在写剧情结果，
而是在描述：调查可能沿着哪些方向展开。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【世界已经发生的历史（既成事实）】
{history}

━━━━━━━━━━━━━━━━━━━━
【未来可能遇到的情况（潜在事件 / 危险 / 线索）】
{(conds != null && conds.Count > 0 ? string.Join("\n", conds.Select(e => "- " + e)) : "（暂无）")}

━━━━━━━━━━━━━━━━━━━━
【你的任务（核心）】

从【KP 的视角】，列出若干条【调查员可能经历的调查故事线】。

每一条故事线应描述：
- 调查最初可能从哪里被注意到
- 调查如何逐步深入、串联线索
- 最终会逼近怎样的一类真相或威胁

━━━━━━━━━━━━━━━━━━━━
【严格规则（必须遵守）】

- 不写调查结果或结局
- 不触发、不完成任何条件事件
- 不假设调查员一定成功
- 不出现第二人称（你 / 你们）
- 不直接复述事件列表
- 不新增原文中不存在的世界信息
- 使用第三人称
- 语气像 KP 的“内部推演 / 备团笔记”
- 每一条故事线应彼此明显不同

━━━━━━━━━━━━━━━━━━━━
【输出格式（严格）】

- 返回一个字符串数组
- 每个字符串 = 一条完整的调查故事线描述
- 每条 3～6 句话为宜
- 不要编号
- 不要解释
- 不要 JSON 以外的内容

返回结构：
List<string>"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? new List<string>();
    }

    public static async Task<List<string>> GptExtractWorldProcesses(
        string history,
        List<string> conds,
        int allDays
    )
    {
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【社会阵营识别器】。

你的职责是：
从故事背景中，
识别【现实层面存在的社会阵营】。

这里的阵营不是哲学思想，
而是：在日常生活中真实存在、可以被指认的群体。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【故事背景】
{history}

━━━━━━━━━━━━━━━━━━━━
【补充背景（潜在事件，仅用于理解世界结构）】
{string.Join("\n", conds.Select(e => "- " + e))}

━━━━━━━━━━━━━━━━━━━━
【你的任务（非常重要）】

请列出这个世界中【主要存在的社会阵营】。

阵营示例（仅作说明，不要照抄）：
- 维持治安的警察与执法者
- 从事盗窃、走私、暴力的犯罪者
- 掌握部分真相但选择隐瞒的官方人员
- 被异常影响却尚未察觉的普通居民
- 私下活动的神秘信仰者或邪教团体

━━━━━━━━━━━━━━━━━━━━
【阵营规则】

- 阵营必须是【社会层面可识别的群体】
- 不要使用抽象哲学语言
- 不要使用隐喻或象征性表达
- 每个阵营应当“如果没有调查员介入，也会继续存在并行动”

━━━━━━━━━━━━━━━━━━━━
【输出格式（必须严格）】

- 返回 JSON 数组
- 每个元素为一个 string
- string 结构固定为：

""阵营=……
|角色定位=……
|主要目标=……""

━━━━━━━━━━━━━━━━━━━━
【禁止事项】

- 不写神话隐喻
- 不写宏大意识形态
- 不写剧情
- 不解释

返回结构：
List<string>
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? new List<string>();
    }

    public async Task<List<string>> GptGetMainStory(
        List<string> steams,
        string befStory,
        List<string> conds,
        int allDay
    )
    {
        if (allDay <= 0)
            return new List<string>();

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    $@"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【KP 主线时间轴导演】。

你的职责是：
在【没有调查员介入】的情况下，
为一个存在明确核心矛盾的世界，
生成一条【必然在 {allDay} 天内走向爆发的主线时间轴】。

你不是在描写异象，
你是在导演一场不可避免的失控过程。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【参与主线的阵营】
{string.Join("\n", steams.Select(s => "- " + s))}

━━━━━━━━━━━━━━━━━━━━
【世界既成历史（冲突的起点）】
{befStory}

━━━━━━━━━━━━━━━━━━━━
【潜在事件与未来走向（可作为手段与征兆）】
{string.Join("\n", conds.Select(c => "- " + c))}

━━━━━━━━━━━━━━━━━━━━
【时间限制】
{allDay} 天（主线必须在此时间内进入极端冲突或失控状态）

━━━━━━━━━━━━━━━━━━━━
【你的核心任务（绝对规则）】

1. 先在内部确定：
   - 1 条【核心主线矛盾】（阵营 vs 阵营）
   - 该矛盾在第 {allDay} 天或之前【不可逆爆发】

2. 将整个时间轴划分为三个阶段：
   - 前期（准备与掩盖）
   - 中期（征兆与对立）
   - 后期（公开冲突与失控）

3. 每一天只写：
   - 各阵营为【主线矛盾】所做的推进性行为
   - 次要冲突必须服务于主线升级

━━━━━━━━━━━━━━━━━━━━
【叙述硬性要求（非常重要）】

- 每一天必须比前一天【更接近失控】
- 不允许“并列异象”
- 不允许写成背景拼贴
- 必须体现：资源消耗、风险扩大、掩盖失效或对立公开
- 不出现调查员、玩家、第二人称
- 使用第三人称
- 按【从早到晚】顺序描述

━━━━━━━━━━━━━━━━━━━━
【输出格式（严格）】

- 返回字符串数组（List<string>）
- 数组长度 = {allDay}
- 每个字符串 = 当天发生的事情
- 每天 1～3 句话
- 不编号
- 不解释
- 不要 JSON 以外的内容

返回结构：
List<string>"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<string>>(messages);

        return result ?? new List<string>();
    }
    
    public async Task<string> GptGetMainConflict(
        List<string> steams,
        string befStory,
        List<string> conds,
        int allDay
    )
    {
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    $@"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【核心冲突解析器】。
    
    你的职责是：
    从多个阵营与世界状态中，
    抽象出【唯一的一条主要矛盾】。
    
    该矛盾必须在 {allDay} 天内自然升级为不可逆状态。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
    【参与世界运作的阵营】
    {string.Join("\n", steams.Select(s => "- " + s))}
    
    ━━━━━━━━━━━━━━━━━━━━
    【世界既成历史（冲突的根源）】
    {befStory}
    
    ━━━━━━━━━━━━━━━━━━━━
    【潜在事件与未来走向】
    {string.Join("\n", conds.Select(c => "- " + c))}
    
    ━━━━━━━━━━━━━━━━━━━━
    【你的任务（非常重要）】
    
    只做一件事：
    
    用【一句话】描述这个故事的【主要矛盾】。
    
    ━━━━━━━━━━━━━━━━━━━━
    【主矛盾判定规则】
    
    - 必须是【阵营 vs 阵营】
    - 双方目标无法同时实现
    - 冲突在 {allDay} 天内必然升级
    - 不涉及调查员或玩家
    - 不写结果、不写过程
    
    ━━━━━━━━━━━━━━━━━━━━
    【输出要求（严格）】
    
    - 只输出一行纯文本
    - 不要解释
    - 不要列表
    - 不要 JSON
    "
            }
        };
    
        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);
    
        return result?.Trim() ?? string.Empty;
    }
    public async Task<string> GptExtractStoryDriver(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个【主线故事驱动力解析器】。

你的职责是：
从文本中提取【唯一的一条主线驱动力】。

这条驱动力描述的是：
- 世界中正在发生的一个不可停止的进程
- 玩家介入，只能选择“如何应对”，而不能让它不存在

你不分析玩家心理，
你不描述体验，
你不写主题或氛围。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【故事背景文本】
{text}

━━━━━━━━━━━━━━━━━━━━
【你的任务（非常重要）】

从以上内容中，提取并总结：

【这一整个故事的主线驱动力】

判定标准（必须全部满足）：
- 只能有【一个】
- 必须是【世界层面的进程或冲突】
- 即使没有玩家，也会持续推进
- 可以用来回答问题：
  “如果不被阻止，最终会发生什么？”

━━━━━━━━━━━━━━━━━━━━
【禁止事项】

- 不要提调查员或玩家
- 不要使用隐喻或文学语言
- 不要列举多个动因
- 不要写心理感受

━━━━━━━━━━━━━━━━━━━━
【输出要求】

- 用一句清晰、分析性的陈述
- 输出为【纯文本】
- 不要列表
- 不要 JSON
- 不要解释
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        return result ?? string.Empty;
    }
    
    public async Task<string> GptExtractStoryStartTask(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【调查员引入情节解析器】。

你的职责是：
从文本中提取【调查员刚被引入故事时】KP 可以直接交付的信息。

你不写世界全貌，
你不总结主线，
你只关注“调查从哪里开始”。"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"
【故事背景文本】
{text}

━━━━━━━━━━━━━━━━━━━━
【你的任务（严格）】

从以上文本中，总结调查员介入时可以得到的三项信息：

1. 【调查员所处的背景现状】
   - 世界或地点表面上发生了什么异常
   - 必须是调查员一开始就能被告知的内容

2. 【调查员被引入的直接原因】
   - 是委托、求助、失踪、异常事件、官方任务等
   - 必须是一个明确的“为什么要来”

3. 【调查员的初始任务】
   - 刚开始需要调查、确认或处理的事情
   - 这是一个“可立刻行动”的目标

━━━━━━━━━━━━━━━━━━━━
【重要限制】

- 只使用文本中已存在的信息
- 不提前揭露隐藏真相
- 不引入主线结局或终极威胁
- 不出现调查员心理描写
- 不使用文学化或隐喻语言

━━━━━━━━━━━━━━━━━━━━
【输出格式（严格）】

输出为一段【结构清晰的说明文本】，
必须包含上述三部分内容，
但不要使用编号、列表或标题。

语气应当像：
KP 在开场时向调查员说明情况。

━━━━━━━━━━━━━━━━━━━━
【禁止事项】

- 不要 JSON
- 不要解释分析
- 不要加入新设定
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        return result ?? string.Empty;
    }


}