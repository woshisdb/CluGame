using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTalkCardData : CardData, IStateFlag
{
    public FastTalkCardData() : base()
    {
        title = "快速交谈";
        description = "通过话术临时欺骗、转移话题或蒙混过关，适用于紧急情况或需要快速摆脱怀疑的场景。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new FastTalkCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.fastTalk;
    }
}



public class FastTalkCardModel : CardModel
{
    public FastTalkCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}