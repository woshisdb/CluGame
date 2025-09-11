using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCardData : CardData, IStateFlag
{
    public ClimbCardData() : base()
    {
        title = "攀爬";
        description = "攀爬垂直表面（如墙壁、悬崖、树木），维持平衡避免坠落，在探索地形复杂的区域时必不可少。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ClimbCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.climb;
    }
}



public class ClimbCardModel : CardModel
{
    public ClimbCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}