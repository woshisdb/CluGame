using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeNode : WasterTimeExeNode
{
    public TestTimeNode() : base("WasterTime", "!!!!!!!!!")
    {
    }

    //public override bool CanProcess(TaskPanelModel task)
    //{
    //    return (Time.time-task.beginTime)>GetTime();
    //}

    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e2;
    }

    public override float GetTime()
    {
        return 5;
    }

    public override bool TimeSwitch(TaskPanelModel model)
    {
        var canSwitch = (Time.time - model.beginTime) > GetTime();
        if(canSwitch)
        {
            model.SetExeNode(new TestFinishNode());
        }
        return canSwitch;
    }

    public override bool WhenCardChange(TaskPanelModel task)
    {
        return false;
    }

    //public override void Process(TaskPanelModel task)
    //{
    //    task.SetExeNode(new TestFinishNode());
    //}
}
