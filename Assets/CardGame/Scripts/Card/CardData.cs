using System.Collections.Generic;

public abstract class CardData
{
    public string title;
    public string description;
    public ViewType viewType;
    public HashSet<CardFlag> cardFlags;
    public CardEnum cardEnum;
    public bool needRefresh;
    public CardData()
    {
        cardFlags = new HashSet<CardFlag>();
    }
    public abstract CardEnum GetCardType();
    public abstract CardModel CreateModel();
}