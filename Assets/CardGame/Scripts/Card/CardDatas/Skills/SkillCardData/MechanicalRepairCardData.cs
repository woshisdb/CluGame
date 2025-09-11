using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalRepairCardData : CardData, IStateFlag
{
    public MechanicalRepairCardData() : base()
    {
        title = "机械维修";
        description = "修理或制造非电气化机械装置，如引擎、锁具、简单工具等，在修复设备或制作简易装置时必不可少。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new MechanicalRepairCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.mechanicalRepair;
    }
}



public class MechanicalRepairCardModel : CardModel
{
    public MechanicalRepairCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}