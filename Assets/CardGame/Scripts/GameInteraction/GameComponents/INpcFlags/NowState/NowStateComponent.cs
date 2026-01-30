public class NowStateComponent:BaseComponent
{
    public NowStateComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
    }
}

public class NowStateComponentCreator:BaseComponentCreator<NowStateComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.NowStateComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NowStateComponent(cardModel, this);
    }
}