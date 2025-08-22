public class CardsManager
{
    /// <summary>
    /// 整个游戏的一系列的卡牌
    /// </summary>
    public List<CardModel> cardmodels;
    public CardsManager()
    {
        cardmodels = new List<CardModel>();
    }
    /// <summary>
    /// 刷新每一帧的行为
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        foreach (var cardModel in cardmodels)
        {
            if (cardModel.NeedRefresh())
            {
                if (cardModel.hasSwitch())
                {
                    GameFrameWork.Instance.ViewModelManager.RefreshView(cardModel);
                }
            }
        }
    }
}