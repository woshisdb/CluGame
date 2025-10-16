using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmCardData : SkillCardData, IStateFlag
{
    public CharmCardData() : base()
    {
        title = "魅惑";
        description = "通过魅力与亲和力赢得他人好感、建立信任，不同于交涉的理性说服，更依赖情感共鸣与个人吸引力。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new CharmCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.charm;
    }
}



public class CharmCardModel : CardModel
{
    public CharmCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}