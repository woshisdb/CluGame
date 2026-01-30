using System.Collections.Generic;

public class EndWorkInteractionControler: GetInteractionControler, GetWorldMapInteraction
{
    public string GetKey()
    {
        return "停止工作";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var sleep = beObj.GetComponent<NeedJobComponent>();
            sleep.GetConsume().EndUse();
            AfterProcess(beObj,me);
        }));
        return ret;
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (beObj.IsSatComponent<NeedJobComponent>())
        {
            var needSleepComponent = beObj.GetComponent<NeedJobComponent>();
            if (needSleepComponent.IsWorking())
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
        AfterProcess(beObj,me);
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.EndWork;
    }
}