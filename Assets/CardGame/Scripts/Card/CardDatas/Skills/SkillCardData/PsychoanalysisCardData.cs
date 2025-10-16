using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoanalysisCardData : SkillCardData, IStateFlag
{
    public PsychoanalysisCardData() : base()
    {
        title = "精神分析";
        description = "深入解析他人心理创伤、潜意识动机，可用于治疗心理疾病，或发现被压抑的记忆与秘密。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new PsychoanalysisCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.psychoanalysis;
    }
}



public class PsychoanalysisCardModel : CardModel
{
    public PsychoanalysisCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}