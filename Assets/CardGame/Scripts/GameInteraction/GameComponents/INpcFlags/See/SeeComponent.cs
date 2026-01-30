/// <summary>
/// 能看代表是有智力
/// </summary>
public class SeeComponent:BaseComponent
{
    public SeeComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
    }
}

public class SeeComponentCreator : BaseComponentCreator<SeeComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.SleepComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new SeeComponent(cardModel, this);
    }
}