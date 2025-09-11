using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthCardData : CardData, IStateFlag
{
    public StrengthCardData() : base()
    {
        title = "力量";
        description = "肉体的强度";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new StrengthCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.strength;
    }
}



public class StrengthCardModel : CardModel
{
    public StrengthCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}