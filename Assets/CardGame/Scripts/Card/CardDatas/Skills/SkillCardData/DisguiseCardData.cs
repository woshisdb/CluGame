using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisguiseCardData : SkillCardData, IStateFlag
{
    public DisguiseCardData() : base()
    {
        title = "乔装";
        description = "通过改变外貌、服饰或行为举止伪装成他人或特定身份，用于潜入、隐藏真实身份或误导他人。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new DisguiseCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.disguise;
    }
}



public class DisguiseCardModel : CardModel
{
    public DisguiseCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}