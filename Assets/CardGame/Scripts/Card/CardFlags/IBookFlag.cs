/// <summary>
/// 书籍的Flag
/// </summary>
[InterfaceBind(CardFlag.book)]
public interface IBookFlag:IGoodsCardFlag
{
    bool CanRead(CardModel card);
    bool Read(CardModel card);
}