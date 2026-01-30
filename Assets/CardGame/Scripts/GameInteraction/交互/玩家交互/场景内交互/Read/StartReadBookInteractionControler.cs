using System.Collections.Generic;

/// <summary>
/// 读书的控制器
/// </summary>
public class StartReadBookInteractionControler:GetInteractionControler,GetWorldMapInteraction
{
    public string GetKey()
    {
        return "阅读";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        return null;
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            var del = new ReadMapBehave(map,beObj.GetComponent<SeeComponent>(), me.GetComponent<BookComponent>());
            del.Run();
            AfterProcess(map,beObj,me);
        }));
        return ret;
    }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (beObj.IsSatComponent<SeeComponent>() && me.IsSatComponent<BookComponent>())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        throw new System.NotImplementedException();
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.ReadBook;
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