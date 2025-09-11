using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeCardData : CardData, IStateFlag
{
    public DodgeCardData() : base()
    {
        title = "闪避";
        description = "基础值为敏捷的一半，用于躲避近战攻击、陷阱或非弹道类危险，无法直接躲避子弹但可降低被击中概率。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new DodgeCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.dodge;
    }
}



public class DodgeCardModel : CardModel
{
    public DodgeCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}