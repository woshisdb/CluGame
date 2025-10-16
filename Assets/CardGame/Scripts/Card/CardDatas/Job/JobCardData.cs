using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 工作的效果
/// </summary>
public interface IJobEffect
{
    
}

/// <summary>
/// 具体工作的信息，例如普通医生，院长
/// </summary>
public class JobCardData: CardData
{
    /// <summary>
    /// 工作的描述信息
    /// </summary>
    JobInfo jobInfo;
    /// <summary>
    /// 工作的效果
    /// </summary>
    public List<IJobEffect> effects;
    public override CardEnum GetCardType()
    {
        throw new System.NotImplementedException();
    }

    public override CardModel CreateModel()
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// 工作
/// </summary>
public class JobCardModel : CardModel
{
    /// <summary>
    /// 工作提供者
    /// </summary>
    public IHaveJob HaveJob;
    /// <summary>
    /// 工作数目
    /// </summary>
    public int sum;
    /// <summary>
    /// 职工数
    /// </summary>
    public List<INeedJob> needJobs;

    public List<IJobEffect> jobEffects
    {
        get
        {
            return ((JobCardData)cardData).effects;
        }
    }

    public JobCardModel(CardEnum cardEnum) : base(cardEnum)
    {
        needJobs = new List<INeedJob>();
    }

    public JobCardModel(CardData cardData) : base(cardData)
    {
        needJobs = new List<INeedJob>();
    }

    public virtual void Effect()
    {
        
    }
}
