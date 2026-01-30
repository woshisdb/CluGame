using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckState
{
    Succ,
    Fail
}

public class CheckCardData : CardData
{
    public CheckCardData():base()
    {
        this.viewType = ViewType.SkillCard;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.Check;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new CheckCardModel(this);
    }
}

public class CheckCardModel : CardModel
{
    public (CardEnum, string) id;
    public CheckCardModel(CardData cardData) : base(cardData)
    {
        this.cardEnum = CardEnum.Check;
    }
    /// <summary>
    /// 监测卡片的效果
    /// </summary>
    /// <param name="done"></param>
    public void OnEffect(Action done)
    {
        done?.Invoke();
    }

    public void SetCard(CardEnum card,string dicId)
    {
        id = (card, dicId);
    }

    public override string GetTitle()
    {
        return GlobalCheckCardConfig.GetConfig(id.Item1,id.Item2).title;
    }

    public override string GetDescription()
    {
        return GlobalCheckCardConfig.GetConfig(id.Item1,id.Item2).description;
    }
}