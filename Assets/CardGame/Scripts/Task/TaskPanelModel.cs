using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SlotRequire
{
    public Func<CardModel, bool> Require;
    /// <summary>
    /// 当满足时的处理
    /// </summary>
    public Func<CardModel, bool> OnAdd;
    public Func<CardModel, bool> OnRemove;
    public string name;
    public SlotRequire(Func<CardModel, bool> require, string name)
    {
        Require = require;
        this.name = name;
    }
}

[SerializeField]
public class TaskPanelModel:IModel
{
    [SerializeField]
    public ExeNode exeNode;
    public string description { get { return exeNode.description; } }
    public string title { get { return exeNode.title; } }
    /// <summary>
    /// 卡片映射
    /// </summary>
    public Dictionary<CardRequire,CardModel> cardsMap;
    public float beginTime;
    public TaskPanelModel()
    {
        cardsMap = new Dictionary<CardRequire,CardModel>();
    }

    public IView CreateView()
    {
        var view = GameFrameWork.Instance.taskPanel.GetComponent<TaskPanelView>();
        view.BindModel(this);
        return view;
    }
    public void AddCard(CardRequire cardRequire,CardModel cardModel)
    {
        cardsMap.Add(cardRequire, cardModel);
    }
    public void RemoveCard(CardRequire cardRequire)
    {
        cardsMap.Remove(cardRequire);
    }
    public float BeginTime()
    {
        return 1;
    }
    public float RemainTime()
    {
        return 1;
    }
    /// <summary>
    /// 时间变化时状态变化
    /// </summary>
    /// <returns></returns>
    public bool CheckTimeSwitch()
    {
        if(exeNode.GetExeType()== ExeType.WasterTime)
        {
            var te = (WasterTimeExeNode)exeNode;
            var remainTime = ((((WasterTimeExeNode)(exeNode)).GetTime() + beginTime) - Time.time);
            if(remainTime < 0)//需要进行转换
            {
                te.TimeSwitch(this);
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 当卡片切换时检测状态变化
    /// </summary>
    /// <returns></returns>
    public bool CanChangeCardSwitch()
    {
        return exeNode.WhenCardChange(this);
        //if (exeNode.GetExeType() == ExeType.WasterTime)
        //{
        //    return false;
        //}
        //else if (exeNode.GetExeType() == ExeType.Select)///选择节点
        //{
        //    return exeNode.WhenCardChange(this);
        //}
        //else//结束节点
        //{
        //    return false;
        //}
    }
    /// <summary>
    /// 点击继续的时候检测状态变化
    /// </summary>
    /// <returns></returns>
    public bool CanClickSwitch()
    {
        if (exeNode.GetExeType() == ExeType.WasterTime)
        {
            return false;
        }
        else if (exeNode.GetExeType() == ExeType.Select)///选择节点
        {
            return ((SelectExeNode)exeNode).CanClickChange(this);
        }
        else//结束节点
        {
            return ((FinishExeNode)exeNode).CanClickChange(this);
        }
    }

    /// <summary>
    /// 进行状态切换
    /// </summary>
    //public bool Switch()
    //{
    //    if(exeNode.GetExeType() == ExeType.WasterTime)
    //    {
    //        var wasterNode = (WasterTimeExeNode)exeNode;
    //        if (wasterNode.CanProcess(this))
    //        {
    //            wasterNode.Process(this);
    //            return true;
    //        }
    //    }
    //    else if(exeNode.GetExeType() == ExeType.Select)
    //    {
    //        var selectNode = (SelectExeNode)exeNode;
    //        if (selectNode.CanProcess(this))
    //        {
    //            selectNode.Process(this);
    //            return true;
    //        }
    //    }
    //    else///结束节点
    //    {
    //        var finishNode = (FinishExeNode)exeNode;
    //        var cards = finishNode.GetCards(this);
    //        foreach(var x in cards)
    //        {
    //            GameFrameWork.Instance.AddCardByCardData(x,new Vector3(0,0,0));
    //        }

    //        return true;
    //    }

    //    return false;
    //}
    public void SetExeNode(ExeNode exeNode)
    {
        this.exeNode = exeNode;
        beginTime = Time.time;
    }
    public float GetRemainTime()
    {
        return ((((WasterTimeExeNode)(exeNode)).GetTime() + beginTime) - Time.time);
    }
}
