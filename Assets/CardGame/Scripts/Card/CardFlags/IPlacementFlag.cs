/// <summary>
/// 地点卡
/// </summary>
[InterfaceBind(CardFlag.person)]
public interface IPlacementFlag:ICardFlag
{
    bool CanEnter(CardModel card)
    bool Enter(CardModel card)
}