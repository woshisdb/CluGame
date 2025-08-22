using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TaskView : SerializedMonoBehaviour,IView
{
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

    }
}
