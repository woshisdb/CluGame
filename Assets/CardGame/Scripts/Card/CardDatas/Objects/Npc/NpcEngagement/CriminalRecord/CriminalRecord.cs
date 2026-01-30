using System.Collections.Generic;

public enum CrimeType
{
    Theft=1,        // 盗窃
    Assault=2,      // 伤害 / 殴打
    Murder=3,       // 谋杀
    Fraud=4,        // 欺诈
    Arson=5,        // 纵
    Trespassing=6,   // 非法入侵
    Destroy=7,//破坏
}
/// <summary>
/// 一些犯罪
/// </summary>
public abstract class CriminalRecord : SocialImpactRecord
{
    
}

/// <summary>
/// 盗窃
/// </summary>
public class TheftCriminalRecord : CriminalRecord
{
    /// <summary>
    /// 对应对ID
    /// </summary>
    public string npcId;
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 伤害
/// </summary>
public class AssaultCriminalRecord : CriminalRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 谋杀
/// </summary>
public class MurderCriminalRecord : CriminalRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 欺诈
/// </summary>
public class FraudCriminalRecord : CriminalRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 非法入侵
/// </summary>
public class TrespassingCriminalRecord : CriminalRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 破坏行为
/// </summary>
public class DestroyCriminalRecord : CriminalRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}