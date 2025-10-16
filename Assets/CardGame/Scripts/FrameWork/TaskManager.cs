using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TaskManager
{
    /// <summary>
    /// 整个游戏的一系列任务
    /// </summary>
    public List<TaskPanelModel> tasks{
        get
        {
            return GameFrameWork.Instance.gameConfig.saveData.saveFile.tasks;
        }}
    
    public TaskManager()
    {
    }
    public void Init()
    {
        //var tv = GameObject.Find("Task").GetComponent<TaskView>();
        //tasks.Add(tv.model);
        int no = 0;
        foreach(var x in tasks)
        {
            GameFrameWork.Instance.AddTaskByData(x,new Vector3(no*3,0,4));
            no++;
        }
    }
    public void Update()
    {
        foreach (var task in tasks)
        {
            if (task.CheckTimeSwitch())
            {
                GameFrameWork.Instance.viewModelManager.RefreshView(task);
            }
        }
    }
}