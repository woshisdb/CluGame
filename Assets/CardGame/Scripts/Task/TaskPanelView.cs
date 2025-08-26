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
    public Dictionary<Slot, CardRequire> slotMap;
    public Transform slotRoot;
    public Transform cardRoot;
    public TextMeshPro timeRemain;

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

    public IModel GetModel()
    {
        return taskPanelModel;
    }
    /// <summary>
    /// 刷新面板信息
    /// </summary>
    /// <returns></returns>
    public void Refresh()
    {
        this.title.text = taskPanelModel.title;
        this.description.text = taskPanelModel.description;
        if (taskPanelModel.exeNode.GetExeType() == ExeType.Select)//如果是选择卡片节点
        {
            slotRoot.gameObject.SetActive(true);
            cardRoot.gameObject.SetActive(false);
            timeRemain.gameObject.SetActive(false);
            foreach (var x in slotMap)
            {
                GameObject.Destroy(x.Key.gameObject);
            }

            slotMap.Clear();
            for (int no = 0; no < ((SelectExeNode)taskPanelModel.exeNode).cardRequires.Count; no++)
                //foreach (var x in ((SelectExeNode)taskPanelModel.exeNode).cardRequires)
            {
                var x = ((SelectExeNode)taskPanelModel.exeNode).cardRequires[no];
                var slotTemplate = GameFrameWork.Instance.gameConfig.slotTemplate;
                var obj = GameObject.Instantiate(slotTemplate);
                slotMap.Add(obj.GetComponent<Slot>(), x);
                obj.transform.SetParent(slotRoot);
                obj.GetComponent<Slot>().Pos(no % 3, no / 3);
                obj.GetComponent<Slot>().name.text = x.name;
                obj.GetComponent<Slot>().taskPanelView = this;
                obj.GetComponent<Slot>().isInit = true;
                if (taskPanelModel.cardsMap.ContainsKey(x))//包含这个卡的内容
                {
                    var card = GameFrameWork.Instance.AddCardByCardModel(taskPanelModel.cardsMap[x],new Vector3(0,0,0));
                    var dragMe = card.GetComponent<DragMe>();
                    obj.GetComponent<DragMe>().TryPlace(new Vector3(0,0,0),dragMe);
                }
                obj.GetComponent<Slot>().isInit = false;
            }
        }
        else if (taskPanelModel.exeNode.GetExeType() == ExeType.WasterTime)//花费时间的节点
        {
            slotRoot.gameObject.SetActive(false);
            cardRoot.gameObject.SetActive(false);
            timeRemain.gameObject.SetActive(true);
            timeRemain.text = taskPanelModel.GetRemainTime().ToString();
        }
        else//如果是结束节点
        {
            slotRoot.gameObject.SetActive(false);
            cardRoot.gameObject.SetActive(true);
            timeRemain.gameObject.SetActive(false);
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
            return require.Require(cardModel);
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
    public void Release()
    {
    }
}
