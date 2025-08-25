using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestCardData : CardData
{
    public override CardModel CreateModel()
    {
        return new TestCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.money;
    }
}

public class TestCardModel : CardModel
{
    public TestCardModel(CardData cardData) : base(cardData)
    {

    }
}