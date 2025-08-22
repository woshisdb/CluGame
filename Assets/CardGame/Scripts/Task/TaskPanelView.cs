using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TaskPanelView : SerializedMonoBehaviour,IView
{
    public TextMeshPro title;
    public TextMeshProUGUI description;
    public TaskPanelModel taskPanelModel;
    public Dictionary<CardRequire, Slot> slotMap;
    public Transform slotRoot;
    public void BindModel(IModel model)
    {
        this.taskPanelModel = (TaskPanelModel)model;
        Refresh();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Continue()
    {

    }

    public IModel GetModel()
    {
        return taskPanelModel;
    }

    public void Refresh()
    {
        this.title.text = taskPanelModel.title;
        this.description.text = taskPanelModel.description;
        if (taskPanelModel.exeNode.GetExeType() == ExeType.Select)
        {
            foreach(var x in slotMap)
            {
                GameObject.Destroy(x.Value);
            }
            slotMap.Clear();
            for(int no=0;no< ((SelectExeNode)taskPanelModel.exeNode).cardRequires.Count;no++)
            //foreach (var x in ((SelectExeNode)taskPanelModel.exeNode).cardRequires)
            {
                var x = ((SelectExeNode)taskPanelModel.exeNode).cardRequires[no];
                var slotTemplate = GameFrameWork.Instance.gameConfig.slotTemplate;
                var obj = GameObject.Instantiate(slotTemplate);
                slotMap.Add(x,obj.GetComponent<Slot>());
                obj.transform.SetParent(slotRoot);
                obj.GetComponent<Slot>().Pos(no%3,no/3);
            }
        }
    }
    /// <summary>
    /// 添加卡片
    /// </summary>
    public void OnAddCard(CardRequire cardRequire,CardModel cardModel)
    {
        taskPanelModel.AddCard(cardRequire,cardModel);
    }
    /// <summary>
    /// 删除卡片
    /// </summary>
    public void OnRemoveCard(CardRequire cardRequire, CardModel cardModel)
    {
        taskPanelModel.RemoveCard(cardRequire);
    }

}
