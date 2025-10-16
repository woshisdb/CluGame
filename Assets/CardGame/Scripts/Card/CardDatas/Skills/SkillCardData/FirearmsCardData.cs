using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmsCardData : SkillCardData, IStateFlag
{
    public FirearmsCardData() : base()
    {
        title = "射击";
        description = "使用各类枪械（手枪、步枪、霰弹枪等）进行攻击，决定射击精度和命中率，是远程战斗的核心技能。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new FirearmsCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.firearms;
    }
}



public class FirearmsCardModel : CardModel
{
    public FirearmsCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}