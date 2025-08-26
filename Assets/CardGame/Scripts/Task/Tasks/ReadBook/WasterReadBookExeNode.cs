using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 需要花费的时间
/// </summary>
public class WasterReadBookExeNode : WasterTimeExeNode
{
    public WasterReadBookExeNode() : base("正在阅读知识", "知识不断积累")
    {
    }

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
}