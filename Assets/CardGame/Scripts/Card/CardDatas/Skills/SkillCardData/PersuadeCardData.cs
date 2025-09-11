using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuadeCardData : CardData, IStateFlag
{
    public PersuadeCardData() : base()
    {
        title = "交涉";
        description = "通过理性沟通说服他人，达成合作、获取信息或改变对方立场，是社交互动中的核心技能。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new PersuadeCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.persuade;
    }
}



public class PersuadeCardModel : CardModel
{
    public PersuadeCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}