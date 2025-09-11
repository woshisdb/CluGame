using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCardData : CardData, IStateFlag
{
    public HealthCardData() : base()
    {
        title = "生命值";
        description = "你的生命值";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new HealthCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.health;
    }
}



public class HealthCardModel : CardModel
{
    public HealthCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}