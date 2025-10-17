using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceType
{
    space1
}

public class SpaceConfig
{
    /// <summary>
    /// 空间类型
    /// </summary>
    public SpaceType spaceType;
    /// <summary>
    /// 摄像机到达的位置
    /// </summary>
    public Transform pos;
}

public abstract class SpaceCardData : CardData
{
    public SpaceCardData() : base()
    {
        title = "房间卡";
        description = "房间卡牌,可以互动的房间";
        viewType = ViewType.SpaceCard;
    }

    public override CardModel CreateModel()
    {
        return new SpaceCardModel(this);
    }
    public virtual bool CanGo()
    {
        return false;
    }
    public virtual SpaceType GetSpace()
    {
        return SpaceType.space1;
    }
}



public class SpaceCardModel : CardModel,IHaveJob
{
    public SpaceCardModel(CardData cardData) : base(cardData)
    {
        SetDataByKey("Jobs",new List<JobCardModel>());
        // 模型初始化逻辑
    }
    public override List<UIItemBinder> GetUI()
    {
        var ret = new List<UIItemBinder>()
        {
        };
        if(((SpaceCardData)cardData).CanGo())
        {
            ret.Add(
            new ButtonBinder(() => { return "前往"; }, () =>
            {
                GameFrameWork.Instance.GoToSpace(((SpaceCardData)cardData).GetSpace());
            }));
        }

        var jobs = GetJobs();
        if (jobs!=null)
        {
            var jobui = new List<UIItemBinder>();
            foreach (var job in jobs)
            {
                jobui.Add(new ButtonBinder(() =>
                {
                    return job.GetJobInfo().jobName;
                }, () =>
                {
                    
                }));
            }
            ret.Add(new TableItemBinder(() => { return "职位";},jobui));
        }
        return ret;
    }

    public void AddJob(JobCardModel jobHandle)
    {
        var jobs =GetObjectByKey<List<JobCardModel>>("Jobs");
        if (jobs == null)
        {
            SetDataByKey("Jobs",new List<JobCardModel>());
        }
        jobs =GetObjectByKey<List<JobCardModel>>("Jobs");
        jobs.Add(jobHandle);
    }

    public List<JobCardModel> GetJobs()
    {
        return GetObjectByKey<List<JobCardModel>>("Jobs");
    }
}