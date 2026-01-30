/// <summary>
/// 提供娱乐
/// </summary>
public class ProvidePlayComponent : BaseComponent, ISupply<NeedPlayComponent>
{
    /// <summary>
    /// 娱乐效率倍率
    /// </summary>
    public float Rate;

    public SupplyModule<NeedPlayComponent> Provider;

    public ProvidePlayComponent(CardModel cardModel, IComponentCreator creator)
        : base(cardModel, creator)
    {
        var ctx = creator as ProvidePlayComponentCreator;
        Rate = ctx.Rate;

        Provider = new SupplyModule<NeedPlayComponent>(
            cardModel,
            ctx.sum,
            e =>
            {
                // 是否允许被使用（可扩展：是否损坏、是否开放等）
                return true;
            },
            e =>
            {
                // 开始提供娱乐
            },
            e =>
            {
                // 结束提供娱乐
            }
        );
    }

    public SupplyModule<NeedPlayComponent> GetSupply()
    {
        return Provider;
    }
}
public class ProvidePlayComponentCreator : BaseComponentCreator<ProvidePlayComponent>
{
    /// <summary>
    /// 可同时服务的数量
    /// </summary>
    public int sum;

    /// <summary>
    /// 娱乐效率倍率
    /// </summary>
    public float Rate;

    public override ComponentType ComponentName()
    {
        return ComponentType.ProvidePlayComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ProvidePlayComponent(cardModel, this);
    }
}
