using System.Collections.Generic;

public class GetJobInteractionControler:GetInteractionControler,GetWorldMapInteraction
{
    public string GetKey()
    {
        return "获取工作";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        var need = beObj.GetComponent<NeedJobComponent>();
        var provide = me.GetComponent<ProvideJobComponent>();
        foreach (var x in provide.ProvideJobItem)
        {
            if (x.CanGetJob(need))
            {
                ret.Add(new ButtonBinder(() =>
                {
                    return GetKey();
                }, () =>
                {
                    x.GetJob(need);
                    need.GetJob(x);
                    AfterProcess(beObj,me);
                }));
            }
        }
        return ret;
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        return GetUI(beObj, me);
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (beObj.IsSatComponent<NeedJobComponent>()&&me.IsSatComponent<ProvideJobComponent>())
        {
            var need = beObj.GetComponent<NeedJobComponent>();
            var provide = me.GetComponent<ProvideJobComponent>();
            if (need.HasJob()&&provide.HasEmptyJob(need))
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
        return InteractionType.GetJob;
    }
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        AfterProcess(beObj, me);
    }
}