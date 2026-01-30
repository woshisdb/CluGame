public class MonsterMoveCardData:CardData
{
    public MonsterMoveCfg cfg;
    public MonsterMoveCardData(MonsterMoveCfg cfg)
    {
        this.cfg = cfg;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterMove;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new MonsterMoveCardModel(this);
    }
}

public class MonsterMoveCardModel:CardModel
{
    public MonsterMoveCfg cfg;
    public MonsterMoveCardModel(CardData cardData) : base(cardData)
    {
        this.cfg = ((MonsterMoveCardData)cardData).cfg;
    }
}