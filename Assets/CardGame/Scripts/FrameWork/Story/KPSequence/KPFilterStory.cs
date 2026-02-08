using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class KPFilterStory
{
    /// <summary>
    /// 第一步把PDF整理成文本信息,再精简下模组
    /// </summary>
    [Button("读取信息")]
    public async Task AskGptText()
    {
        var GptLongTextProcessor = new GptLongTextProcessor();
        var rawText = KPSystem.Load("模组");
        var str = await GptLongTextProcessor.ProcessLongText(rawText);
        KPSystem.Save("模组精简",str);
        Debug.Log("PDF 文本规范化整理完成（分段模式）");
    }
    /// <summary>
    /// 第二步过滤gpt的信息，把它生成KV表列出场景中有哪些东西
    /// </summary>
    [Button("过滤信息")]
    public async Task FilterGptText()
    {
        Dictionary<string,string> infoDictionary = new Dictionary<string, string>();
        List<string> storyHappen = new();
        var rawText = KPSystem.Load("模组精简");
        var chunks = GptLongTextProcessor.SplitText(rawText,2000);
        int index = 1;
        foreach (var str in chunks)
        {
            Debug.Log(index+"/"+chunks.Count+".....");
            index++;
            var data = await KPSystem.GptFilterInfo(infoDictionary,str);
            foreach (var kv in data.mapInfo)
            {
                if (infoDictionary.ContainsKey(kv.Key))
                {
                    infoDictionary[kv.Key]=await KPSystem.GptCombineInfo(kv.Key,infoDictionary[kv.Key],kv.Value);
                }
                else
                {
                    infoDictionary[kv.Key] = kv.Value;
                }
            }
            var events = await KPSystem.GptExtractStoryHappen(str);
            storyHappen = events;
        }

        KPSystem.Save<Dictionary<string, string>>("数据字典", infoDictionary);
        return;
    }
    /// <summary>
    /// 进一步为过滤的KV信息来天假类型
    /// </summary>
    [Button("生成带类型字典")]
    public static async Task BuideHasTypeDic()
    {
        var infoDictionary = KPSystem.Load<Dictionary<string, string>>("数据字典");
        Dictionary<string, CocDicItem> typedDict =
            new Dictionary<string, CocDicItem>();

        int typeIndex = 1;
        foreach (var kv in infoDictionary)
        {
            Debug.Log($"TypeCheck {typeIndex}/{infoDictionary.Count} : {kv.Key}");
            typeIndex++;

            var item = await KPSystem.GptCheckCocDicItemType(kv.Key, kv.Value);
            if (item != null)
            {
                typedDict[kv.Key] = item;
            }
        }
        KPSystem.Save<Dictionary<string, CocDicItem>>("数据字典_typed", typedDict);
        return;
    }
    /// <summary>
    /// 根据模组的文本，整理出故事已经发生的事情,和未发生的事情
    /// </summary>
    [Button("整理故事信息")]
    public async Task StoryFilterGptText()
    {
        List<string> storyHappen = new();
        var rawText = KPSystem.Load("模组精简");
        var chunks = GptLongTextProcessor.SplitText(rawText,4000);
        int index = 1;
        foreach (var str in chunks)
        {
            Debug.Log(index+"/"+chunks.Count+".....");
            index++;
            var events = await KPSystem.GptExtractStoryHappen(str);
            
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
        var ret = KPSystem.SplitStoryHappen(storyHappen);
        int size = 10; // 你可以调 5 / 10 / 20
        var conds = await KPSystem.CombineAll(ret.Conditional, size);
        var happened = await KPSystem.CombineAll(ret.Happened, size);
        KPSystem.Save("预期事件", conds);
        KPSystem.Save("已经发生", happened);
        return;
    }
    /// <summary>
    /// 输出已经发生的事情
    /// </summary>
    [Button("输出已经发生的故事")]
    public async Task GenerateHistoryStory()
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
    }
}