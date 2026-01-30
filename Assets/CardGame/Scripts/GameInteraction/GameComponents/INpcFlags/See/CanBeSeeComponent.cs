using System.Collections.Generic;

public class CanBeSeeComponent:IComponent
{
    public CardModel CardModel;
    /// <summary>
    /// 当被看
    /// </summary>
    /// <param name="seeMapBehave"></param>
    public void WhenBeSee(SeeMapBehave seeMapBehave)
    {
        
    }

    public CardModel GetCard()
    {
        return CardModel;
    }

    public CanBeSeeComponent(CardModel cardModel,CanBeSeeComponentCreator creator)
    {
        this.CardModel = cardModel;
    }
}

public class CanBeSeeComponentCreator : IComponentCreator
{
    public ComponentType ComponentName()
    {
        return ComponentType.CanBeSeeComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new CanBeSeeComponent(cardModel,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}