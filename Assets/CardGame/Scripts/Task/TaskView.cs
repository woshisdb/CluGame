using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TaskView : SerializedMonoBehaviour,IView
{
    public TextMeshPro text;
    public Sprite Sprite;
    public TextMeshPro endTime;
    [SerializeField]
    public TaskPanelModel model;
    public void BindModel(IModel model)
    {
        this.model = (TaskPanelModel)model;
        this.onBindView();
        this.Refresh();
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
        text.text = model.title;
        endTime.gameObject.SetActive(false);
        if(model.exeNode.GetExeType() == ExeType.WasterTime)
        {
            endTime.gameObject.SetActive(true);
            endTime.text = model.GetRemainTime().ToString();
        }
        else if(model.exeNode.GetExeType() == ExeType.Select)
        {
            
        }
        else
        {
            
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
            GameFrameWork.Instance.AddCardByCardModel(card,new Vector3(0,0,0));
        }
    }

    public void Release()
    {
    }
    public void OnDestroy()
    {
        this.OnDestroyView();
    }
}
