
using System.Collections.Generic;

public enum NpcThinkType
{
    /// <summary>
    /// 态度:NPC 对玩家的情绪基调：友善、冷漠、敌意
    /// </summary>
    Attitude,
    /// <summary>
    /// 信任程度:NPC 是否提供信息、是否合作、是否撒谎
    /// </summary>
    Trust,
    /// <summary>
    /// 支配程度:玩家威慑 / NPC 是否被压制、是否敢反抗
    /// </summary>
    Dominance,
    /// <summary>
    /// 威胁程度:NPC 是否逃跑、是否隐藏信息、是否攻击
    /// </summary>
    Threat,
    /// <summary>
    /// 亲密程度:是否允许接触隐私、是否透露秘密、是否求助
    /// </summary>
    Intimacy,
}

/// <summary>
/// NPC 对玩家的态度得分范围：-3 到 3
/// </summary>
public enum AttitudeLevel
{
    Hostile = -3,      // 极度敌意
    Unfriendly = -2,   // 明显不喜欢
    Cold = -1,         // 冷漠
    Neutral = 0,       // 中立
    Polite = 1,        // 有点友好
    Friendly = 2,      // 友好
    VeryFriendly = 3   // 非常喜欢
}

public class NpcThink
{
    public NpcCardModel npc;

    public ThinkModule npcThinks;
    /// <summary>
    /// 一系列对社会记录
    /// </summary>
    public List<SocialImpactRecord> socialImpactRecords;
    public NpcThink(NpcCardModel npc)
    {
        this.npc = npc;
        this.npcThinks = new ThinkModule(npc);
        socialImpactRecords = new List<SocialImpactRecord>();
    }

    public int GetMoodByType(NpcThinkType npcThinkType,NpcCardModel other)
    {
        var records = other.NpcThink.socialImpactRecords;
        Dictionary<NpcThinkType, List<int>> ret = new Dictionary<NpcThinkType, List<int>>();
        ret[NpcThinkType.Attitude] = new List<int>();
        ret[NpcThinkType.Trust] = new List<int>();
        ret[NpcThinkType.Dominance] = new List<int>();
        ret[NpcThinkType.Intimacy] = new List<int>();
        ret[NpcThinkType.Threat] = new List<int>();

        foreach (var record in records)
        {
            var effectRecord = record.EffectToNpc(other);
            foreach (var obj in effectRecord)
            {
                ret[obj.Key].Add(obj.Value);
            }
        }
        
        return npcThinks.CalculateMoodRate(npcThinkType,ret);
    }
}