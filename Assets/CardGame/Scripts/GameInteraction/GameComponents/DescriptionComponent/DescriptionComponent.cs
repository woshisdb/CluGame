using System.Collections.Generic;
public class DescriptionComponent:IComponent
{
    public string title;
    public string description;
    public CardModel card;
    public DescriptionComponent(CardModel card,DescriptionComponentCreator creator)
    {
        this.card = card;
        this.title = creator.title;
        this.description = creator.description;
    }
    public CardModel GetCard()
    {
        return card;
    }
}

public class DescriptionComponentCreator : IComponentCreator
{
    public string title;
    public string description;
    public ComponentType ComponentName()
    {
        return ComponentType.DescriptionComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new DescriptionComponent(cardModel,this);
    }
    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}