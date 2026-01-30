using System.Collections.Generic;

public interface Position
{
    
}

/// <summary>
/// 
/// </summary>
public class OnePosition : Position
{
    public string id;
    public string npc;

    public OnePosition(string id)
    {
        this.id = id;
    }
}

/// <summary>
/// 单独一个位置，用来表示重要位置
/// </summary>
public class PositionSet:Position
{
    public string id;
    public int maxSum;
    public List<string> npcs;
    public PositionSet(string id,int maxSum)
    {
        this.id = id;
        this.maxSum = maxSum;
        npcs = new List<string>();
    }
}



/// <summary>
/// 由人发起和组织的圈子
/// </summary>
public class Organization:MembershipSocialCircle
{
    /// <summary>
    /// 所占有的一系列位置
    /// </summary>
    public Dictionary<string, Position> Positions;

    public Organization():base()
    {
        Positions = new Dictionary<string, Position>();
    }
    public override bool InThisCircle(string npcId)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryJoinThisCircle(string npcId)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanJoin(string npcId)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanLeave(string npcId)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryLeaveThisCircle(string npcId)
    {
        throw new System.NotImplementedException();
    }
}