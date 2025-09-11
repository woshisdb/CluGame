using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditRatingCardData : CardData, IStateFlag
{
    public CreditRatingCardData() : base()
    {
        title = "信用评级";
        description = "衡量社会地位、财富水平和信用度，决定可动用的资金与资源，可替代外貌影响第一印象或辅助交涉。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new CreditRatingCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.creditRating;
    }
}



public class CreditRatingCardModel : CardModel
{
    public CreditRatingCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}