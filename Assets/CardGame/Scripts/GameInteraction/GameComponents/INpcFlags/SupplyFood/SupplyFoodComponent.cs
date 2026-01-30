using System.Collections.Generic;

public class SupplyFoodComponent:BaseComponent,ISupply<EatFoodComponent>,IGetDetail
{
    public float Rate;
    public SupplyModule<EatFoodComponent> Provider;
    public SupplyFoodComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        var ctx = creator as SupplyFoodComponentCreator;
        Rate = ctx.Rate;
        Provider = new SupplyModule<EatFoodComponent>(cardModel,ctx.sum, e =>
        {
            return true;
        }, (e) =>
        {
            
        }, (e) =>
        {
            
        });
    }

    public SupplyModule<EatFoodComponent> GetSupply()
    {
        return Provider;
    }

    public string GetName()
    {
        return "供应食物";
    }

    public List<UIItemBinder> GetDetail()
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new KVItemBinder(() =>
        {
            return "质量";
        }, () =>
        {
            return Rate.ToString();
        }));
        ret.Add(new KVItemBinder(() =>
        {
            return "数量";
        }, () =>
        {
            return Provider.res.Count + "/" + Provider.MaxNum;
        }));
        return ret;
    }
}

public class SupplyFoodComponentCreator : BaseComponentCreator<SupplyFoodComponent>
{
    public int sum;
    public float Rate;
    public override ComponentType ComponentName()
    {
        return ComponentType.SupplyFoodComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new SupplyFoodComponent(cardModel, this);
    }
}