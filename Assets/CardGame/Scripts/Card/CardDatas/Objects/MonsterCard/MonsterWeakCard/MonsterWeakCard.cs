public class MonsterWeakCardData:CardData
{
    public MonsterWeakCfg cfg;
    public MonsterWeakCardData(MonsterWeakCfg cfg)
    {
        this.cfg = cfg;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterWeak;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return new MonsterWeakCardModel(this);
    }
}

public class MonsterWeakCardModel:CardModel
{
    public MonsterWeakCfg cfg;
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.CardMap[CardEnum.MonsterWeak];
        }
    }
    public MonsterWeakCardModel(CardData cardData) : base(cardData)
    {
        this.cfg = ((MonsterWeakCardData)cardData).cfg;
    }
}