using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchaeologyCardData : CardData, IStateFlag
{
    public ArchaeologyCardData() : base()
    {
        title = "考古学";
        description = "鉴定古董年代与真伪，发掘古代遗址，研究消亡文明的生活方式，辅助解读失传语言和历史谜团。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ArchaeologyCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.archaeology;
    }
}



public class ArchaeologyCardModel : CardModel
{
    public ArchaeologyCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}