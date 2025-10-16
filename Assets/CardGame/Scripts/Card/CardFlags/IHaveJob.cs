using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
/// <summary>
/// 对工作的影响
/// </summary>
public interface IHaveJobEffect
{
    void Effect();
}

/// <summary>
/// 提供职业
/// </summary>
public interface IHaveJob
{
    /// <summary>
    /// 获取工作
    /// </summary>
    /// <returns></returns>
    JobInfo GetJob();
    /// <summary>
    /// 获取工作的效果
    /// </summary>
    /// <returns></returns>
    List<IHaveJobEffect> GetJobEffect();
    // /// <summary>
    // /// 添加晋级分数
    // /// </summary>
    // /// <param name="score"></param>
    // void AddScore(int score);
    // /// <summary>
    // /// 减小晋级分数
    // /// </summary>
    // /// <param name="score"></param>
    // void ReduceScore(int score);
    /// <summary>
    /// 获取分数
    /// </summary>
    /// <returns></returns>
    int GetScore();
    string getScoreText();
    Dictionary<string, int> GetJobDic();
}

public static class IHaveJobEffectExtensions
{
    public static string getScoreText(this IHaveJob jober)
    {
        var txt =jober.getScoreText();
        return txt;
    }
    /// <summary>
    /// 降低分数
    /// </summary>
    /// <param name="jober"></param>
    /// <param name="score"></param>
    public static void ReduceScore(this IHaveJob jober, int score)
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
    public static void AddScore(this IHaveJob jober, int score)
    {
        var str = jober.getScoreText();
        var ret = jober.GetJobDic();
        ret[str] = ret[str] + score;
    }

    public static int GetScore(this IHaveJob jober)
    {
        var str = jober.getScoreText();
        var ret = jober.GetJobDic();
        return ret[str];
    }
}