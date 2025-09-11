using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceCardData : CardData, IStateFlag
{
    public PerformanceCardData() : base()
    {
        title = "表演";
        description = "通过戏剧、音乐或其他表演形式吸引注意力、传递信息或掩饰真实目的，可用于社交场合或伪装。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new PerformanceCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.performance;
    }
}



public class PerformanceCardModel : CardModel
{
    public PerformanceCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}