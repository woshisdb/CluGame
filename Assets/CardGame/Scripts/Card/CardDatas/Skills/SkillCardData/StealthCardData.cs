using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthCardData : SkillCardData, IStateFlag
{
    public StealthCardData() : base()
    {
        title = "潜行";
        description = "隐藏行踪、悄声移动，避免被他人或生物发现，适用于潜入、跟踪或躲避追捕等场景。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new StealthCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.stealth;
    }
}



public class StealthCardModel : CardModel
{
    public StealthCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}