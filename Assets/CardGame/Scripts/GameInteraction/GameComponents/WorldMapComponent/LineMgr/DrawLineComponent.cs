using System.Collections.Generic;

public class CardLineData
{
    public string action;
    public BaseCardConfig from;
    public BaseCardConfig to;
}

public class DrawLineComponent:BaseComponent
{
    public List<CardLineData> CardLineDatas;
    public DrawLineComponent(CardModel cardModel, DrawLineComponentCreator creator) : base(cardModel, creator)
    {
        this.CardLineDatas = creator.CardLineDatas;
    }
}

public class DrawLineComponentCreator : BaseComponentCreator<DrawLineComponent>
{
    public List<CardLineData> CardLineDatas = new();
    public override ComponentType ComponentName()
    {
        return ComponentType.DrawLineComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new DrawLineComponent(cardModel, this);
    }
}