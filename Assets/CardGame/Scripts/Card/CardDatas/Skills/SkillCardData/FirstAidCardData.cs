using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidCardData : CardData, IStateFlag
{
    public FirstAidCardData() : base()
    {
        title = "急救";
        description = "受伤 1 小时内进行紧急处理，可恢复 1 点生命值；对濒死角色进行急救可使其状态稳定 1 小时。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new FirstAidCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.firstAid;
    }
}



public class FirstAidCardModel : CardModel
{
    public FirstAidCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}