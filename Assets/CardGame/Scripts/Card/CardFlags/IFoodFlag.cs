/// <summary>
/// 食物卡
/// </summary>
[InterfaceBind(CardFlag.person)]
public interface IFoodFlag:IGoodsCardFlag
{
    bool CanEat(CardModel card)
    void Eat(CardModel card)
}