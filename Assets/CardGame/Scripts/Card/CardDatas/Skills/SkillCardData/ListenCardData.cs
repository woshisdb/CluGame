using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenCardData : SkillCardData, IStateFlag
{
    public ListenCardData() : base()
    {
        title = "聆听";
        description = "辨识微弱或异常的声音，如远处的脚步声、隐藏的呼吸声、墙壁后的动静等，是重要的环境感知技能。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ListenCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.listen;
    }
}



public class ListenCardModel : CardModel
{
    public ListenCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}