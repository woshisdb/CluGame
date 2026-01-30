using System.Collections.Generic;

public class GoToWorldInteraction:GetWorldMapInteraction
{
    public string GetKey()
    {
        return "前往";
    }

    public List<UIItemBinder> GetUI(CardModel beObj, CardModel me)
    {
        var npc = beObj as NpcCardModel;
        var ret = new List<UIItemBinder>();
        foreach (var x in me.GetComponent<PathComponent>().PathInfos)
        {
            var wayCfg = x;
            ret.Add(
                new ButtonBinder(() => { return "前往"+wayCfg.CardModel.GetTitle(); }, () =>
                {//GameFrameWork.Instance.playerManager.nowName
                    GameFrameWork.Instance.GoToSpace(new GoToSpaceCardDelegate(
                    wayCfg.CardModel as SpaceCardModel,
                    wayCfg.wasterTime,
                    npc.npcId
                ));
            }));
        }
        return ret;
    }
    public bool IsSat(CardModel beObj, CardModel me)
    {
        var space = me as SpaceCardModel;
        if (space!=null)
        {
            if (beObj.GetComponent<BelongComponent>().belong.Value == space)
            {
                return true;
            } 
        }
        return false;
    }

    public void AfterProcess(CardModel beObj, CardModel me)
    {
        
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.GotoSpace;
    }
}