using System;

/// <summary>
/// 等待时间管理器
/// </summary>
public class WaitTimeNode
{
    public int time;
    public Action Action;

    public WaitTimeNode(int time,Action action)
    {
        this.time = GameFrameWork.Instance.GameTimeManager.GetNextTime(time);
        this.Action = action;
    }
}