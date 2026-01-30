using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public struct SkillCardCreateInfo : CardCreateInfo
{
    public NpcSkill skillConfig;

    public CardEnum Belong()
    {
        return CardEnum.skill;
    }

    public SkillCardCreateInfo(NpcSkill skillConfig)
    {
        this.skillConfig = skillConfig;
    }
}

public class SkillCardData : CardData,CardDataDic<NpcSkill,SkillConfig>
{
    public Dictionary<NpcSkill, SkillConfig> SkillConfigs;
    public SkillCardData():base()
    {
        title = "技能";
        description = "技能!";
        viewType = ViewType.SkillCard;
        this.Init<NpcSkill,SkillConfig>(ref SkillConfigs);
    }
    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return null;
    }

    public Dictionary<NpcSkill, SkillConfig> GetDic()
    {
        return SkillConfigs;
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.skill;
    }

    
    [Button]
    public CardModel CreateSkill(SkillConfig skillConfig)
    {
        var book = new SkillCardModel(this,skillConfig);
        GameFrameWork.Instance.cardsManager.cardmodels.Add(book);
        return book;
    }
    
    public override CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        var info = (SkillCardCreateInfo)data;
        return CreateSkill(SkillConfigs[info.skillConfig]);
    }

    public string GetResStr()
    {
        return "卡片库/技能";
    }
}

public class SkillCardModel : CardModel
{
    public SkillConfig SkillConfig;
    public SkillCardModel(CardData cardData,SkillConfig skillConfig) : base(cardData)
    {
        this.SkillConfig = skillConfig;
    }

    public override string GetTitle()
    {
        return SkillConfig.title;
    }

    public override string GetDescription()
    {
        return SkillConfig.descirption;
    }
}