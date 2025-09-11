using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexterityCardData : CardData, IStateFlag
{
    public DexterityCardData() : base()
    {
        title = "敏捷";
        description = "人的灵敏程度";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new DexterityCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.dexterity;
    }
}



public class DexterityCardModel : CardModel
{
    public DexterityCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}