public class ConclusionComponent:BaseComponent
{
    public string conclusionStr;
    public ConclusionEnum conclusionType;
    public ConclusionComponent(CardModel cardModel, ConclusionInfoComponentCreator creator) : base(cardModel, creator)
    {
        this.conclusionStr = creator.conclusionStr;
        this.conclusionType = creator.conclusionType;
    }

    public string GetConclusion(NpcCardModel npc)
    {
        if (conclusionType!=null)
        {
            return ConclusionMap.cmap[conclusionType].GetConclusion(CardModel as SpaceCardModel, npc);
        }
        else
        {
            return conclusionStr;
        }
    }
}

public class ConclusionInfoComponentCreator : BaseComponentCreator<ConclusionComponent>
{
    public string conclusionStr;
    public ConclusionEnum conclusionType;
    public override ComponentType ComponentName()
    {
        return ComponentType.ConclusionInfo;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ConclusionComponent(cardModel, this);
    }

}