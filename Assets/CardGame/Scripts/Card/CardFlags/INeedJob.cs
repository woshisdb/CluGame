using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 金钱的卡牌
/// </summary>
[InterfaceBind(CardFlag.needJob)]
public interface INeedJob : ICardFlag
{
    /// <summary>
    /// 工作的名字
    /// </summary>
    /// <returns></returns>
    string JobName();
    /// <summary>
    /// 工作的描述
    /// </summary>
    /// <returns></returns>
    string JobAim();
    /// <summary>
    /// 工作的数据信息
    /// </summary>
    /// <returns></returns>
    Dictionary<string, int> GetJobDic();
    IHaveJob GetJob();
}

public static class IHaveJobEffectExtensions
{
    public static string getScoreText(this INeedJob jober)
    {
        var txt =jober.getScoreText();
        return txt;
    }
    /// <summary>
    /// 降低分数
    /// </summary>
    /// <param name="jober"></param>
    /// <param name="score"></param>
    public static void ReduceScore(this INeedJob jober, int score)
    {
        var str = jober.getScoreText();
        var ret = jober.GetJobDic();
        ret[str] = ret[str] - score;
    }
    /// <summary>
    /// 增加分数
    /// </summary>
    /// <param name="jober"></param>
    /// <param name="score"></param>
    public static void AddScore(this INeedJob jober, int score)
    {
        var str = jober.getScoreText();
        var ret = jober.GetJobDic();
        ret[str] = ret[str] + score;
    }

    public static int GetScore(this INeedJob jober)
    {
        var str = jober.getScoreText();
        var ret = jober.GetJobDic();
        return ret[str];
    }
}