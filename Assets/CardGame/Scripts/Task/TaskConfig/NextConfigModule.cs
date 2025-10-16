using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireCfg
{
    public string name;
    public bool sat;
    public RequireCfg()
    {
        sat = true;
    }
}

/// <summary>
/// 满足配置之后的处理
/// </summary>
public class NextConfig
{
    /// <summary>
    /// 要求的信息
    /// </summary>
    public List<RequireCfg> requires;
    public TaskEffectModule taskEffect;
    /// <summary>
    /// 下一个配置
    /// </summary>
    public NextConfig()
    {
        requires = new List<RequireCfg>();
        taskEffect = new TaskEffectModule();
    }
}
/// <summary>
/// 下一个回合配置
/// </summary>
public class NextConfigModule
{
    /// <summary>
    /// 下一配置
    /// </summary>
    public List<NextConfig> config;
    public NextConfigModule()
    {
        config = new List<NextConfig>();
    }
}