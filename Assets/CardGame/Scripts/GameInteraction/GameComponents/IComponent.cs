using System.Collections.Generic;

public enum ComponentType
{
    VelocityComponent =1,
    MassSizeComponent = 2,
    SubstanceComponent = 3,
    TemperatureComponent =4,
    HPComponent =5,
    BookComponent = 6,
    SpiritComponent = 7,
    SkillComponent = 8,
    AttackComponent = 9,
    CanBeAttackComponent = 10,
    CanBeSeeComponent = 11,
    ContainerComponent = 12,
    DescriptionComponent = 13,
    BelongComponent = 14,
    SleepProviderComponent = 15,
    SleepComponent = 16,
    MentalComponent = 17,
    EatFoodComponent = 18,
    SupplyFoodComponent = 19,
    NowTaskComponent = 20,
    NowStateComponent = 21,
    ProvideJobComponent = 22,
    NeedJobComponent = 23,
    BehavePointComponent =24,
    DrawLineComponent = 25,
    PathComponent = 26,
    NeedComponent = 27,
    AIMind = 28,
    SellComponent = 30,
    ProduceComponent = 31,
    ConclusionInfo = 32,
    ProvidePlayComponent=33,
    PlayComponent = 34,
    ProvideStudyComponent = 35,
    StudyComponent = 36,
    KinshipComponent = 37,
    RelationComponent = 38,
    ChatComponent = 40,
}

public class BaseComponent:IComponent
{
    public CardModel CardModel;
    public CardModel GetCard()
    {
        return CardModel;
    }

    public BaseComponent(CardModel cardModel, IComponentCreator creator)
    {
        this.CardModel = cardModel;
    }
}

public abstract class BaseComponentCreator<T> : IComponentCreator where T:IComponent
{
    public abstract ComponentType ComponentName();

    public abstract IComponent Create(CardModel cardModel);

    public virtual bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}

public interface IGetDetail
{
    public string GetName();
    public List<UIItemBinder> GetDetail();
}

/// <summary>
/// 组件接口
/// </summary>
public interface IComponent
{
    CardModel GetCard();
}
/// <summary>
/// 用于创建组件
/// </summary>
public interface IComponentCreator
{
    /// <summary>
    /// 获取接口的名字
    /// </summary>
    /// <returns></returns>
    ComponentType ComponentName();
    /// <summary>
    /// 创建组件函数
    /// </summary>
    /// <returns></returns>
    IComponent Create(CardModel cardModel);

    bool NeedComponent(List<IComponentCreator> components);
    
}