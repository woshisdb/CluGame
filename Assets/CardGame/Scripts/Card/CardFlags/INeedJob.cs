using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 金钱的卡牌
/// </summary>
[InterfaceBind(CardFlag.needJob)]
public interface INeedJob : ICardFlag
{
    /// <summary>
    /// 工作的名字
    /// </summary>
    /// <returns></returns>
    string JobName();
    /// <summary>
    /// 工作的描述
    /// </summary>
    /// <returns></returns>
    string JobAim();
}
/// <summary>
/// 工作的目标
/// </summary>
public class JobAim
{
    
}
/// <summary>
/// 工作的得分目标
/// </summary>
public class JobScore
{
    
}