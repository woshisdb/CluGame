using System;
using System.Collections.Generic;

public static class GameTimeUnit
{
    public const long Minute = 1;
    public const long Hour   = 60;
    public const long Day    = 1440;
}


public class GameTimeManager
{
    public List<WaitTimeNode> WaitTimeNodes
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.WaitTimeNodes;
        }
    }
    private static readonly DateTime StartTime =
        new DateTime(1920, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public long CurrentTime {
        get
        {
            return GameFrameWork.Instance.data.saveFile.NowTime;
        }
        set
        {
            GameFrameWork.Instance.data.saveFile.NowTime = value;
        }
    }

    public void Advance(long delta)
    {
        CurrentTime += delta;
    }

    public int GetNextTime(int time)
    {
        return (int)(CurrentTime + time);
    }
    public static DateTime ToDateTime(long time)
    {
        return StartTime.AddMinutes(time);
    }

    public string GetTimeStr()
    {
        var str = ToDateTime(CurrentTime).ToString("yyyy-MM-dd-HH:mm:ss");
        return str;
    }
    public void Init()
    {
    }
    /// <summary>
    /// 几个回合之后执行
    /// </summary>
    public void AddTimeNode(WaitTimeNode WaitTimeNode)
    {
        this.WaitTimeNodes.Add(WaitTimeNode);
    }

    public void RemoveTimeNode(WaitTimeNode WaitTimeNode)
    {
        this.WaitTimeNodes.Remove(WaitTimeNode);
    }

    public void UpdateTimeNode()
    {
        WaitTimeNodes.RemoveAll(e =>
        {
            if (e.time <= CurrentTime+1)
            {
                e.Action?.Invoke();
            }
            return e.time <= CurrentTime+1;
        });
    }
}