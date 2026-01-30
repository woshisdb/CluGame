using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using TMPro;
using UnityEngine;

public class TaskPanelView : SerializedMonoBehaviour, IView
{
    public TextMeshPro title;
    public TextMeshProUGUI description;
    public TaskPanelModel taskPanelModel;
    public TaskConfig taskConfig
    {
        get { return taskPanelModel.exeNode; }
    }
    public Dictionary<Slot, string> slotMap;
    public Transform slotRoot;
    public Transform cardRoot;
    public TextMeshPro timeRemain;
    [Header("放置的位置")]
    public float yPlacePos;
    public void BindModel(IModel model)
    {
        this.taskPanelModel = (TaskPanelModel)model;
        GameFrameWork.Instance.viewModelManager.Bind(model, this);
        Refresh();
    }

    public void onCloseClick()
    {
        gameObject.SetActive(false);
        GameFrameWork.Instance.viewModelManager.ReleaseView(this);
        this.taskPanelModel = null;
    }

    public void onContinueClick()
    {
        if(taskPanelModel.CanClickSwitch())
        {
            GameFrameWork.Instance.viewModelManager.RefreshView(taskPanelModel);
        }
        else
        {
            return;
        }
    }
    public void OnTaskPanelPut()
    {
        Debug.Log("OnTaskPanelPut");
        this.transform.localPosition = new Vector3(this.transform.position.x,yPlacePos, this.transform.position.z);
    }
    public IModel GetModel()
    {
        return taskPanelModel;
    }

    public void RefreshText()
    {
        if (taskPanelModel.textList.ContainsKey(ChangeTextType.title))
        {
            this.title.text = taskPanelModel.textList[ChangeTextType.title];
        }
        else
        {
            this.title.text = taskPanelModel.title;
        }

        if (taskPanelModel.textList.ContainsKey(ChangeTextType.description))
        {
            this.description.text = taskPanelModel.textList[ChangeTextType.description];
        }
        else
        {
            this.description.text = taskPanelModel.description;
        }
    }
    /// <summary>
    /// 刷新面板信息
    /// </summary>
    /// <returns></returns>
    public void Refresh()
    {
        RefreshText();
        slotRoot.gameObject.SetActive(false);
        cardRoot.gameObject.SetActive(false);
        timeRemain.gameObject.SetActive(false);
        if (taskConfig.taskConfigModules is TaskConfig_OnSucc)//如果是选择卡片节点
        {
            slotRoot.gameObject.SetActive(true);
            foreach (var x in slotMap)
            {
                var hasCard = x.Key.HasCard;
                if(hasCard)
                GameObject.Destroy(x.Key.cardView.gameObject);
                GameObject.Destroy(x.Key.gameObject);
            }

            var module = (TaskConfig_OnSucc)taskConfig.taskConfigModules;
            slotMap.Clear();
            int no = 0;
            foreach(var x in module.NeedCards())
            {
                var slotTemplate = GameFrameWork.Instance.gameConfig.slotTemplate;
                var obj = GameObject.Instantiate(slotTemplate);
                slotMap.Add(obj.GetComponent<Slot>(), x.Key);
                obj.transform.SetParent(slotRoot);
                obj.GetComponent<Slot>().Pos(no % 3, no / 3);
                obj.GetComponent<Slot>().name.text = x.Key;
                obj.GetComponent<Slot>().taskPanelView = this;
                obj.GetComponent<Slot>().isInit = true;
                if (taskPanelModel.cardsMap.ContainsKey(x.Key))//包含这个卡的内容
                {
                    var card = GameFrameWork.Instance.AddCardByCardModel(taskPanelModel.cardsMap[x.Key],new Vector3(0,0,0), true);
                    //obj.GetComponent<Slot>().OnCardTryPlaced(new OnCardPlaceArgs
                    //{
                    //    Card = card.GetComponent<CardView>()
                    //}) ;
                    obj.GetComponent<CardSlot>().TryPlaceCard(card.GetComponent<DraggableCard>());
                }
                obj.GetComponent<Slot>().isInit = false;
                no++;
            }
        }
        else if (taskPanelModel.exeNode is TaskConfig_TimeOut )//花费时间的节点
        {
            timeRemain.gameObject.SetActive(true);
            timeRemain.text = taskPanelModel.GetRemainTime().ToString();
        }
        else//如果是结束节点
        {
            cardRoot.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 进行状态转移
    /// </summary>
    /// <returns></returns>
    //public void StateTransition()
    //{
    //    var hasSwitch = taskPanelModel.CheckTimeSwitch();
    //    if (hasSwitch)
    //    {
    //        GameFrameWork.Instance.viewModelManager.RefreshView(taskPanelModel);
    //    }
    //}

    public bool CanAddCard(Slot slot, CardModel cardModel)
    {
        Debug.Log(slot.gameObject.name);
        if(slotMap.ContainsKey(slot))
        {
            var require = slotMap[slot];
            if (taskConfig.taskConfigModules is TaskConfig_OnSucc)
            {
                var cfg = (TaskConfig_OnSucc)taskConfig.taskConfigModules;
                return cfg.CanPut(require, cardModel);
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    /// <summary>
    /// 添加卡片
    /// </summary>
    public void OnAddCard(Slot slot,CardModel cardModel)
    {
        var cardRequire = slotMap[slot];
        taskPanelModel.AddCard(cardRequire,cardModel);
    }
    /// <summary>
    /// 删除卡片
    /// </summary>
    public void OnRemoveCard(Slot slot, CardModel cardModel)
    {
        var cardRequire = slotMap[slot];
        taskPanelModel.RemoveCard(cardRequire);
    }
    
    public void OnSlotTouch(Slot slot)
    {
        // slotMap[slot]
        taskPanelModel.OnSlotTouch(slotMap[slot], (cardModel) =>
        {
            slot.TryAddCardByCardModel(cardModel);
        });
    }
    public void Release()
    {
    }
}
