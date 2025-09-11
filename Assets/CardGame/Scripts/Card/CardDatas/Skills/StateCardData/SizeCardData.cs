using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCardData : CardData, IStateFlag
{
    public SizeCardData() : base()
    {
        title = "体质";
        description = "人的身体素质";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new SizeCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.size;
    }
}



public class SizeCardModel : CardModel
{
    public SizeCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}