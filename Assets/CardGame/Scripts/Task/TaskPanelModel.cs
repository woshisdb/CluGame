using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

[SerializeField]
public class TaskPanelModel:IModel
{
    [SerializeField]
    public TaskConfig exeNode;
    public string description { get { return exeNode.taskConfigModules.description; } }
    public string title { get { return exeNode.taskConfigModules.title; } }
    /// <summary>
    /// 卡片映射
    /// </summary>
    public Dictionary<string,CardModel> cardsMap;

    public Dictionary<ChangeTextType,string> textList;
    public float beginTime;
    public TaskPanelModel()
    {
        cardsMap = new Dictionary<string,CardModel>();
        textList = new Dictionary<ChangeTextType, string>();
    }

    public IView CreateView()
    {
        var view = GameFrameWork.Instance.taskPanel.GetComponent<TaskPanelView>();
        view.BindModel(this);
        return view;
    }
    public void AddCard(string cardRequire,CardModel cardModel)
    {
        if (cardsMap.ContainsKey(cardRequire))
        {
            cardsMap.Remove(cardRequire);
        }
        cardsMap.Add(cardRequire, cardModel);
    }
    public void RemoveCard(string cardRequire)
    {
        cardsMap.Remove(cardRequire);
    }

    public void SetTaskText(Dictionary<ChangeTextType,string> dic)
    {
        textList.Clear();
        textList = dic;
        GameFrameWork.Instance.viewModelManager.RefreshView(this);
    }

    public bool IsSatCard(string name)
    {
        return TryFindCard(name) != null;
    }
	public CardModel TryFindCard(string cardRequire)
	{
        if (!cardsMap.ContainsKey(cardRequire))
        {
            return null;
        }
        else
        {
            return cardsMap[cardRequire];
        }
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
        if(exeNode.taskConfigModules is TaskConfig_TimeOut)
        {
            var te = (TaskConfig_TimeOut)exeNode.taskConfigModules;
            var remainTime = (( te.WasterTime() + beginTime) - Time.time);
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
        // return exeNode.WhenCardChange(this);
        if (exeNode.taskConfigModules is TaskConfig_OnChange)
        {
            var change = (TaskConfig_OnChange)exeNode.taskConfigModules;
            if (change.WhenCardChange(this))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 点击继续的时候检测状态变化
    /// </summary>
    /// <returns></returns>
    public bool CanClickSwitch()
    {
        if (exeNode.taskConfigModules is TaskConfig_OnSucc)
        {
            var cfg = (TaskConfig_OnSucc)exeNode.taskConfigModules;
            if (cfg.CanClickChange(this))
            {
                cfg.ClickChange(this);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void SetExeNode(TaskConfig exeNode)
    {
        this.exeNode = exeNode;
        beginTime = Time.time;
        textList.Clear();
    }
    public float GetRemainTime()
    {
        return ((((TaskConfig_TimeOut)(exeNode.taskConfigModules)).WasterTime() + beginTime) - Time.time);
    }
    public void TryReleaseCard(string cardRequire)
    {
        var card = TryFindCard(cardRequire);
        if (card!=null)
        {
            var cardViews = GameFrameWork.Instance.viewModelManager.FindViews(card);
            if (cardViews!=null)
            {
                foreach (var val in cardViews)
                {
                    ((CardView)val).onGrab();
                    GameObject.Destroy(((CardView)val).gameObject);
                }
                GameFrameWork.Instance.viewModelManager.ReleaseModel(card);
                GameFrameWork.Instance.AddCardByCardModel(card, new Vector3(0, 0, 0));
            }
        }
    }
    /// <summary>
    /// 结束任务
    /// </summary>
    public void EndTask()
    {
        OnEndTask();
        GameFrameWork.Instance.taskManager.RemoveTask(this);
    }

    public void OnEndTask()
    {
        
    }

    public void OnSlotTouch(string slotName,Action<CardModel> action)
    {
        exeNode.taskConfigModules.OnSlotTouch(this,slotName,action);
    }
}
