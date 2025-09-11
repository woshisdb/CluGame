using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychologyCardData : CardData, IStateFlag
{
    public PsychologyCardData() : base()
    {
        title = "心理学";
        description = "分析他人的心理状态、行为动机，判断对方是否说谎或隐藏意图，通常由守秘人进行暗骰判定。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new PsychologyCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.psychology;
    }
}



public class PsychologyCardModel : CardModel
{
    public PsychologyCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}