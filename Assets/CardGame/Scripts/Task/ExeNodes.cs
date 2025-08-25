using System;
using System.Collections.Generic;
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
    /// <summary>
    /// 当卡片切换的时候是否状态切换
    /// </summary>
    /// <returns></returns>
    public abstract bool WhenCardChange(TaskPanelModel task);

    ///// <summary>
    ///// 点击或时间到的时候进行状态切换
    ///// </summary>
    ///// <returns></returns>
    //public abstract bool CanProcess(TaskPanelModel task);
    ///// <summary>
    ///// 状态切换
    ///// </summary>
    //public abstract void Process(TaskPanelModel task);
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
    public abstract bool CanClickChange(TaskPanelModel task);
}

public abstract class WasterTimeExeNode:ExeNode
{
    protected WasterTimeExeNode(string title, string description) : base(title, description)
    {
    }
    public override ExeType GetExeType()
    {
        return ExeType.WasterTime;
    }
    public abstract float GetTime();
    /// <summary>
    /// 时间到了之后切换
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public abstract bool TimeSwitch(TaskPanelModel model);
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
    public abstract bool CanClickChange(TaskPanelModel task);
    public abstract List<CardData> GetCards(TaskPanelModel model);
}