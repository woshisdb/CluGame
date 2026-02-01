using System.Collections.Generic;

/// <summary>
/// 所属的组件
/// </summary>
public class BelongComponent:IComponent
{
    public ObjectRef<IBelong> belong = new ObjectRef<IBelong>();
    public CardModel CardModel;
    public CardModel GetCard()
    {
        return CardModel;
    }

    public BelongComponent(CardModel cardModel,BelongComponentCreator creator)
    {
        this.CardModel = cardModel;
        this.belong.Value = creator.belong.Value;
    }

    public void Init()
    {
        var x = belong.Value;
        belong.SetNull();
        if (x!=null)
        {
            x.Enter(CardModel as NpcCardModel);
        }
    }
}

public class BelongComponentCreator:IComponentCreator
{
    public ObjectRef<IBelong> belong = new();
    public ComponentType ComponentName()
    {
        return ComponentType.BelongComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new BelongComponent(cardModel, this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}