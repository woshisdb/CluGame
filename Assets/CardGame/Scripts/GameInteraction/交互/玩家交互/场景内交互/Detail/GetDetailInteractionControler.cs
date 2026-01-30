using System.Collections.Generic;

public class GetDetailInteractionControler:GetInteractionControler,GetWorldMapInteraction
{
    public string GetKey()
    {
        return "详情";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var uidata = new List<UIItemBinder>();
            foreach (var x in me.GetComponents<IGetDetail>())
            {
                var cont = x.GetDetail();
                uidata.Add(new TableItemBinder(() =>
                {
                    return x.GetName();
                }, x.GetDetail()));
            }
            GameFrameWork.Instance.UIManager.ObjectDetailPanel.ShowPanel(uidata);
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
        return true;
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        if (beObj is NpcCardModel)
        {
            var obj = beObj as NpcCardModel;
            obj.BehavePointComponent.ReducePoint(0);
        }
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.Detail;
    }
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        AfterProcess(beObj, me);
    }
}