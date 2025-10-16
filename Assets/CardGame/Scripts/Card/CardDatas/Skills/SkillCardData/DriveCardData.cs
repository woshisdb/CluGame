using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveCardData : SkillCardData, IStateFlag
{
    public DriveCardData() : base()
    {
        title = "驾驶";
        description = "操作特定交通工具（如汽车、飞机、船只），包括常规驾驶、紧急避险和简单的故障排除。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new DriveCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.drive;
    }
}



public class DriveCardModel : CardModel
{
    public DriveCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}