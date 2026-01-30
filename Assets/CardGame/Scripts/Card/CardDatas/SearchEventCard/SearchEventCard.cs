using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 搜查事件卡，每回合会翻一张
/// </summary>
public class SearchEventCardData : CardData
{
    public override CardEnum GetCardType()
    {
        throw new System.NotImplementedException();
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        throw new System.NotImplementedException();
    }
}

public abstract class SearchEventCardModel : CardModel
{
    public SearchEventCardModel(CardData cardData) : base(cardData)
    {
        
    }
    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="done"></param>
    public abstract void OnEffect(Action done);
}