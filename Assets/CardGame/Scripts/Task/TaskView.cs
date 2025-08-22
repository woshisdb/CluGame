using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TaskView : SerializedMonoBehaviour,IView
{
    public TextMeshPro text;
    public Sprite Sprite;
    public TextMeshProUGUI endTime;
    [SerializeField]
    public TaskPanelModel model;
    public void BindModel(IModel model)
    {
        this.model = (TaskPanelModel)model;
    }

    public IModel GetModel()
    {
        return (IModel)model;
    }

    public void OnStartButtonClick()
    {
        GameFrameWork.Instance.taskPanel.SetActive(true);
        GameFrameWork.Instance.taskPanel.GetComponent<TaskPanelView>().BindModel(model);
    }

    public void Refresh()
    {
        endTime.enabled = (false);
        if(model.exeNode.GetExeType() == ExeType.WasterTime)
        {
            endTime.gameObject.SetActive(true);
        }
        else if(model.exeNode.GetExeType() == ExeType.Select)
        {
            
        }
        else
        {
            
        }
    }

    public void StateTransition()
    {
        var hasSwitch = model.Switch();
        if (hasSwitch)
        {
            Refresh();//刷新当前的状态
        }
    }
    /// <summary>
    /// 交出一系列的卡牌
    /// </summary>
    /// <returns></returns>
    public void ReturnCards(List<CardModel> cards)
    {
        foreach (var card in cards)
        {
            GameFrameWork.Instance.AddCardByCardModel(card);
        }
    }

    public void Release()
    {
        throw new System.NotImplementedException();
    }
}
