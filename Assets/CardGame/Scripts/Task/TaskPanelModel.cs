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
    /// 刷新并且返回状态是否变化
    /// </summary>
    /// <returns></returns>
    public void hasSwitch()
    {
        
    }
    /// <summary>
    /// 切换
    /// </summary>
    public bool Switch()
    {
        if(exeNode.GetExeType() == ExeType.WasterTime)
        {
            var wasterNode = (WasterTimeExeNode)exeNode;
            if (wasterNode.CanProcess())
            {
                wasterNode.Process(this);
                return true;
            }
        }
        else if(exeNode.GetExeType() == ExeType.Select)
        {
            var selectNode = (SelectExeNode)exeNode;
            if (selectNode.CanProcess())
            {
                selectNode.Process(this);
                return true;
            }
        }
        else///结束节点
        {
            var finishNode = (FinishExeNode)exeNode;
            var cards = finishNode.GetCards(this);
            foreach(var x in cards)
            {
                GameFrameWork.Instance.AddCardByCardData(x);
            }

            return true;
        }

        return false;
    }
}
