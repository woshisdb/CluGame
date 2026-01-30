using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathCfg
{
    public CardEnum from;
    public CardEnum to;
    /// <summary>
    /// 是否是单向路径
    /// </summary>
    public bool isOneWay;
}

[CreateAssetMenu(fileName = "路径设置", menuName = "配置/路径配置")]
public class PathConfig : SerializedScriptableObject
{
    public List<PathCfg> PathCfgs;

    public PathConfig()
    {
        PathCfgs = new List<PathCfg>();
    }
}
