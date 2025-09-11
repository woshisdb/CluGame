using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceCardData : CardData, IStateFlag
{
    public AppearanceCardData() : base()
    {
        title = "外貌";
        description = "你的外表";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new AppearanceCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.appearance;
    }
}



public class AppearanceCardModel : CardModel
{
    public AppearanceCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}