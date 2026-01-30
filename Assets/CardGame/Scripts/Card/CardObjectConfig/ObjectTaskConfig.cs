using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectTaskConfigItem
{
    public Dictionary<string, string> dic;
}

[CreateAssetMenu(fileName = "对象任务配置", menuName = "配置/对象任务配置")]
public class ObjectTaskConfig:SerializedScriptableObject
{
    public Dictionary<string, ObjectTaskConfigItem> objTaskCfg;
}
