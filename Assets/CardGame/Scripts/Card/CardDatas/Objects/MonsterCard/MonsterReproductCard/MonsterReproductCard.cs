public class MonsterReproductCardData:CardData
{
    public MonsterReproductCfg cfg;
    public MonsterReproductCardData(MonsterReproductCfg cfg)
    {
        this.cfg = cfg;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterReproduct;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new MonsterReproductCardModel(this);
    }
}

public class MonsterReproductCardModel:CardModel
{
    public MonsterReproductCfg cfg;
    public MonsterReproductCardModel(CardData cardData) : base(cardData)
    {
        this.cfg = ((MonsterReproductCardData)cardData).cfg;
    }
}