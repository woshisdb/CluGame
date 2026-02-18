using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class KPWorldStoryTask
{
    public string description;
    public string title;
}

public class KPWorldStoryManager
{
    public List<KPWorldStoryTask> kpTask;
    public string firstSceneContext;
    public KPSpaceStoryManager KPSpaceStoryManager;
    [Button]
    public async void StartStory()
    {
        var cocText = KPSystem.Load("模组精简");
        
        if (string.IsNullOrEmpty(cocText))
        {
            Debug.LogError("模组精简文本为空！请先执行 KPSystem 的相关操作生成模组精简。");
            return;
        }

        await GenerateTasks(cocText);
        await GenerateFirstScene(cocText);
        
        Debug.Log("故事初始化完成！");
        Debug.Log($"任务数量: {kpTask?.Count ?? 0}");
        Debug.Log($"第一个场景: {firstSceneContext}");
        
        var spaceManager = new KPSpaceStoryManager();
        this.KPSpaceStoryManager = spaceManager;
        spaceManager.context = firstSceneContext;
        spaceManager.StartSpaceStory();
    }

    private async Task GenerateTasks(string cocText)
    {
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【任务提取器】。
你的职责是：
从模组文本中提取【调查员需要完成的主要任务】。
规则：
- 任务必须是可行动的、明确的目标
- 任务必须与模组的核心剧情相关
- 不生成背景介绍或氛围描述
- 不生成过于细小的子任务
- 每个任务都应该对剧情推进有重要作用"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【你的任务】

从模组中提取【调查员需要完成的主要任务】。

每个任务必须包含：
- title：任务标题（简洁明了）
- description：任务描述（包含任务目标、关键信息、重要性）

输出要求：
- 返回 JSON 数组
- 只输出 JSON，不输出其他内容
- 数组格式：List<KPWorldStoryTask>

示例格式：
[
  {{
    ""title"": ""调查失踪案件"",
    ""description"": ""调查员需要查明最近镇上发生的多起失踪案件，找出失踪者的下落和原因。这是模组的核心线索，将引导调查员发现更大的阴谋。""
  }}
]"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<List<KPWorldStoryTask>>(messages);

        kpTask = result ?? new List<KPWorldStoryTask>();
    }

    private async Task GenerateFirstScene(string cocText)
    {
        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【首次接触场景生成器】。

你的职责是：
从模组中找到调查员的导入故事，在什么地方发生什么事情。

规则：
- 这是调查员真正开始故事的第一个场景
- 营造克苏鲁风格的氛围
- 使用第三人称、描述性语言"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【你的任务】

获取coc模组文本中导入场景（例如PL在某个酒馆收到了一份委托）。

场景描述应包含：
- 调查员首次接触异常事件后的场景地点和环境
- 场景中的关键角色（用他们的名字）

输出要求：
- 只输出纯文本
- 不使用 JSON
- 不使用编号或列表
- 直接输出场景描述

示例格式：
在警局的审讯室里，侦探A和嫌疑人B面对面坐着。审讯室的灯光昏暗，墙上挂着单向玻璃。嫌疑人B看起来紧张不安，手指不停地敲击桌面。空气中弥漫着陈旧的烟草味。审讯室的门紧闭，隔音效果很好，外面的声音几乎听不见。"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        firstSceneContext = result ?? string.Empty;
    }
}