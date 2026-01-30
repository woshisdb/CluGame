using System.Collections.Generic;

public class StartWorkInteractionControler: GetInteractionControler, GetWorldMapInteraction
{
    public string GetKey()
    {
        return "开始工作";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var self = me.GetComponent<ProvideJobComponent>();
        var other = beObj.GetComponent<NeedJobComponent>();
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

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if(me.IsSatComponent<ProvideJobComponent>() && beObj.IsSatComponent<NeedJobComponent>())
        {
            var self = me.GetComponent<ProvideJobComponent>();
            var other = beObj.GetComponent<NeedJobComponent>();
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

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        return GetUI(beObj, me);
    }

    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        AfterProcess(beObj, me);
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.StartWork;
    }
}