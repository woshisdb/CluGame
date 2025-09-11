using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceCardData : CardData
{
    public SpaceCardData() : base()
    {
        title = "房间卡";
        description = "房间卡牌,可以互动的房间";
        viewType = ViewType.SpaceCard;
    }

    public override CardModel CreateModel()
    {
        return new SpaceCardModel(this);
    }

}



public class SpaceCardModel : CardModel
{
    public SpaceCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}