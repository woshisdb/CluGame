using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimidateCardData : CardData, IStateFlag
{
    public IntimidateCardData() : base()
    {
        title = "恐吓";
        description = "通过威胁、威慑迫使他人服从，可获取信息或让目标退缩，但可能引发对方的敌意或反抗。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new IntimidateCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.intimidate;
    }
}



public class IntimidateCardModel : CardModel
{
    public IntimidateCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}