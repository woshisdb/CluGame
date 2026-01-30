using System.Collections.Generic;
using UnityEngine;

public struct MonsterAttackCardCreateInfo : CardCreateInfo
{
    public MonsterAttackCfg MonsterAttackCfg;

    public CardData CardData
    {
        get
        {
            return this.GetCardData();
        }
    }
    public CardEnum Belong()
    {
        return CardEnum.MonsterAttack;
    }

    public MonsterAttackCardCreateInfo(MonsterAttackCfg cfg)
    {
        MonsterAttackCfg = cfg;
    }
}
public class MonsterAttackCardData:CardData,CardDataDic<MonsterAttackEnum,MonsterAttackCfg>
{
    public Dictionary<MonsterAttackEnum, MonsterAttackCfg> MonsterAttackCfgs;
    public MonsterAttackCardData():base()
    {
        viewType = ViewType.SkillCard;
        this.needRefresh = true;
        this.Init(ref MonsterAttackCfgs);
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterAttack;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new MonsterAttackCardModel((MonsterAttackCardCreateInfo)CardCreateInfo);
    }

    public string GetResStr()
    {
        return "卡片库/攻击卡";
    }

    public Dictionary<MonsterAttackEnum, MonsterAttackCfg> GetDic()
    {
        return MonsterAttackCfgs;
    }
}

public class MonsterAttackCardModel:CardModel
{
    public MonsterAttackCfg cfg;
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.CardMap[CardEnum.MonsterAttack];
        }
    }
    public MonsterAttackCardModel(MonsterAttackCardCreateInfo createInfo) : base(createInfo.CardData)
    {
        this.cfg = createInfo.MonsterAttackCfg;
    }

    public override string GetTitle()
    {
        return cfg.cardName;
    }

    public override string GetDescription()
    {
        return cfg.cardDescription;
    }
}