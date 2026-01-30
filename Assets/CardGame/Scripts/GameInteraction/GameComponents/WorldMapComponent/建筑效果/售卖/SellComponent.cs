using System.Collections.Generic;

public class SellComponent:BaseComponent
{
    /// <summary>
    /// 售卖的物品
    /// </summary>
    public List<Item> SellItems;
    public SellComponent(CardModel cardModel, SellComponentCreator creator) : base(cardModel, creator)
    {
        SellItems = new List<Item>();
    }
}

public class SellComponentCreator : BaseComponentCreator<SellComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.SellComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new SellComponent(cardModel, this);
    }
}