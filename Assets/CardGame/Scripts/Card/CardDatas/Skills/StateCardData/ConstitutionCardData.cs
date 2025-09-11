using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstitutionCardData : CardData, IStateFlag
{
    public ConstitutionCardData() : base()
    {
        title = "体质";
        description = "人的身体素质";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ConstitutionCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.constitution;
    }
}



public class ConstitutionCardModel : CardModel
{
    public ConstitutionCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}