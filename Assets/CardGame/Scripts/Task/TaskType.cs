using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Sleep,
}

public abstract class CardRequire
{
    /// <summary>
    /// 需求卡牌名字
    /// </summary>
    public string name;
    public CardRequire(string name)
    {
        this.name = name;
    }

    public abstract bool Require(CardModel card);
}
/// <summary>
/// 执行的枚举
/// </summary>
public enum ExeEnum
{
    e1,
}

public enum ExeType
{
    /// <summary>
    /// 可以直接获取卡片
    /// </summary>
    Finish,
    /// <summary>
    /// 卡片选择
    /// </summary>
    Select,
    /// <summary>
    /// 卡片花费时间
    /// </summary>
    WasterTime,
}


/// <summary>
/// 执行节点
/// </summary>
[Serializable]
public abstract class ExeNode
{
    /// <summary>
    /// 执行的类型
    /// </summary>
    public abstract ExeType GetExeType();
    /// <summary>
    /// 描述
    /// </summary>
    public string description;
    /// <summary>
    /// 标题
    /// </summary>
    public string title;
    public ExeEnum exeEnum;
    /// <summary>
    /// 转到对应节点
    /// </summary>
    /// <param name="kv"></param>
    /// <returns></returns>
    //public abstract ExeProcess SwitchTo(Dictionary<CardRequire, CardModel> kv);
    public ExeNode(string title,string description)
    {
        this.title = title;
        this.description = description;
    }
    public abstract ExeEnum GetExeEnum();
}

public abstract class SelectExeNode:ExeNode
{
    /// <summary>
    /// 需要的一系列卡牌
    /// </summary>
    public List<CardRequire> cardRequires;

    protected SelectExeNode(string title, string description) : base(title, description)
    {
    }

    public override ExeType GetExeType()
    {
        return ExeType.Select;
    }
    /// <summary>
    /// 是否可以执行
    /// </summary>
    /// <returns></returns>
    public abstract bool CanProcess(TaskPanelModel task);
    /// <summary>
    /// 执行
    /// </summary>
    public abstract void Process(TaskPanelModel task);
}

public abstract class WasterTimeExeNode:ExeNode
{
    protected WasterTimeExeNode(string title, string description) : base(title, description)
    {
    }
    public abstract void Process(TaskPanelModel task);
    public override ExeType GetExeType()
    {
        return ExeType.Select;
    }
    public abstract float GetTime();
}

public abstract class FinishExeNode : ExeNode
{
    protected FinishExeNode(string title, string description) : base(title, description)
    {
    }

    public override ExeType GetExeType()
    {
        return ExeType.Finish;
    }
    public abstract List<CardData> GetCards(TaskPanelModel model);
}