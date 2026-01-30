using System.Collections.Generic;

public class ContainerComponent:IComponent
{
    public int AllSize;
    public int nowSize;
    public List<CardModel> Contents;
    public CardModel CardModel;
    public CardModel GetCard()
    {
        return CardModel;
    }

    public ContainerComponent(CardModel card,ContainerComponentCreator creator)
    {
        this.CardModel = card;
        this.AllSize = creator.AllSize;
        Contents = new();
    }

    public bool TryAdd(CardModel content)
    {
        if (content.IsSatComponent<MassSizeComponent>())
        {
            var cmp = content.GetComponent<MassSizeComponent>();
            if (cmp.VolumeSize+nowSize>AllSize)
            {
                return false;
            }
            else
            {
                Contents.Add(content);
                nowSize+=cmp.VolumeSize;
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    
    public void TryRemove(CardModel content)
    {
        Contents.Remove(content);
        nowSize-=content.GetComponent<MassSizeComponent>().VolumeSize;
    }
}

public class ContainerComponentCreator : IComponentCreator
{
    public int AllSize;
    public ComponentType ComponentName()
    {
        return ComponentType.ContainerComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new ContainerComponent(cardModel,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}