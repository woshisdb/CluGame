using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Sleep,
}

public abstract class CardRequire
{
    /// <summary>
    /// 需求卡牌名字
    /// </summary>
    public string name;
    public CardRequire(string name)
    {
        this.name = name;
    }

    public abstract bool Require(CardModel card);
}
/// <summary>
/// 执行的枚举
/// </summary>
public enum ExeEnum
{
    e1,
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


