using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocksmithCardData : CardData, IStateFlag
{
    public LocksmithCardData() : base()
    {
        title = "锁匠";
        description = "破解各类锁具、制作简易钥匙，或给锁具设置陷阱，在潜入密室、打开封锁区域时非常实用。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new LocksmithCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.locksmith;
    }
}



public class LocksmithCardModel : CardModel
{
    public LocksmithCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}