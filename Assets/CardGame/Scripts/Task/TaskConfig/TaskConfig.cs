using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public TransType condType;
    public List<string> conds;

    public CardConfig()
    {
        conds = new List<string>();
    }
    public abstract bool isSat(CardModel cardModel);
    // 根据函数名调用当前类的方法
    public object CallMethod(string methodName, params object[] parameters)
    {
        // 获取当前类的类型
        Type type = this.GetType();
        
        // 查找方法（BindingFlags指定查找范围：公开方法+实例方法）
        MethodInfo method = type.GetMethod(
            methodName, 
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        );

        if (method == null)
        {
            Debug.Log($"未找到方法：{methodName}");
            return null;
        }

        // 调用方法（第一个参数：实例对象，静态方法传null；第二个参数：参数列表）
        object result = method.Invoke(method.IsStatic ? null : this, parameters);
        return result;
    }

    public bool IsSatConfig(CardModel cardModel)
    {
        if (conds.Count==0)
        {
            return true;
        }
        else
        {
            if (condType == TransType.all)
            {
                foreach (var vb in conds)
                {
                    var ret = CallMethod(vb,cardModel);
                    if (ret != null&&(bool)ret == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (var vb in conds)
                {
                    var ret = CallMethod(vb,cardModel);
                    if (ret != null&&(bool)ret == true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    
    //-------------------------静态配置
    public bool TestCfg1(CardModel cardModel)
    {
        return true;
    }
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
        if (!IsSatConfig(cardModel))
        {
            return false;
        }
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
        if (!IsSatConfig(cardModel))
        {
            return false;
        }
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
/// 传入函数的参数
/// </summary>
public class CardFuncConfig : CardConfig
{
    public Func<CardModel,bool> satFunc;
    public Func<TaskPanelModel, List<CardModel>> canSelCards;
    public CardFuncConfig(Func<CardModel,bool> satFunc,Func<TaskPanelModel, List<CardModel>> canSelCards)
    {
        this.satFunc = satFunc;
        this.canSelCards = canSelCards;
    }
    public override bool isSat(CardModel cardModel)
    {
        return satFunc(cardModel);
    }
}
//
// /// <summary>
// /// 任务的类型
// /// </summary>
// [CreateAssetMenu(fileName = "TaskConfig", menuName = "Task/TaskConfig")]
public class TaskConfig
{
    public string taskName;
    /// <summary>
    /// 任务配置模块
    /// </summary>
    public TaskConfigModule taskConfigModules;
    public TaskConfig(string name,TaskConfigModule task)
    {
        taskName = name;
        taskConfigModules = task;
    }
}

