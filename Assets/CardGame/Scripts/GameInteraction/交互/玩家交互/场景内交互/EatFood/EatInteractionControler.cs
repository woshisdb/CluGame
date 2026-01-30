using System.Collections.Generic;

public class EatInteractionControler:GetInteractionControler,GetWorldMapInteraction
{
    public string GetKey()
    {
        return "吃饭";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var self = me.GetComponent<SupplyFoodComponent>();
        var other = beObj.GetComponent<EatFoodComponent>();
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            other.GetConsume().StartUse(self);
            AfterProcess(beObj,me);
        }));
        return ret;
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        return GetUI(beObj, me);
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if(me.IsSatComponent<SupplyFoodComponent>() && beObj.IsSatComponent<EatFoodComponent>())
        {
            var self = me.GetComponent<SupplyFoodComponent>();
            var other = beObj.GetComponent<EatFoodComponent>();
            var supply = self.GetSupply();
            var consume = other.GetConsume();
            if (supply.CanUse(other)&&consume.CanUse(self))
            {
                return true;
            }
        }
        return false;
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        if (beObj is NpcCardModel)
        {
            var obj = beObj as NpcCardModel;
            obj.BehavePointComponent.ReducePoint(1);
        }
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.Sleep;
    }
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        AfterProcess(beObj, me);
    }
}