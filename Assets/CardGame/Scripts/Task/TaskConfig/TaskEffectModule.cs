using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 任务的效果
/// </summary>
public abstract class TaskEffect
{
    public abstract void OnEffect(TaskPanelModel task);
}


// /// <summary>
// /// 获取卡片效果
// /// </summary>
// public class GetCard_TaskEffect:TaskEffect
// {
//     public List<CardEnum> cards;
//     public GetCard_TaskEffect()
//     {
//         cards = new List<CardEnum>();
//     }
//     public override void OnEffect(TaskPanelModel task)
//     {
//         foreach (var x in cards)
//         {
//             GameFrameWork.Instance.AddCardByEnum(x,Vector3.zero);
//         }
//     }
// }
// /// <summary>
// /// 移除卡片效果
// /// </summary>
// public class RemoveCard_TaskEffect:TaskEffect
// {
//     public List<CardEnum> cards;
//     public RemoveCard_TaskEffect()
//     {
//         cards = new List<CardEnum>();
//     }
//     public override void OnEffect(TaskPanelModel task)
//     {
//     }
// }

public enum ChangeTextType
{
    title,
    description
}

public abstract class TextType
{
    public abstract string getStr(TaskPanelModel task,ChangeTextType changeTextType);
}

public class CustomCardTextType:TextType
{
    public string str;

    public override string getStr(TaskPanelModel task,ChangeTextType changeTextType)
    {
        return str;
    }
}


/// <summary>
/// 改变任务名
/// </summary>
public class ChangeText_TaskEffect : TaskEffect
{
    public Dictionary<ChangeTextType,TextType> texlist;

    public ChangeText_TaskEffect()
    {
        texlist = new Dictionary<ChangeTextType, TextType>();
    }
    public override void OnEffect(TaskPanelModel task)
    {
        task.textList.Clear();
        var ret = new Dictionary<ChangeTextType,string>();
        foreach (var item in texlist)
        {
            var x = item.Value.getStr(task, item.Key);
            ret[item.Key] = x;
        }
        task.textList = ret;
    }
}

/// <summary>
/// 任务的效果
/// </summary>
public class TaskEffectModule
{
    /// <summary>
    /// 一系列的任务效果
    /// </summary>
    public List<TaskEffect> effects;
    public TaskEffectModule()
    {
        effects = new List<TaskEffect>();
    }

    public virtual void OnEffect(TaskPanelModel task)
    {
        foreach (var eff in effects)
        {
            eff.OnEffect(task);
        }
    }
}