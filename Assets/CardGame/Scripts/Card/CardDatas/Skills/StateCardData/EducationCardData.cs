using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducationCardData : CardData, IStateFlag
{
    public EducationCardData() : base()
    {
        title = "教育";
        description = "你的受教育水平";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new EducationCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.education;
    }
}



public class EducationCardModel : CardModel
{
    public EducationCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}