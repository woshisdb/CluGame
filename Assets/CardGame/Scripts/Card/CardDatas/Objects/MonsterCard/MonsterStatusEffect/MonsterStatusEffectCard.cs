
using System.Collections.Generic;

public struct MonsterStatusEffectCardCreateInfo : CardCreateInfo
{
    public MonsterStatusEffectCfg MonsterStatusEffectCfg;
    public CardEnum Belong()
    {
        return CardEnum.MonsterEffect;
    }
}

public class MonsterStatusEffectCardData:CardData,CardDataDic<StatusEffectType,MonsterStatusEffectCfg>
{
    public Dictionary<StatusEffectType, MonsterStatusEffectCfg> MonsterStatusEffectCfgs;
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterEffect;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        var info = (MonsterStatusEffectCardCreateInfo)CardCreateInfo;
        return new MonsterStatusEffectModel(info.MonsterStatusEffectCfg,info.GetCardData());
    }

    public string GetResStr()
    {
        return "卡片库/效果";
    }

    public Dictionary<StatusEffectType, MonsterStatusEffectCfg> GetDic()
    {
        return MonsterStatusEffectCfgs;
    }

    public MonsterStatusEffectCardData() : base()
    {
        this.Init(ref MonsterStatusEffectCfgs);
    }
}

public class MonsterStatusEffectModel : CardModel
{
    public MonsterStatusEffectCfg cfg;
    public MonsterStatusEffectModel(MonsterStatusEffectCfg MonsterStatusEffectCfg,CardData cardData) : base(cardData)
    {
        this.cfg = MonsterStatusEffectCfg;
    }
    public override string GetTitle()
    {
        return cfg.name;
    }

    public override string GetDescription()
    {
        return cfg.descirption;
    }
}