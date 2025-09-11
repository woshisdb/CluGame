using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthulhuMythosCardData : CardData, IStateFlag
{
    public CthulhuMythosCardData() : base()
    {
        title = "克苏鲁神话";
        description = "认知古神、外神、神话生物及禁忌知识，可回忆魔法咒语或解读神话文献，但会降低理智值上限。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new CthulhuMythosCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.cthulhuMythos;
    }
}



public class CthulhuMythosCardModel : CardModel
{
    public CthulhuMythosCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}