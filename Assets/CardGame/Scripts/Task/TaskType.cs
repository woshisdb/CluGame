using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Sleep,
}

public enum ExeType
{
    /// <summary>
    /// 可以直接获取卡片
    /// </summary>
    Finish,
    /// <summary>
    /// 卡片选择
    /// </summary>
    Select,
    /// <summary>
    /// 卡片花费时间
    /// </summary>
    WasterTime,
}


