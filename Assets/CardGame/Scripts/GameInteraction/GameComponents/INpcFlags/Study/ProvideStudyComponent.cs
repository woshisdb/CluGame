/// <summary>
/// 提供学习
/// </summary>
public class ProvideStudyComponent : BaseComponent, ISupply<NeedStudyComponent>
{
    /// <summary>
    /// 学习效率倍率
    /// </summary>
    public float Rate;

    public SupplyModule<NeedStudyComponent> Provider;

    public ProvideStudyComponent(CardModel cardModel, IComponentCreator creator)
        : base(cardModel, creator)
    {
        var ctx = creator as ProvideStudyComponentCreator;
        Rate = ctx.Rate;

        Provider = new SupplyModule<NeedStudyComponent>(
            cardModel,
            ctx.sum,
            e =>
            {
                // 是否可用于学习（例如是否开放、是否损坏）
                return true;
            },
            e =>
            {
                // 开始学习
            },
            e =>
            {
                // 结束学习
            }
        );
    }

    public SupplyModule<NeedStudyComponent> GetSupply()
    {
        return Provider;
    }
}
public class ProvideStudyComponentCreator : BaseComponentCreator<ProvideStudyComponent>
{
    /// <summary>
    /// 可同时学习的人数
    /// </summary>
    public int sum;

    /// <summary>
    /// 学习效率倍率
    /// </summary>
    public float Rate;

    public override ComponentType ComponentName()
    {
        return ComponentType.ProvideStudyComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ProvideStudyComponent(cardModel, this);
    }
}
