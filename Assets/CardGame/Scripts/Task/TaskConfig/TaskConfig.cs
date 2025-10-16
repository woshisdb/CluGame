using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public enum TransType
{
    one,//只要一个就行
    all,//全部都满足
}
/// <summary>
/// 状态转移配置
/// </summary>
public abstract class CardConfig
{
    public abstract bool isSat(CardModel cardModel);
}
/// <summary>
/// Flag的卡片转移
/// </summary>
public class TagCardConfig: CardConfig
{
    /// <summary>
    /// 转移的类型
    /// </summary>
    public TransType transType;
    public List<CardFlag> cardFlags;
    public TagCardConfig()
    {
        cardFlags = new List<CardFlag>();
    }

    public override bool isSat(CardModel cardModel)
    {
        if (transType == TransType.all)
        {
            foreach (var x in cardFlags)
            {
                if (!cardModel.hasFlag(x))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            foreach (var x in cardFlags)
            {
                if (cardModel.hasFlag(x))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
/// <summary>
/// CardEnum的卡片转移
/// </summary>
public class CardTransConfig : CardConfig
{
    /// <summary>
    /// 转移的类型
    /// </summary>
    public List<CardEnum> cardFlags;
    public CardTransConfig()
    {
        cardFlags= new List<CardEnum>();
    }

    public override bool isSat(CardModel cardModel)
    {
        foreach (var x in cardFlags)
        {
            if (cardModel.cardEnum==x)
            {
                return true;
            }
        }
        return false;
    }
}


/// <summary>
/// 任务的类型
/// </summary>
[CreateAssetMenu(fileName = "TaskConfig", menuName = "Task/TaskConfig")]
public class TaskConfig:SerializedScriptableObject
{
    public string taskName;
    /// <summary>
    /// 任务配置模块
    /// </summary>
    public TaskConfigModule taskConfigModules;
    public TaskConfig()
    {
        taskConfigModules = new TaskConfigModule();
    }
}

