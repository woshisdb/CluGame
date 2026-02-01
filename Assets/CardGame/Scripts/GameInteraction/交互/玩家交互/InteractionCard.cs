public enum InteractionType
{
    Attack=1,
    Move=2,
    ReadBook=3,
    See=4,
    Sleep=5,
    GetJob = 6,
    Detail = 7,
    Awark = 8,
    StopEat = 9,
    GotoSpace = 10,
    StartWork = 11,
    EndWork = 12,
    Talk,
}

public class InteractionCardCreateInfo:CardCreateInfo
{
    public CardEnum Belong()
    {
        return CardEnum.Interaction;
    }
}


public class InteractionCardData : CardData
{
    public override CardEnum GetCardType()
    {
        return CardEnum.Interaction;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new InteractionCardModel(CardCreateInfo.GetCardData());
    }
}

public class InteractionCardModel:CardModel
{
    public InteractionCardModel(CardData cardData) : base(cardData)
    {
        
    }
    
}