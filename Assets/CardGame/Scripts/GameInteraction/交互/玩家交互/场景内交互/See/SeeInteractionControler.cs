using System.Collections.Generic;

/// <summary>
/// 观察的行为
/// </summary>
public class SeeInteractionControler:GetInteractionControler
{
    public string GetKey()
    {
        return "查看";
    }

    public List<UIItemBinder> GetUI(MapLoader map,CardModel beObj,CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var del = new SeeMapBehave(map,beObj.GetComponent<SeeComponent>(),me.GetComponent<CanBeSeeComponent>());
            del.Run();
            AfterProcess(map,beObj,me);
        }));
        return ret;
    }

    public bool IsSat(CardModel beObj,CardModel me)
    {
        return me.IsSatComponent<CanBeSeeComponent>() && beObj.IsSatComponent<SeeComponent>();
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.See;
    }
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        if (beObj is NpcCardModel)
        {
            var obj = beObj as NpcCardModel;
            obj.BehavePointComponent.ReducePoint(1);
        }
    }
}