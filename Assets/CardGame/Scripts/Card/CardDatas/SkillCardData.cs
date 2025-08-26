using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillCardData : CardData,ISkillFlag
{
    public SkillCardData():base()
    {
        title = "技能";
        description = "技能!";
        InitCardFlags(typeof(SkillCardData));
    }
    public int CardLevel(CardModel card)
    {
        return 1;
    }

    public override CardModel CreateModel()
    {
        return new SkillCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.money;
    }

    public void TryUp(CardModel card)
    {

    }
}

public class SkillCardModel : CardModel
{
    public SkillCardModel(CardData cardData) : base(cardData)
    {

    }
}