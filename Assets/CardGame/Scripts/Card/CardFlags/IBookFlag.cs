/// <summary>
/// 书籍的Flag
/// </summary>
[InterfaceBind(CardFlag.person)]
public interface IBookFlag：IGoodsCardFlag
{
    pub
    bool CanRead(CardModel card)
    bool Read(CardModel card)
}