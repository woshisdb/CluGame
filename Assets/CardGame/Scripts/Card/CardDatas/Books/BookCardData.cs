public class BookCardData : CardData, IBookFlag
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
}