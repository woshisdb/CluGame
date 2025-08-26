/// <summary>
/// 技能卡
/// </summary>
[InterfaceBind(CardFlag.person)]
public interface ISkillFlag:ICardFlag
{
    /// <summary>
    /// 当前技能的等级
    /// </summary>
    /// <returns></returns>
    int CardLevel();
    /// <summary>
    /// 尝试升级
    /// </summary>
    /// <returns></returns>
    int TryUp(CardModel card);
}