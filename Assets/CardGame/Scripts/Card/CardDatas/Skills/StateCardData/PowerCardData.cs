using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCardData : CardData, IStateFlag
{
    public PowerCardData() : base()
    {
        title = "意志";
        description = "你的意志水平";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new PowerCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.power;
    }
}



public class PowerCardModel : CardModel
{
    public PowerCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}