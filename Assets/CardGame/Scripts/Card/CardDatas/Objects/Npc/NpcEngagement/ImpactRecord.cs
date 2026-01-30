using System.Collections.Generic;

/// <summary>
/// 与npc的一些互动记录信息
/// </summary>
public class ImpactRecord
{
    
}
/// <summary>
/// she
/// </summary>
public abstract class SocialImpactRecord : ImpactRecord
{
    public abstract Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc);
}
