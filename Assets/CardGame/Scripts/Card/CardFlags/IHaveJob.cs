using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;


/// <summary>
/// 提供职业的，例如警局
/// </summary>
public interface IHaveJob
{
    void AddJob(JobCardModel jobHandle);
    List<JobCardModel> GetJobs();
    
    
}
