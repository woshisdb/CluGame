using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlCardData : SkillCardData, IStateFlag
{
    public BrawlCardData() : base()
    {
        title = "格斗";
        description = "徒手或使用简易武器（如棍棒、石块）进行近战，包括拳击、摔跤等基础搏斗技巧。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new BrawlCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.brawl;
    }
}



public class BrawlCardModel : CardModel
{
    public BrawlCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}