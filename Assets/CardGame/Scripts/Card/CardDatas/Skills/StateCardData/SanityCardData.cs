using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityCardData : CardData, IStateFlag
{
    public SanityCardData() : base()
    {
        title = "理智";
        description = "你的理智情况";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new SanityCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.sanity;
    }
}



public class SanityCardModel : CardModel
{
    public SanityCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}