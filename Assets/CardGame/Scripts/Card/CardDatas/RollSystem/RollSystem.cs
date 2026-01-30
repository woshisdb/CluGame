
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public enum CocCheckResult
{
    CriticalSuccess,   // 大成功（01 或 ≤极低阈值）
    ExtremeSuccess,    // 极难成功（≤技能/5）
    HardSuccess,       // 困难成功（≤技能/2）
    Success,           // 普通成功（≤技能）
    Failure,           // 失败
    Fumble             // 大失败（96-00 或阈值以上）
}

public class RollSystem
{
    /// <summary>
    /// CoC 百分骰检定
    /// </summary>
    /// <param name="skillValue">技能值（0~100）</param>
    /// <returns>检定结果枚举</returns>
    public CocCheckResult Check(int skillValue)
    {
        int roll = Random.Range(1, 101); // 1~100
        return ResolveResult(roll, skillValue);
    }
    /// <summary>
    /// 带动画的 CoC 检定（动画播完后回调）
    /// </summary>
    public CocCheckResult CheckWithAnim(int skillValue, Action<int, CocCheckResult> done)
    {
        int roll = Random.Range(1, 101);
        var result = ResolveResult(roll, skillValue);

        // TODO: 播放动画
        done?.Invoke(roll, result);

        return result;
    }
    /// <summary>
    /// 判断 CoC 检定结果是否为“成功”
    /// </summary>
    public static bool IsSuccess(CocCheckResult result)
    {
        switch (result)
        {
            case CocCheckResult.CriticalSuccess:
            case CocCheckResult.ExtremeSuccess:
            case CocCheckResult.HardSuccess:
            case CocCheckResult.Success:
                return true;

            case CocCheckResult.Failure:
            case CocCheckResult.Fumble:
            default:
                return false;
        }
    }
    /// <summary>
    /// 根据掷骰和技能值解析 CoC 判定结果
    /// </summary>
    private CocCheckResult ResolveResult(int roll, int skill)
    {
        // 大成功
        if (roll == 1)
            return CocCheckResult.CriticalSuccess;

        // 大失败（CoC7：技能 < 50 → 96-100；技能 ≥50 → 100）
        if ((skill < 50 && roll >= 96) || (skill >= 50 && roll == 100))
            return CocCheckResult.Fumble;

        if (roll <= skill / 5)
            return CocCheckResult.ExtremeSuccess;

        if (roll <= skill / 2)
            return CocCheckResult.HardSuccess;

        if (roll <= skill)
            return CocCheckResult.Success;

        return CocCheckResult.Failure;
    }

    public CheckCardModel CheckWithCardsAnim(List<CheckCardModel> cards, Action done)
    {
        var card = GetRandomItem(cards);
        done?.Invoke();
        return card;
    }

    private T GetRandomItem<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            throw new Exception("列表为空，无法随机选取！");
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    public static bool IsSuccessAtLeast(
        CocCheckResult result,
        CocCheckResult requiredLevel
    )
    {
        return GetRank(result) >= GetRank(requiredLevel);
    }

    public static int GetRank(CocCheckResult result)
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
