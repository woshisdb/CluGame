using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCardData : SkillCardData, IStateFlag
{
    public TrackCardData() : base()
    {
        title = "追踪";
        description = "通过足迹、痕迹或环境变化追踪目标行踪，判断目标数量、行进方向及停留时间，适用于野外或城市。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new TrackCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.track;
    }
}



public class TrackCardModel : CardModel
{
    public TrackCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}