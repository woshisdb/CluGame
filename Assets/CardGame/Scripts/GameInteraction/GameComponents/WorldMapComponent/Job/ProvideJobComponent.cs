using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;


/// <summary>
/// 提供职业的，例如警局
/// </summary>
public class ProvideJobComponent:BaseComponent,ISupply<NeedJobComponent>
{
    public List<ProvideJobItem> ProvideJobItem;
    public SupplyModule<NeedJobComponent> Supply;
    /// <summary>
    /// 请求的工作
    /// </summary>
    public List<NeedJobComponent> requestJobs;
    public ProvideJobComponent(CardModel cardModel, ProvideJobComponentCreator creator) : base(cardModel, creator)
    {
        requestJobs = new List<NeedJobComponent>();
        ProvideJobItem = new List<ProvideJobItem>();
        foreach (var x in creator.ProvideJobItems)
        {
            ProvideJobItem.Add(x.Create(this));
        }

        Supply = new(cardModel,999, e =>
        {
            if (e.HasJob()&&ProvideJobItem.Contains(e.ProvideJobItem))
            {
                return true;
            }
            return false;
        }, (e) =>
        {
            
        }, (e) =>
        {
            
        });
    }
    /// <summary>
    /// 请求工作
    /// </summary>
    /// <param name="req"></param>
    public void RequestJob(NeedJobComponent req)
    {
        requestJobs.Add(req);
    }
    
    public bool HasEmptyJob(NeedJobComponent need)
    {
        foreach (var x in ProvideJobItem)
        {
            if (x.CanGetJob(need))
            {
                return true;
            }
        }

        return false;
    }

    public SupplyModule<NeedJobComponent> GetSupply()
    {
        return Supply;
    }
}

public class ProvideJobItem
{
    public ProvideJobComponent Component;
    public int Sum;

    public int StartTime
    {
        get
        {
            return JobObjectConfig.WeeklyWorkTimeConfig.DailyHours.StartHours;
        }
    }

    public int EndTime
    {
        get
        {
            return JobObjectConfig.WeeklyWorkTimeConfig.DailyHours.EndHours;
        }
    }
    public JobObjectConfig JobObjectConfig;
    public string JobName
    {
        get
        {
            return JobObjectConfig.Name;
        }
    }
    public List<NeedJobComponent> NeedJobs;
    /// <summary>
    /// 当前的工人
    /// </summary>
    public List<NeedJobComponent> nowWorkers;
    public ProvideJobItem(ProvideJobItemCreator creator,ProvideJobComponent Component)
    {
        this.Component = Component;
        JobObjectConfig = creator.JobObjectConfig;
        NeedJobs = new List<NeedJobComponent>();
        nowWorkers = new List<NeedJobComponent>();
        Sum = creator.Sum;
    }

    public void GetJob(NeedJobComponent need)
    {
        NeedJobs.Add(need);
        need.GetJob(this);
    }

    public void ReleaseJob(NeedJobComponent need)
    {
        NeedJobs.Remove(need);
        need.ReleaseJob();
    }

    public bool CanGetJob(NeedJobComponent need)
    {
        return NeedJobs.Count < Sum;
    }

    public void StartWork(NeedJobComponent need)
    {
        nowWorkers.Add(need);
    }

    public void EndWork(NeedJobComponent need)
    {
        nowWorkers.Remove(need);
    }
}

public class ProvideJobItemCreator
{
    public JobObjectConfig JobObjectConfig;
    public int Sum;

    public ProvideJobItem Create(ProvideJobComponent cmp)
    {
        return new ProvideJobItem(this,cmp);
    }
}

public class ProvideJobComponentCreator:BaseComponentCreator<ProvideJobComponent>
{
    public List<ProvideJobItemCreator> ProvideJobItems;
    public override ComponentType ComponentName()
    {
        return ComponentType.ProvideJobComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ProvideJobComponent(cardModel, this);
    }
}