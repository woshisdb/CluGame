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

    public TaskPanelModel AddTaskData(TaskConfig taskConfig)
    {
        var module = new TaskPanelModel();
        module.SetExeNode(taskConfig);
        return module;
    }

    public void RemoveTask(TaskPanelModel task)
    {
        tasks.Remove(task);
        var view = GameFrameWork.Instance.taskPanel.GetComponent<TaskPanelView>();
        if (view.taskPanelModel == task)
        {
            view.gameObject.SetActive(false);
        }
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

    public void StartTask(TaskConfig taskConfig)
    {
        var module = AddTaskData(taskConfig);
        Camera cam = Camera.main;
        // 屏幕中心的屏幕坐标
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        // 将其转换为世界坐标
        Vector3 worldPos = cam.ScreenToWorldPoint(screenCenter);
        worldPos.y = 10;
        GameFrameWork.Instance.OpenTaskPanel(module,worldPos);
    }
}