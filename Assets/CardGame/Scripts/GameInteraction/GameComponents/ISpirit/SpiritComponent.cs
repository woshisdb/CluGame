using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

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
    Courage =0,
    /// <summary>
    /// 冷静 Composure
    /// 高：在压力或恐怖场景中保持理智和逻辑。
    /// 低：容易慌乱、出错，但不直接掉 SAN。
    /// 影响：事件应对效率、对线索的解读准确度。
    /// </summary>
    Composure=1,
    /// <summary>
    /// 冲动性 Impulsiveness
    /// 高：倾向快速决策，贸然行动。
    /// 低：深思熟虑，行动谨慎。
    /// 影响：行动节奏、意外风险发生率。
    /// </summary>
    Impulsiveness=2,
    /// <summary>
    /// 攻击性 Aggressiveness
    /// 高：倾向使用武力，先打后问。
    /// 低：更偏向交涉、谈判或观察。
    /// 影响：战斗频率、与 NPC 的互动方式。
    /// </summary>
    Aggressiveness=3,
    /// <summary>
    /// 贪欲 Greed
    /// 高：强烈渴望财富、权力或禁忌知识。
    /// 低：淡泊名利，抗诱惑力强。
    /// 影响：腐化风险、与邪教/黑市的交集。
    /// </summary>
    Greed=4,
    /// <summary>
    /// 好奇心 Curiosity
    /// 高：喜欢探索未知，积极调查线索。
    /// 低：趋于回避，害怕卷入。
    /// 影响：调查深度、主动发现事件的概率。
    /// </summary>
    Curiosity=5,
    /// <summary>
    /// 虔诚度 Piety
    /// 高：依赖宗教或神秘信仰，容易迷信。
    /// 低：理性怀疑，不信神秘。
    /// 影响：对神秘学线索的接受度、与宗教势力的关系。
    /// </summary>
    Piety=6,
    /// <summary>
    /// 怀疑 Skepticism
    /// 高：坚持科学理性，容易否认异常。
    /// 低：更容易接受神秘，甚至卷入邪教。
    /// 影响：对超自然证据的解读方式。
    /// </summary>
    Skepticism=7,
    /// <summary>
    /// 道德观 Morality
    /// 高：遵守伦理和善恶原则。
    /// 低：目标至上，不择手段。
    /// 影响：同伴关系、关键抉择的走向。
    /// </summary>
    Morality=8,
    /// <summary>
    /// 权威态度 Authority
    /// 高：顺从权威，依赖秩序。
    /// 中：保持独立，不轻易站边。
    /// 低：反叛或漠视权威。
    /// 影响：与组织/政府的互动关系。
    /// </summary>
    Authority=10,
    /// <summary>
    /// 忠诚 Loyalty
    /// 高：坚定支持同伴或组织，不会背叛。
    /// 低：可能隐瞒情报，甚至倒戈。
    /// 影响：团队稳定性、关键事件的背叛概率。
    /// </summary>
    Loyalty=11,
    /// <summary>
    /// 社交需求 Sociability
    /// 高：喜欢群体和交流，依赖合作。
    /// 低：独来独往，冷漠孤僻。
    /// 影响：与 NPC 的关系、获取情报的效率。
    /// </summary>
    Sociability=12,
    /// <summary>
    /// 禁忌知识接受度 ForbiddenKnowledgeAcceptance
    /// 高：主动追求异常事情背后的事情。
    /// 低：本能排斥异常事件，避免接触。
    /// 影响：是否积极的面对未知事件。
    /// </summary>
    ForbiddenKnowledgeAcceptance=13
}
public class PersonalityTag
{
    public string Id;
    public string Description;

    public override string ToString()
    {
        return Description;
    }
}
public class SpiritRangeRule
{
    public NpcMind Mind;
    public int Min;
    public int Max;

    public PersonalityTag Tag;

    public bool Match(StateItem<NpcMind> state)
    {
        return state.value >= Min && state.value <= Max;
    }
}
public static class SpiritPersonalityBuilder
{
    public static List<PersonalityTag> Build(
        SpiritComponent spirit,
        List<SpiritRangeRule> rules)
    {
        var result = new List<PersonalityTag>();

        foreach (var rule in rules)
        {
            if (!spirit.dataMap.TryGetValue(rule.Mind, out var state))
                continue;

            if (rule.Match(state))
                result.Add(rule.Tag);
        }

        return result;
    }
}
public static class DefaultSpiritPersonalityRules
{
    public static readonly List<SpiritRangeRule> Rules = new()
    {
        // ==================================================
        // Courage 勇气
        // ==================================================
        new()
        {
            Mind = NpcMind.Courage,
            Min = 0, Max = 20,
            Tag = new PersonalityTag { Id = "crippling_fear", Description = "对危险有病态级的恐惧" }
        },
        new()
        {
            Mind = NpcMind.Courage,
            Min = 21, Max = 40,
            Tag = new PersonalityTag { Id = "timid", Description = "倾向于回避冲突和风险" }
        },
        new()
        {
            Mind = NpcMind.Courage,
            Min = 61, Max = 80,
            Tag = new PersonalityTag { Id = "brave", Description = "在必要时愿意直面威胁" }
        },
        new()
        {
            Mind = NpcMind.Courage,
            Min = 81, Max = 100,
            Tag = new PersonalityTag { Id = "reckless_valor", Description = "几乎不计后果地面对危险" }
        },

        // ==================================================
        // Composure 冷静
        // ==================================================
        new()
        {
            Mind = NpcMind.Composure,
            Min = 0, Max = 25,
            Tag = new PersonalityTag { Id = "panic_stricken", Description = "在压力下迅速崩溃" }
        },
        new()
        {
            Mind = NpcMind.Composure,
            Min = 26, Max = 45,
            Tag = new PersonalityTag { Id = "nervous", Description = "容易紧张，判断力下降" }
        },
        new()
        {
            Mind = NpcMind.Composure,
            Min = 70, Max = 85,
            Tag = new PersonalityTag { Id = "steady_mind", Description = "大多数情况下保持冷静" }
        },
        new()
        {
            Mind = NpcMind.Composure,
            Min = 86, Max = 100,
            Tag = new PersonalityTag { Id = "unshakable", Description = "面对恐怖几乎不会动摇" }
        },

        // ==================================================
        // Impulsiveness 冲动
        // ==================================================
        new()
        {
            Mind = NpcMind.Impulsiveness,
            Min = 0, Max = 25,
            Tag = new PersonalityTag { Id = "overthinker", Description = "行动前反复权衡，犹豫不决" }
        },
        new()
        {
            Mind = NpcMind.Impulsiveness,
            Min = 26, Max = 45,
            Tag = new PersonalityTag { Id = "cautious_actor", Description = "谨慎行动，偏向安全选择" }
        },
        new()
        {
            Mind = NpcMind.Impulsiveness,
            Min = 70, Max = 85,
            Tag = new PersonalityTag { Id = "rash", Description = "常常未经充分考虑就行动" }
        },
        new()
        {
            Mind = NpcMind.Impulsiveness,
            Min = 86, Max = 100,
            Tag = new PersonalityTag { Id = "hot_blooded", Description = "极端冲动，容易制造意外" }
        },

        // ==================================================
        // Aggressiveness 攻击性
        // ==================================================
        new()
        {
            Mind = NpcMind.Aggressiveness,
            Min = 0, Max = 25,
            Tag = new PersonalityTag { Id = "peace_seeker", Description = "极力避免任何形式的冲突" }
        },
        new()
        {
            Mind = NpcMind.Aggressiveness,
            Min = 26, Max = 45,
            Tag = new PersonalityTag { Id = "defensive", Description = "只在必要时使用武力" }
        },
        new()
        {
            Mind = NpcMind.Aggressiveness,
            Min = 70, Max = 85,
            Tag = new PersonalityTag { Id = "violent_tendency", Description = "倾向于用暴力解决问题" }
        },
        new()
        {
            Mind = NpcMind.Aggressiveness,
            Min = 86, Max = 100,
            Tag = new PersonalityTag { Id = "bloodthirsty", Description = "对暴力有病态依赖" }
        },

        // ==================================================
        // Curiosity 好奇心
        // ==================================================
        new()
        {
            Mind = NpcMind.Curiosity,
            Min = 0, Max = 25,
            Tag = new PersonalityTag { Id = "avoidant", Description = "刻意回避未知与异常" }
        },
        new()
        {
            Mind = NpcMind.Curiosity,
            Min = 26, Max = 45,
            Tag = new PersonalityTag { Id = "reserved_observer", Description = "保持距离地观察事物" }
        },
        new()
        {
            Mind = NpcMind.Curiosity,
            Min = 70, Max = 85,
            Tag = new PersonalityTag { Id = "investigator", Description = "积极调查未知线索" }
        },
        new()
        {
            Mind = NpcMind.Curiosity,
            Min = 86, Max = 100,
            Tag = new PersonalityTag { Id = "obsessive_seeker", Description = "对真相有近乎病态的执念" }
        },

        // ==================================================
        // Forbidden Knowledge 禁忌知识
        // ==================================================
        new()
        {
            Mind = NpcMind.ForbiddenKnowledgeAcceptance,
            Min = 0, Max = 25,
            Tag = new PersonalityTag { Id = "knowledge_fearful", Description = "本能排斥神话与禁忌" }
        },
        new()
        {
            Mind = NpcMind.ForbiddenKnowledgeAcceptance,
            Min = 26, Max = 45,
            Tag = new PersonalityTag { Id = "reluctant_reader", Description = "必要时才接触禁忌知识" }
        },
        new()
        {
            Mind = NpcMind.ForbiddenKnowledgeAcceptance,
            Min = 70, Max = 85,
            Tag = new PersonalityTag { Id = "occult_student", Description = "系统性研究神秘学" }
        },
        new()
        {
            Mind = NpcMind.ForbiddenKnowledgeAcceptance,
            Min = 86, Max = 100,
            Tag = new PersonalityTag { Id = "forbidden_seeker", Description = "主动追逐禁忌与深渊真理" }
        },
    };

}
public static class MindMagnitudeMap
{
    public static readonly Dictionary<string, int> Map = new()
    {
        { "tiny", 1 },
        { "small", 3 },
        { "medium", 6 },
        { "large", 10 },
        { "extreme", 18 }
    };
}

[GptResponse("NPC 心理变化评估结果")]
[Serializable]
public class GptMindResult
{
    public List<GptMindEffect> mind_effects;
}

[Serializable]
public class GptMindEffect
{
    public string mind;        // NpcMind 枚举名
    public string direction;   // increase / decrease
    public string magnitude;   // tiny / small / medium / large / extreme
    public string reason;
}

/// <summary>
/// 动物的精神
/// </summary>
public class SpiritComponent:IComponent
{
    public CardModel CardModel;
    public static HashSet<NpcMind> SpiritState = new()
    {
        NpcMind.Courage,
        NpcMind.Composure,
        NpcMind.Impulsiveness,
        NpcMind.Aggressiveness,
        NpcMind.Greed,
        NpcMind.Curiosity,
        NpcMind.Piety,
        NpcMind.Skepticism,
        NpcMind.Morality,
        NpcMind.Authority,
        NpcMind.Loyalty,
        NpcMind.Sociability,
        NpcMind.ForbiddenKnowledgeAcceptance,
    };

    public CardModel GetCard()
    {
        return CardModel;
    }

    public Dictionary<NpcMind, StateItem<NpcMind>> dataMap;
    public SpiritComponent(CardModel cardModel,SpiritComponentCreator creator)
    {
        this.CardModel = cardModel;
        dataMap = new();
        foreach (var s in SpiritState)
        {
            dataMap[s] = new StateItem<NpcMind>(s,50,100);
        }
    }
    // ⭐ 新增：生成人格标签
    public List<PersonalityTag> GetPersonalityTags()
    {
        return SpiritPersonalityBuilder.Build(
            this,
            DefaultSpiritPersonalityRules.Rules
        );
    }
    [Button]
    public async Task ApplyEventDescriptionAsync(string eventDescription)
    {
        var gptMessages = BuildGptMessages(eventDescription);
        // 1. 直接调用你现有的 GPT 系统
        GptMindResult result =
            await GameFrameWork.Instance.GptSystem
                .ChatToGPT<GptMindResult>(gptMessages);
        
        if (result?.mind_effects == null)
            return;
        
        // 2. 应用 GPT 判断结果（安全、可控）
        foreach (var effect in result.mind_effects)
        {
            if (!Enum.TryParse(effect.mind, out NpcMind mind))
                continue;
        
            if (!dataMap.TryGetValue(mind, out var state))
                continue;
        
            int delta = MindMagnitudeMap.Map
                .GetValueOrDefault(effect.magnitude, 0);
        
            if (effect.direction == "decrease")
                delta = -delta;
        
            state.value = Math.Clamp(
                state.value + delta,
                0,
                state.maxValue
            );
        }
    }
    private List<QwenChatMessage> BuildGptMessages(string eventDescription)
{
    string schema = GptSchemaBuilder.BuildSchema(typeof(GptMindResult));
    // 当前 Mind 快照
    var mindSnapshot = new Dictionary<string, int>();
    foreach (var kv in dataMap)
        mindSnapshot[kv.Key.ToString()] = kv.Value.value;
    var systemPrompt = new QwenChatMessage
    {
        role = "system",
        content = $@"
你是一个【克苏鲁风格调查员心理裁判】。

规则：
- 你只判断心理变化【方向】和【强度】
- 不要输出任何具体数值
- 只影响与事件直接相关的 Mind
- 输出必须是 JSON，且能被程序解析

可用 Mind：
{string.Join(", ", Enum.GetNames(typeof(NpcMind)))}
强度等级：
tiny / small / medium / large / extreme
"
    };

    var userPrompt = new QwenChatMessage
    {
        role = "user",
        content = $@"
NPC 当前心理状态（0-100）：
{JsonUtility.ToJson(mindSnapshot)}
事件描述：
""'{eventDescription}'""
请严格返回以下 JSON 结构，不要输出多余文本。JSON 结构：
{schema}
"
    };
    return new List<QwenChatMessage>
    {
        systemPrompt,
        userPrompt
    };
}
}

public class SpiritComponentCreator : IComponentCreator
{
    public ComponentType ComponentName()
    {
        return ComponentType.SpiritComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new SpiritComponent(cardModel,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}