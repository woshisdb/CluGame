using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 工作信息
/// </summary>
public class JobInfo
{
    /// <summary>
    /// 工作的名字
    /// </summary>
    public string jobName;
    /// <summary>
    /// 工作的描述
    /// </summary>
    public string jobDescription;
    public string UpGrade;
}

public enum WorkAttitude {
    // 积极主动：主动承担任务，追求超额完成
    PROACTIVE,
    // 认真负责：按要求完成任务，注重结果质量
    RESPONSIBLE,
    // 敷衍应付：仅完成基础要求，不关注细节和质量
    PERFUNCTORY,
    // 消极抵触：对任务有抵触情绪，拖延或逃避
    NEGATIVE_RESISTANCE,
}

/// <summary>
/// 个人对这个工作的态度
/// </summary>
public class PersonJobInfo
{
    public WorkAttitude workAttitude;
    public JobInfo jobInfo;
}

public class JobRecordCardData : CardData
{
    public JobRecordCardData():base()
    {
        this.viewType = ViewType.JobRecordCard;
    }

    public override CardModel CreateModel()
    {
        throw new System.NotImplementedException();
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.JobRecord;
    }

    public CardModel CreateCardModelByJobInfo(NpcCardModel npcCardModel,JobInfo jobInfo)
    {
        return null;
    }
}


public class JobRecordCardModel : CardModel
{
    /// <summary>
    /// 要显示工作的信息
    /// </summary>
    public INeedJob needJober;
    public JobRecordCardModel(INeedJob needJob,CardData cardData) : base(cardData)
    {
        this.needJober = needJob;
    }
}