using System.Collections.Generic;

public class StopEatInteractionControler:GetInteractionControler,GetWorldMapInteraction
{
    public string GetKey()
    {
        return "醒来";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var sleep = beObj.GetComponent<EatFoodComponent>();
            sleep.GetConsume().EndUse();
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
        if (beObj.IsSatComponent<EatFoodComponent>())
        {
            var needSleepComponent = beObj.GetComponent<EatFoodComponent>();
            if (needSleepComponent.IsEating())
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
        return InteractionType.StopEat;
    }
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        AfterProcess(beObj, me);
    }
}