using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 行为在叙事中的可行性
/// </summary>
public enum NarrativePossibility
{
    Impossible,
    Possible,
    Trivial
}

/// <summary>
/// GPT 返回的叙事可行性判断
/// </summary>
public class GptNarrativeCheckResult
{
    public NarrativePossibility possibility;

    /// <summary>
    /// 若需要检定，最低成功等级要求
    /// </summary>
    public CocCheckResult requiredSuccessLevel;

    /// <summary>
    /// KP 给出的判断理由
    /// </summary>
    public string reason;
}

/// <summary>
/// 最终对外返回的叙事判定结果
/// </summary>
public class NarrativeResult
{
    public bool success;
    public NarrativePossibility possibility;
    public CocCheckResult? checkResult;
    public CocCheckResult? requiredSuccessLevel;
    public string narrativeReason;
}

/// <summary>
/// CoC 成功判断工具
/// </summary>
public static class CocCheckUtil
{
    public static bool IsSuccess(CocCheckResult result)
    {
        return result == CocCheckResult.CriticalSuccess
            || result == CocCheckResult.ExtremeSuccess
            || result == CocCheckResult.HardSuccess
            || result == CocCheckResult.Success;
    }

    /// <summary>
    /// 是否满足最低成功等级要求
    /// </summary>
    public static bool IsSuccessAtLeast(
        CocCheckResult result,
        CocCheckResult required
    )
    {
        return GetRank(result) >= GetRank(required);
    }

    private static int GetRank(CocCheckResult result)
    {
        switch (result)
        {
            case CocCheckResult.Fumble:           return 0;
            case CocCheckResult.Failure:          return 1;
            case CocCheckResult.Success:          return 2;
            case CocCheckResult.HardSuccess:      return 3;
            case CocCheckResult.ExtremeSuccess:   return 4;
            case CocCheckResult.CriticalSuccess:  return 5;
            default: return 0;
        }
    }
}

/// <summary>
/// 自动 KP：叙事判定系统（核心）
/// </summary>
public class NarrativeCheck
{
    private RollSystem rollSystem;

    public NarrativeCheck(RollSystem rollSystem)
    {
        this.rollSystem = rollSystem;
    }

    /// <summary>
    /// 执行一次叙事行为判定
    /// </summary>
    public async Task<NarrativeResult> CheckAsync(
        string actionDescription,
        string sceneDescription,
        int skillValue
    )
    {
        var gptResult = await AskGptNarrativePossibility(
            actionDescription,
            sceneDescription
        );

        switch (gptResult.possibility)
        {
            case NarrativePossibility.Impossible:
                return new NarrativeResult
                {
                    success = false,
                    possibility = NarrativePossibility.Impossible,
                    narrativeReason = gptResult.reason
                };

            case NarrativePossibility.Trivial:
                return new NarrativeResult
                {
                    success = true,
                    possibility = NarrativePossibility.Trivial,
                    narrativeReason = gptResult.reason
                };

            case NarrativePossibility.Possible:
                var checkResult = GameFrameWork.Instance.rollSystem.Check(skillValue);
                bool success = CocCheckUtil.IsSuccessAtLeast(
                    checkResult,
                    gptResult.requiredSuccessLevel
                );

                return new NarrativeResult
                {
                    success = success,
                    possibility = NarrativePossibility.Possible,
                    checkResult = checkResult,
                    requiredSuccessLevel = gptResult.requiredSuccessLevel,
                    narrativeReason = gptResult.reason
                };

            default:
                throw new Exception("未知的叙事可行性结果");
        }
    }

    /// <summary>
    /// 向 GPT 询问叙事可行性（KP 判断）
    /// </summary>
    private async Task<GptNarrativeCheckResult> AskGptNarrativePossibility(
        string actionDescription,
        string sceneDescription
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptNarrativeCheckResult));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
@"你是一名克苏鲁神话跑团的守密人（KP）。
你只负责判断【行为是否可能】以及【需要的成功等级】。
不要进行掷骰，不要扩展剧情，不要引入新事实。
判断必须现实、冷酷，并符合 CoC 规则。"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
$@"【当前场景描述】
{sceneDescription}

【玩家尝试的行为】
{actionDescription}

请判断：
1. 该行为属于：
   - Impossible（不可能）
   - Trivial（无需检定，必然成功）
   - Possible（需要检定）

2. 若需要检定，请指定最低成功等级：
   - Success
   - HardSuccess
   - ExtremeSuccess
   - CriticalSuccess

请给出简短理由。

请严格使用 JSON 格式返回：
{schema}"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GptNarrativeCheckResult>(messages);
    }
}
