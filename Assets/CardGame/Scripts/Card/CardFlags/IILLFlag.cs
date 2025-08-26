[InterfaceBind(CardFlag.person)]
public interface IILLFlag:IStateFlag
{
    /// <summary>
    /// 是否感染
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    bool CanEffect(CardModel card);
    /// <summary>
    /// 对人的影响
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    void Effect(CardModel card);
    /// <summary>
    /// 治愈了
    /// </summary>
    /// <returns></returns>
    void Cure();
}