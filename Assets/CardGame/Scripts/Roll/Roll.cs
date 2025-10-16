using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoCDiceRoller
{
    private System.Random random;

    public CoCDiceRoller(int? seed = null)
    {
        random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
    }

    /// <summary>
    /// 投一次COC判定骰
    /// </summary>
    /// <param name="skill">技能值 (1-100)</param>
    /// <returns>RollResult 包含结果信息</returns>
    public RollResult Roll(int skill)
    {
        int roll = random.Next(1, 101); // 范围 [1,100]

        string outcome;
        if (roll == 1)
        {
            outcome = "大成功";
        }
        else if (roll == 100 || (skill < 50 && roll >= 96))
        {
            outcome = "大失败";
        }
        else if (roll <= skill / 5)
        {
            outcome = "极难成功";
        }
        else if (roll <= skill / 2)
        {
            outcome = "困难成功";
        }
        else if (roll <= skill)
        {
            outcome = "成功";
        }
        else
        {
            outcome = "失败";
        }

        return new RollResult(skill, roll, outcome);
    }
}

public struct RollResult
{
    public int Skill { get; private set; }
    public int Roll { get; private set; }
    public string Outcome { get; private set; }

    public RollResult(int skill, int roll, string outcome)
    {
        Skill = skill;
        Roll = roll;
        Outcome = outcome;
    }

    public override string ToString()
    {
        return $"技能 {Skill} | 投骰 {Roll} → {Outcome}";
    }
}
