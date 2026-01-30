using System.Collections.Generic;

public enum PersonalityBias
{
    ThreatSensitive,  // 对危险极度敏感
    TrustWeighted,    // 信任权重更高
    Emotional,        // 感情权重大
    Rational,         // 按数学计算加权
    Vengeful,         // 负面优先
    Forgiving         // 正面优先
}
/// <summary>
/// 基础的思考模块
/// </summary>
public abstract class ThinkBaseModule
{
    public NpcCardModel npc;
    public ThinkBaseModule(NpcCardModel npc)
    {
        this.npc = npc;
    }
    /// <summary>
    /// 获取思考类型的结果
    /// </summary>
    /// <param name="things"></param>
    /// <returns></returns>
    public abstract int GetThinkType(Dictionary<NpcThinkType, List<int>> things);
    /// <summary>
    /// 获取思考类型
    /// </summary>
    /// <returns></returns>
    public abstract NpcThinkType GetThinkType();
}

/// <summary>
/// npc的情感模型，包括对人对感情
/// </summary>
public class ThinkModule
{
    public NpcCardModel npc;
    public Dictionary<NpcThinkType, ThinkBaseModule> npcThinkmodules;
    public int CalculateMoodRate(NpcThinkType npcThink, Dictionary<NpcThinkType, List<int>> things)
    {
         return npcThinkmodules[npcThink].GetThinkType(things);
    }

    public ThinkModule(NpcCardModel npc)
    {
        this.npc = npc;
        npcThinkmodules = new Dictionary<NpcThinkType, ThinkBaseModule>()
        {
            { NpcThinkType.Attitude, new FriendlyAttitudeThinkMood(npc) },
            { NpcThinkType.Dominance, new FriendlyDominanceThinkMood(npc) },
            { NpcThinkType.Intimacy, new FriendlyIntimacyThinkMood(npc) },
            { NpcThinkType.Threat, new FriendlyThreatMood(npc) },
            { NpcThinkType.Trust, new FriendlyTrustThinkMood(npc) }
        };
    }
}
