using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoTargetType
{
    /// <summary>
    /// 具体的人
    /// </summary>
    Person,
    /// <summary>
    /// 圈子
    /// </summary>
    Circle,
}

public enum DoThingType
{

}

public interface IDoThingTarget
{

}

/// <summary>
/// 做的事情
/// </summary>
public abstract class DoThingInfo
{
    /// <summary>
    /// 对人做的事情
    /// </summary>
    /// <returns></returns>
    public abstract DoTargetType DoTargetType();
    /// <summary>
    /// 获取目标
    /// </summary>
    /// <returns></returns>
    public abstract IDoThingTarget GetTarget();
    /// <summary>
    /// 做所得事情
    /// </summary>
    /// <returns></returns>
    public abstract DoThingType GetThingType();

}

