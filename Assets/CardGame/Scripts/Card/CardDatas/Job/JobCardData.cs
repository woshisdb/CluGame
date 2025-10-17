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
    public string ID;
    /// <summary>
    /// 工作的描述信息
    /// </summary>
    public JobInfo jobInfo;
    /// <summary>
    /// 工作的效果
    /// </summary>
    public List<IJobEffect> effects;

    public JobCardData() : base()
    {
        viewType = ViewType.JobCard;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.JobCard;
    }

    public override CardModel CreateModel()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 创建具体的工作
    /// </summary>
    /// <returns></returns>
    public CardModel CreateSpecialJob(IHaveJob haveJober)
    {
        return new JobCardModel(haveJober,this);
    }
}

/// <summary>
/// 工作
/// </summary>
public class JobCardModel : CardModel
{
    [SerializeField]
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.JobCardDatas[jobId];
        }
    }
    /// <summary>
    /// 工作提供者
    /// </summary>
    public IHaveJob jobProvider;
    /// <summary>
    /// 工作数目
    /// </summary>
    public int sum;
    /// <summary>
    /// 职工数
    /// </summary>
    public List<INeedJob> needJobs;

    public string jobId;
    public List<IJobEffect> jobEffects
    {
        get
        {
            return ((JobCardData)cardData).effects;
        }
    }

    public JobCardModel(IHaveJob jobProvider,CardData cardData) : base(cardData)
    {
        jobId = ((JobCardData)cardData).ID;
        this.jobProvider = jobProvider;
        needJobs = new List<INeedJob>();
        jobProvider.AddJob(this);
    }

    public JobInfo GetJobInfo()
    {
        return ((JobCardData)cardData).jobInfo;
    }
}
