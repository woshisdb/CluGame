using System.Collections.Generic;

/// <summary>
/// 睡眠
/// </summary>
public class SleepProviderComponent:BaseComponent,ISupply<NeedSleepComponent>
{
    public float Rate;
    public SupplyModule<NeedSleepComponent> Provider;
    public SleepProviderComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        var ctx = creator as SleepProviderComponentCreator;
        Rate = ctx.Rate;
        Provider = new SupplyModule<NeedSleepComponent>(cardModel,ctx.sum, e =>
        {
            return true;
        }, (e) =>
        {
            
        }, (e) =>
        {
            
        });
    }
    public SupplyModule<NeedSleepComponent> GetSupply()
    {
        return Provider;
    }
}

public class SleepProviderComponentCreator : BaseComponentCreator<SleepProviderComponent>
{
    public int sum;
    public float Rate;
    public override ComponentType ComponentName()
    {
        return ComponentType.SleepProviderComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new SleepProviderComponent(cardModel, this);
    }
}