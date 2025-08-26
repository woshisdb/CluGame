using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoneyCardData : CardData,IMoneyFlag
{
    public MoneyCardData() : base()
    {
        title = "money";
        description = "money";
        InitCardFlags(typeof(IMoneyFlag));
    }
    public override CardModel CreateModel()
    {
        return new TestCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.money;
    }
}

public class MoneyCardModel : CardModel
{
    public MoneyCardModel(CardData cardData) : base(cardData)
    {

    }
}