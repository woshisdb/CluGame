using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultCardData : SkillCardData, IStateFlag
{
    public OccultCardData() : base()
    {
        title = "神秘学";
        description = "识别神秘符号、仪式道具和魔法书籍，了解民间传说、超自然现象和非神话体系的神秘知识。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new OccultCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.occult;
    }
}



public class OccultCardModel : CardModel
{
    public OccultCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}