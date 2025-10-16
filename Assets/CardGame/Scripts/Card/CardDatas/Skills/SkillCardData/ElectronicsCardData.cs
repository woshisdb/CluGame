using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicsCardData : SkillCardData, IStateFlag
{
    public ElectronicsCardData() : base()
    {
        title = "电子学";
        description = "维修复杂电子设备，破解电子系统，理解无线电、计算机等现代电子设备的原理与操作（现代背景）。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new ElectronicsCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.electronics;
    }
}



public class ElectronicsCardModel : CardModel
{
    public ElectronicsCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}