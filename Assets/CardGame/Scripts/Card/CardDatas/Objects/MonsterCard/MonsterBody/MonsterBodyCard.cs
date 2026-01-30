public struct MonsterBodyCardCreateInfo:CardCreateInfo
{
    public MonsterBody monstBody;
    public MonsterBodyCardCreateInfo(MonsterBody monsterBody)
    {
        this.monstBody = monsterBody;
    }

    public CardEnum Belong()
    {
        return CardEnum.MonsterBody;
    }
}
public class MonsterBodyCardData:CardData
{
    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterBody;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        var info = (MonsterBodyCardCreateInfo)CardCreateInfo;
        return new MonsterBodyCardModel(info.monstBody,info.GetCardData());
    }
}

public class MonsterBodyCardModel : CardModel
{
    public MonsterBody monsterBody;
    public MonsterBodyCardModel(MonsterBody MonsterBody,CardData cardData) : base(cardData)
    {
        this.monsterBody = MonsterBody;
    }
    public override string GetTitle()
    {
        return monsterBody.name;
    }

    public override string GetDescription()
    {
        return monsterBody.description;
    }
}