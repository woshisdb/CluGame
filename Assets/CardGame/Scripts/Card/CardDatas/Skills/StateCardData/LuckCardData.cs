using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckCardData : CardData, IStateFlag
{
    public LuckCardData() : base()
    {
        title = "幸运";
        description = "你是否幸运";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new LuckCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.luck;
    }
}



public class LuckCardModel : CardModel
{
    public LuckCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}