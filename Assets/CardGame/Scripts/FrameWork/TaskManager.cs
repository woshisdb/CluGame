using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TaskManager
{
    /// <summary>
    /// 整个游戏的一系列任务
    /// </summary>
    public List<TaskPanelModel> tasks;
    
    public TaskManager()
    {
        tasks = new List<TaskPanelModel>();
    }

    public void Update()
    {
        foreach (var task in tasks)
        {
            if (task.hasSwitch())
            {
                GameFrameWork.Instance.viewModelManager.RefreshView(task);
            }
        }
    }
}