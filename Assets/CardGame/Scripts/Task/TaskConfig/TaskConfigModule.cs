using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务的配置
/// </summary>
public class TaskConfigModule
{
    public string title;
    public string description;
}
/// <summary>
/// 任务成功，允许点击
/// </summary>
public interface TaskConfig_OnSucc
{
    bool CanClickChange(TaskPanelModel task);
    void ClickChange(TaskPanelModel task);
    NextConfigModule getSucc();
    bool CanPut(string req,CardModel card);
    Dictionary<string, CardConfig> NeedCards();
}
/// <summary>
/// 时间超时
/// </summary>
public interface TaskConfig_TimeOut
{
    int WasterTime();
    
    void TimeSwitch(TaskPanelModel task);
}
/// <summary>
/// 任务选择卡片
/// </summary>
public interface TaskConfig_OnChange
{
    /// <summary>
    /// 当卡片切换的时候是否状态切换
    /// </summary>
    /// <returns></returns>
    bool WhenCardChange(TaskPanelModel task);
    NextConfigModule getChange();
}

/// <summary>
/// 花费时间配置
/// </summary>
public class WasterTime_TaskConfigModule:TaskConfigModule,TaskConfig_TimeOut
{
    /// <summary>
    /// 时间配置
    /// </summary>
    public int wasterTime;

    public int WasterTime()
    {
        return wasterTime;
    }
    /// <summary>
    /// 当时间到了之后
    /// </summary>
    public NextConfigModule onTimeOut;
    public WasterTime_TaskConfigModule()
    {
        onTimeOut = new NextConfigModule();
    }

    public NextConfigModule getTimeOut()
    {
        return onTimeOut;
    }

    public void TimeSwitch(TaskPanelModel task)
    {
        
    }
}
/// <summary>
/// 带有时间限制的需要卡片
/// </summary>
public class Limited_TaskConfigModule : WasterTime_TaskConfigModule, TaskConfig_OnChange,TaskConfig_OnSucc
{
    /// <summary>
    /// 需要的卡片
    /// </summary>
    public Dictionary<string, CardConfig> needCards;
    /// <summary>
    /// 点击继续后的行为
    /// </summary>
    public NextConfigModule onSucc;
    /// <summary>
    /// 状态切换后的行为
    /// </summary>
    public NextConfigModule onChange;
    public Limited_TaskConfigModule():base()
    {
        needCards = new Dictionary<string, CardConfig>();
        onSucc = new NextConfigModule();
    }

    public NextConfigModule getSucc()
    {
        return onSucc;
    }

    public bool CanPut(string req, CardModel card)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, CardConfig> NeedCards()
    {
        return needCards;
    }

    public bool CanClickChange(TaskPanelModel task)
    {
        throw new System.NotImplementedException();
    }

    public void ClickChange(TaskPanelModel task)
    {
        throw new System.NotImplementedException();
    }

    public NextConfigModule getChange()
    {
        return onChange;
    }

    public bool WhenCardChange(TaskPanelModel task)
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// 需要卡片,没有时间需求
/// </summary>
public class NeedCard_TaskConfigModule:TaskConfigModule, TaskConfig_OnChange, TaskConfig_OnSucc
{
    /// <summary>
    /// 有哪些卡需要转移
    /// </summary>
    public Dictionary<string, CardConfig> needCards;
    /// <summary>
    /// 点击继续后的行为
    /// </summary>
    public NextConfigModule onSucc;
    /// <summary>
    /// 状态切换后的行为
    /// </summary>
    public NextConfigModule onChange;
    public NeedCard_TaskConfigModule()
    {
        needCards = new Dictionary<string, CardConfig>();
        onSucc = new NextConfigModule();
    }
    public NextConfigModule getSucc()
    {
        return onSucc;
    }

    public bool CanPut(string req, CardModel card)
    {
        if (needCards.ContainsKey(req))
        {
            return needCards[req].isSat(card);
        }
        else
        {
            return false;
        }
    }

    public Dictionary<string, CardConfig> NeedCards()
    {
        return needCards;
    }

    public NextConfigModule getChange()
    {
        return onChange;
    }
    public NextConfig isSat(TaskPanelModel task,NextConfigModule next)
    {
        foreach (var cfg in next.config )
        {
            bool cfgIsSat = true;
            foreach (var req in cfg.requires)
            {
                if (req.sat)
                {
                    if (isSlotSat(req.name,task))
                    {
                        //成功
                    }
                    else
                    {
                        cfgIsSat = false;
                        //失败
                        break;
                    }
                }
                else
                {
                    if (!isSlotSat(req.name,task))
                    {
                        //成功
                    }
                    else
                    {
                        cfgIsSat = false;
                        //失败
                        break;
                    }
                }
            }
            if (cfgIsSat)
            {
                return cfg;
            }
        }
        return null;
    }
    public bool isSlotSat(string req,TaskPanelModel task)
    {
        var x = task.TryFindCard(req);
        if (x==null)
        {
            return false;
        }
        return true;
    }

    public virtual bool CanClickChange(TaskPanelModel task)
    {
        var ret = this.isSat(task, this.onSucc);
        if (ret!=null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 任务改变
    /// </summary>
    /// <param name="task"></param>
    public virtual void ClickChange(TaskPanelModel task)
    {
        var module = this;
        var ret = module.isSat(task, module.onSucc);
        ret.taskEffect.OnEffect(task);
    }
    /// <summary>
    /// 当卡片切换的时候是否状态切换
    /// </summary>
    /// <returns></returns>
    public virtual bool WhenCardChange(TaskPanelModel task)
    {
        var module = this;
        var ret = module.isSat(task, module.onChange);
        if (ret!=null)
        {
            ret.taskEffect.OnEffect(task);
            return true;
        }
        else
        {
            return false;
        }
    }
}

