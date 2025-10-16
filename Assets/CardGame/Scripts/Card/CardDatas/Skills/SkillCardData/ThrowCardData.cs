using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCardData : SkillCardData, IStateFlag
{
    public ThrowCardData() : base()
    {
        title = "投掷";
        description = "准确投掷武器（如飞刀、飞斧）或物品（如绳索、石块），决定投掷距离与精准度，可用于攻击或辅助。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ThrowCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.throwing;
    }
}



public class ThrowCardModel : CardModel
{
    public ThrowCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}