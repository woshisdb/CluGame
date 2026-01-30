using System.Collections.Generic;
using UnityEngine;

public class MoveInteractionControler:GetInteractionControler
{
    public string GetKey()
    {
        return "移动";
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return "前往";
        }, () =>
        {
            var cell = me as CellModel;
            var npc = beObj as NpcCardModel;
            var behave = new RunMapBehave(npc,map);
            behave.start = (npc.GetFrom() as CellModel).CellView;
            behave.aim = cell.CellView;
            behave.Run();
            AfterProcess(map,beObj,me);
        }));
        return ret;
    }

    // public List<AIBehave> GetDecision(MapLoader map, CardModel beObj, CardModel me)
    // {
    //     var ret = new List<AIBehave>();
    //     var retData = new Dictionary<string,CardModel>();
    //     var cell = me as CellModel;
    //     var npc = beObj as NpcCardModel;
    //     var moveBehave = new AIBehave(null, null, true,null,null).SetEndAction(e =>
    //     {
    //         var behave = new RunMapBehave(npc,map);
    //         behave.start = ((CellModel)(npc.GetFrom())).CellView;
    //         behave.aim = cell.CellView;
    //         behave.Run();
    //         AfterProcess(map,beObj,me);
    //     }).SetRetData(retData);
    //     ret.Add(moveBehave);
    //     return ret;
    // }

    public bool IsSat(CardModel beObj, CardModel me)
    {
        if (!(me is CellModel && beObj is NpcCardModel))
        {
            return false;
        }
        var cell = me as CellModel;
        var npc = beObj as NpcCardModel;
        if (cell.ContainNpc(npc.npcId))
        {
            //不为空则不能去
            return false;
        }
        else
        {
            //为空则可以去
            return true;
        }
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.Move;
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