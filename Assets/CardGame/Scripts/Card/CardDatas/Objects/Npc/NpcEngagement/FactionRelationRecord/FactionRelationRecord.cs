using System.Collections.Generic;

public enum FactionActionType
{
    Join,//加入
    Betray,//背叛
    Attack,//攻击
    Help//帮助
}
/// <summary>
/// 组织 / 阵营记录
/// </summary>
public abstract class FactionRelationRecord:SocialImpactRecord
{
    
}

public class JoinFactionRelationRecord:FactionRelationRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}

public class BetrayFactionRelationRecord:FactionRelationRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}

public class AttackFactionRelationRecord:FactionRelationRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}

public class HelpFactionRelationRecord:FactionRelationRecord
{
    public override Dictionary<NpcThinkType, int> EffectToNpc(NpcCardModel npc)
    {
        throw new System.NotImplementedException();
    }
}