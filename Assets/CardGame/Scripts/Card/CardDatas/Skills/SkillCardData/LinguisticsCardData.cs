using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinguisticsCardData : CardData, IStateFlag
{
    public LinguisticsCardData() : base()
    {
        title = "语言学";
        description = "掌握多门语言，翻译文本（包括古代文字），分析语言特征推断说话者背景，默认包含母语能力。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new LinguisticsCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.linguistics;
    }
}



public class LinguisticsCardModel : CardModel
{
    public LinguisticsCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}