using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelligenceCardData : CardData, IStateFlag
{
    public IntelligenceCardData() : base()
    {
        title = "智力";
        description = "你的智力水平";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new IntelligenceCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.intelligence;
    }
}



public class IntelligenceCardModel : CardModel
{
    public IntelligenceCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}