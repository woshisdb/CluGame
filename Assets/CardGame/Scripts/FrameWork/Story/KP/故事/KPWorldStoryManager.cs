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
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【开场场景生成器】。

你的职责是：
从模组文本中提取【调查员刚被引入故事时】的场景描述。

规则：
- 描述调查员所处的初始场景
- 包含场景中的关键角色
- 营造克苏鲁风格的氛围
- 不推进剧情，只描述当前状态
- 使用第三人称、描述性语言"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【你的任务】

生成一个【开场场景描述】，用于 KP 模式的 context 字段。

场景描述应包含：
- 场景地点和环境
- 场景中的关键角色（用他们的名字）
- 初始的氛围和异常现象
- 调查员可能注意到的重要细节

输出要求：
- 只输出纯文本
- 不使用 JSON
- 不使用编号或列表
- 直接输出场景描述

示例格式：
一个昏暗的电梯里，侦探A和嫌疑人B站在一起。电梯在上升过程中突然停顿，灯光闪烁，空气中弥漫着淡淡的铁锈味。墙壁上似乎有模糊的污渍，像是某种液体留下的痕迹。"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        firstSceneContext = result ?? string.Empty;
    }
}