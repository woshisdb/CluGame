using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineCardData : SkillCardData, IStateFlag
{
    public MedicineCardData() : base()
    {
        title = "医学";
        description = "专业治疗伤病，花费 1 小时以上可恢复 1D3 生命值，每人仅能接受一次医学治疗（与急救不冲突）。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new MedicineCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.medicine;
    }
}



public class MedicineCardModel : CardModel
{
    public MedicineCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}