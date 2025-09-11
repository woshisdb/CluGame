using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotHiddenCardData : CardData, IStateFlag
{
    public SpotHiddenCardData() : base()
    {
        title = "侦查";
        description = "用于发现隐藏的线索、密门、伪装的物品或潜伏的生物，通过视觉、嗅觉等感官察觉环境中的异常细节。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new SpotHiddenCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.spotHidden;
    }
}



public class SpotHiddenCardModel : CardModel
{
    public SpotHiddenCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}