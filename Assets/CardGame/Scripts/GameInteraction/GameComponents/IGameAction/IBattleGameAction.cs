/// <summary>
/// 战斗行为的接口
/// </summary>
public interface IBattleGameAction:IBaseGameAction
{
    /// <summary>
    /// 获取线索Id
    /// </summary>
    /// <returns></returns>
    int GetClueId();
    /// <summary>
    /// 获取满足条件的进度
    /// </summary>
    /// <returns></returns>
    (int, int) GetSatProcess();
    /// <summary>
    /// 设置已经获取线索的Id
    /// </summary>
    void SetHasGetClue();
    /// <summary>
    /// 获取线索的描述
    /// </summary>
    /// <returns></returns>
    string GetClueDescription();
}