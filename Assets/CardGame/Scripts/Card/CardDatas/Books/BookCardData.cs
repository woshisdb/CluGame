using System.Collections.Generic;

public class BookCardData : CardData, IBookFlag,IInteractionCard
{
    public bool CanRead(CardModel card)
    {
        throw new System.NotImplementedException();
    }

    public override CardModel CreateModel()
    {
        throw new System.NotImplementedException();
    }

    public override CardEnum GetCardType()
    {
        throw new System.NotImplementedException();
    }

    public bool Read(CardModel card)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 与卡片的交互
    /// </summary>
    /// <param name="inputs"></param>
    public void Interaction(Dictionary<string, CardModel> inputs,TaskPanelModel task)
    {
        
    }
}

public class BookCardModel : CardModel
{
    public BookCardModel(CardData cardData) : base(cardData)
    {
        
    }
}