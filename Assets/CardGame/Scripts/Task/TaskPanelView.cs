using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    public void BindModel(IModel model)
    {
        this.taskPanelModel = (TaskPanelModel)model;
        Refresh();
    }

    public void onCloseClick()
    {
        gameObject.SetActive(false);
    }

    public void onContinueClick()
    {
        StateTransition();
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
            slotRoot.setEnable(true);
            cardRoot.setEnable(false);
            foreach (var x in slotMap)
            {
                GameObject.Destroy(x.Key);
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
            }
        }
        else if (taskPanelModel.exeNode.GetExeType() == ExeType.WasterTime)//花费时间的节点
        {
            slotRoot.setEnable(false);
            cardRoot.setEnable(false);
        }
        else//如果是结束节点
        {
            slotRoot.setEnable(false);
            cardRoot.setEnable(true);
        }
    }
    /// <summary>
    /// 进行状态转移
    /// </summary>
    /// <returns></returns>
    public void StateTransition()
    {
        var hasSwitch = taskPanelModel.Switch();
        if (hasSwitch)
        {
            Refresh();//刷新当前的状态
        }
    }

    public bool CanAddCard(Slot slot, CardModel cardModel)
    {
        var require = slotMap[slot];
        return require.Require(card);
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

}
