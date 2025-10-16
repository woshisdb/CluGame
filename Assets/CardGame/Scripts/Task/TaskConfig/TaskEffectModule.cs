using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

/// <summary>
/// 任务的效果
/// </summary>
public abstract class TaskEffect
{
    public abstract void OnEffect(TaskPanelModel task);
}

public class InteractionCard_TaskEffect : TaskEffect
{
    public string obj;
    public Dictionary<string,string> mapper;

    public InteractionCard_TaskEffect()
    {
        mapper = new Dictionary<string,string>();
    }

    public override void OnEffect(TaskPanelModel task)
    {
        var nowMap = task.cardsMap;
        var owner = nowMap[obj];
        var ret = new Dictionary<string, CardModel>();
        foreach (var str in mapper)
        {
            ret[str.Value]=nowMap[str.Key];
        }
        ((IInteractionCard)owner).Interaction(ret,task);
    }
}

/// <summary>
/// 获取卡片效果
/// </summary>
public class GetCard_TaskEffect:TaskEffect
{
    public List<CardEnum> cards;
    public GetCard_TaskEffect()
    {
        cards = new List<CardEnum>();
    }
    public override void OnEffect(TaskPanelModel task)
    {
        foreach (var x in cards)
        {
            GameFrameWork.Instance.AddCardByEnum(x,Vector3.zero);
        }
    }
}
/// <summary>
/// 移除卡片效果
/// </summary>
public class RemoveCard_TaskEffect:TaskEffect
{
    public List<CardEnum> cards;
    public RemoveCard_TaskEffect()
    {
        cards = new List<CardEnum>();
    }
    public override void OnEffect(TaskPanelModel task)
    {
    }
}
/// <summary>
/// 下一个任务
/// </summary>
public class NextTask_TaskEffect:TaskEffect
{
    public string nextTaskName;
    public override void OnEffect(TaskPanelModel task)
    {
        var taskCfg = GameFrameWork.Instance.gameConfig.GetTask(nextTaskName);
        if (taskCfg!=null)
        {
            task.SetExeNode(taskCfg);
        }
    }
}

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

public class FromCardTextType:TextType
{
    /// <summary>
    /// 显示卡片内容的id
    /// </summary>
    public string cardId;
    public override string getStr(TaskPanelModel task,ChangeTextType changeTextType)
    {
        var ret = ((IChangeTextCard)(task.cardsMap[cardId])).ChangeText(task,changeTextType);
        return ret;
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