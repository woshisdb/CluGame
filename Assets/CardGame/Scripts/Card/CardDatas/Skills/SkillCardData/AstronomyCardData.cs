using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronomyCardData : SkillCardData, IStateFlag
{
    public AstronomyCardData() : base()
    {
        title = "天文学";
        description = "观测星体位置、识别星座，用于导航（尤其在野外或海上）、判断时间，或解读与天体相关的神话。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new AstronomyCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.astronomy;
    }
}



public class AstronomyCardModel : CardModel
{
    public AstronomyCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}