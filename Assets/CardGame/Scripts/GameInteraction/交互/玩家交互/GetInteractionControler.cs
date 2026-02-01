using System;
using System.Collections.Generic;
/// <summary>
/// 获取UI控制
/// </summary>
public interface  GetInteractionControler
{
    public string GetKey();
    public List<UIItemBinder> GetUI(MapLoader map,CardModel beObj,CardModel me);
    public bool IsSat(CardModel beObj,CardModel me);
    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me);
    public InteractionType GetInteractionType();
}

public interface GetWorldMapInteraction
{
    public string GetKey();
    public List<UIItemBinder> GetUI(CardModel beObj,CardModel me);
    // public List<AIBehave> GetDecision(CardModel beObj, CardModel me);
    public bool IsSat(CardModel beObj,CardModel me);
    public void AfterProcess(CardModel beObj, CardModel me);
    public InteractionType GetInteractionType();
}

public static class CardModelExtend
{
    public static List<GetWorldMapInteraction> worldMapInfos = new()
    {
        new GoToWorldInteraction(),
        new SleepInteractionControler(),
        new AwakeInteractionControler(),
        new EatInteractionControler(),
        new StopEatInteractionControler(),
        new GetJobInteractionControler(),
        // new StartReadBookInteractionControler(),
        // new EndReadBookInteractionControler(),
        new GetDetailInteractionControler(),
        new EndWorkInteractionControler(),
        new StartWorkInteractionControler(),
        new ChatInteractionControler(),
    };
    public static List<GetInteractionControler> infos = new()
    {
        new SeeInteractionControler(),
        new AttackInteractionControler(),
        new MoveInteractionControler(),
        new SleepInteractionControler(),
        new AwakeInteractionControler(),
        new EatInteractionControler(),
        new StopEatInteractionControler(),
        new GetJobInteractionControler(),
        // new StartReadBookInteractionControler(),
        // new EndReadBookInteractionControler(),
        new GetDetailInteractionControler(),
        new EndWorkInteractionControler(),
        new StartWorkInteractionControler(),
    };
    public static List<UIItemBinder> GetMapUI(this CardModel me,MapLoader map, CardModel beObj)
    {
        var ret = new List<UIItemBinder>();
        foreach (var info in infos)
        {
            if (info.IsSat(beObj,me))
            {
                var obj = info.GetUI(map,beObj,me);
                if (obj!=null)
                {
                    ret.AddRange(obj);
                }
            }
        }
        return ret;
    }

    public static List<UIItemBinder> GetWorldMapUI(this CardModel me, CardModel beObj)
    {
        var ret = new List<UIItemBinder>();
        foreach (var info in worldMapInfos)
        {
            if (info.IsSat(beObj,me))
            {
                var obj = info.GetUI(beObj,me);
                if (obj!=null)
                {
                    ret.AddRange(obj);
                }
            }
        }
        return ret;
    }
}