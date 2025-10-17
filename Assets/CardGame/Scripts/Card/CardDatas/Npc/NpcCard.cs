using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelationType
{
    father,
    mother,
    child,
}

public enum SexType
{
    male,
    female,
}

public class NpcCardData : CardData
{
    /// <summary>
    /// NPC的名字
    /// </summary>
    public string NpcName;
    /// <summary>
    /// 年龄
    /// </summary>
    public int age;
    /// <summary>
    /// 性别
    /// </summary>
    public SexType sex;
    /// <summary>
    /// npc的信息
    /// </summary>
    public Dictionary<string, int> npcInfo;
    public Dictionary<RelationType,string> relationship;
    public NpcCardData():base()
    {
        viewType = ViewType.NPCCard;
        npcInfo = new Dictionary<string, int>();
    }
    
    public override CardModel CreateModel()
    {
        return null;
    }
    public CardModel CreateModelByNpc()
    {
        return new NpcCardModel(this.NpcName,this);
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.npc;
    }
}

/// <summary>
/// NPC 的心理与性格属性
/// 用于描述调查员或怪物的行为倾向
/// 数值一般在 1-1000 之间，数值高低决定表现。
/// </summary>
public enum NpcMind
{
    /// <summary>
    /// 勇气 Courage
    /// 高：敢于直面危险，可能会冒险面对怪物。
    /// 低：容易逃避、畏缩，即使有利也会选择保守。
    /// 影响：行动优先级（战斗 vs 躲避）、调查推进速度。
    /// </summary>
    Courage,
    /// <summary>
    /// 冷静 Composure
    /// 高：在压力或恐怖场景中保持理智和逻辑。
    /// 低：容易慌乱、出错，但不直接掉 SAN。
    /// 影响：事件应对效率、对线索的解读准确度。
    /// </summary>
    Composure,
    /// <summary>
    /// 冲动性 Impulsiveness
    /// 高：倾向快速决策，贸然行动。
    /// 低：深思熟虑，行动谨慎。
    /// 影响：行动节奏、意外风险发生率。
    /// </summary>
    Impulsiveness,
    /// <summary>
    /// 攻击性 Aggressiveness
    /// 高：倾向使用武力，先打后问。
    /// 低：更偏向交涉、谈判或观察。
    /// 影响：战斗频率、与 NPC 的互动方式。
    /// </summary>
    Aggressiveness,
    /// <summary>
    /// 贪欲 Greed
    /// 高：强烈渴望财富、权力或禁忌知识。
    /// 低：淡泊名利，抗诱惑力强。
    /// 影响：腐化风险、与邪教/黑市的交集。
    /// </summary>
    Greed,
    /// <summary>
    /// 好奇心 Curiosity
    /// 高：喜欢探索未知，积极调查线索。
    /// 低：趋于回避，害怕卷入。
    /// 影响：调查深度、主动发现事件的概率。
    /// </summary>
    Curiosity,
    /// <summary>
    /// 虔诚度 Piety
    /// 高：依赖宗教或神秘信仰，容易迷信。
    /// 低：理性怀疑，不信神秘。
    /// 影响：对神秘学线索的接受度、与宗教势力的关系。
    /// </summary>
    Piety,
    /// <summary>
    /// 怀疑 Skepticism
    /// 高：坚持科学理性，容易否认异常。
    /// 低：更容易接受神秘，甚至卷入邪教。
    /// 影响：对超自然证据的解读方式。
    /// </summary>
    Skepticism,
    /// <summary>
    /// 道德观 Morality
    /// 高：遵守伦理和善恶原则。
    /// 低：目标至上，不择手段。
    /// 影响：同伴关系、关键抉择的走向。
    /// </summary>
    Morality,
    /// <summary>
    /// 权威态度 Authority
    /// 高：顺从权威，依赖秩序。
    /// 中：保持独立，不轻易站边。
    /// 低：反叛或漠视权威。
    /// 影响：与组织/政府的互动关系。
    /// </summary>
    Authority,
    /// <summary>
    /// 忠诚 Loyalty
    /// 高：坚定支持同伴或组织，不会背叛。
    /// 低：可能隐瞒情报，甚至倒戈。
    /// 影响：团队稳定性、关键事件的背叛概率。
    /// </summary>
    Loyalty,
    /// <summary>
    /// 社交需求 Sociability
    /// 高：喜欢群体和交流，依赖合作。
    /// 低：独来独往，冷漠孤僻。
    /// 影响：与 NPC 的关系、获取情报的效率。
    /// </summary>
    Sociability,
    /// <summary>
    /// 恐惧触发点 PhobiaTrigger
    /// 特定恐惧源：黑暗、虫子、血液、密闭空间等。
    /// 影响：在触发时可能陷入恐慌或行动受限。
    /// </summary>
    PhobiaTrigger,
    /// <summary>
    /// 禁忌知识接受度 ForbiddenKnowledgeAcceptance
    /// 高：主动追求神话知识，可能快速腐化。
    /// 低：本能排斥，避免接触。
    /// 影响：学习神话书籍、使用法术的倾向。
    /// </summary>
    ForbiddenKnowledgeAcceptance
}

/// <summary>
/// 角色卡
/// </summary>
public class NpcCardModel : CardModel
{
    public string npcId;
    [SerializeField]
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.NpcCardDatas[npcId];
        }
    }
    public NpcCardModel(string npcId,CardData cardData) : base(cardData)
    {
        this.npcId = npcId;
    }
}