using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipMgr : MonoBehaviour
{
    /// <summary>
    /// 所做的事情的映射表
    /// </summary>
    public Dictionary<IDoThingTarget,DoThingInfo> doThingMap;
}
