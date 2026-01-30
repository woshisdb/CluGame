public class BuildingCardCreateInfo:CardCreateInfo
{
    public BuildingCardCfg buildingCfg;
    public CardEnum Belong()
    {
        return CardEnum.BuildingCard;
    }
}

public class BuildingCardData:CardData
{
    public override CardEnum GetCardType()
    {
        return CardEnum.BuildingCard;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        var creator = CardCreateInfo as BuildingCardCreateInfo;
        return new BuildingCardModel(CardCreateInfo.GetCardData(),creator.buildingCfg);
    }
}

public class BuildingCardModel : CardModel
{
    public BuildingCardModel(CardData cardData,BaseCardConfig cfg) : base(cardData,cfg)
    {
        
    }
}