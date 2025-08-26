/// <summary>
/// 技能卡
/// </summary>
[InterfaceBind(CardFlag.skill)]
public interface ISkillFlag:ICardFlag
{
    /// <summary>
    /// 当前技能的等级
    /// </summary>
    /// <returns></returns>
    int CardLevel(CardModel card);
    /// <summary>
    /// 尝试升级
    /// </summary>
    /// <returns></returns>
    void TryUp(CardModel card);
}