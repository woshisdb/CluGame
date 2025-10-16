using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalRepairCardData : SkillCardData, IStateFlag
{
    public ElectricalRepairCardData() : base()
    {
        title = "电气维修";
        description = "改装、维修电气设备，如电路、发电机、老式电器等，处理与电力相关的故障或设备调试。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ElectricalRepairCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.electricalRepair;
    }
}



public class ElectricalRepairCardModel : CardModel
{
    public ElectricalRepairCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}