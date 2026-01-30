/// <summary>
/// 社交的关系网络的基类
/// </summary>
public abstract class SocialCircle
{
    /// <summary>
    /// npcId
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public abstract bool InThisCircle(string npcId);
}
/// <summary>
/// 根据某种规则直接确定关系的
/// </summary>
public abstract class RuleSocialCircle:SocialCircle
{
    
}
/// <summary>
/// 成员申请加入制
/// </summary>
public abstract class MembershipSocialCircle : SocialCircle
{
    /// <summary>
    /// 是否可以加入圈子
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public abstract bool TryJoinThisCircle(string npcId);
    /// <summary>
    /// 是否加入
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public abstract bool CanJoin(string npcId);
    /// <summary>
    /// 可以离开
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public abstract bool CanLeave(string npcId);
    /// <summary>
    /// 是否可以离开圈子
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public abstract bool TryLeaveThisCircle(string npcId);
}
/// <summary>
/// 自我认同的圈子
/// </summary>
public abstract class IdentitySocialCircle : SocialCircle
{
    
}