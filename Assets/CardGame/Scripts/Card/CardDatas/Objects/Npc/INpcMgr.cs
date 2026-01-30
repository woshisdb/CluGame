using System.Collections.Generic;

public interface INpcMgr
{
    List<string> GetNpcs();
}

public static class INpcMgrExtent
{
    public static NpcCardModel FindNpc(this INpcMgr npcMgr, string npcName)
    {
        return GameFrameWork.Instance.data.saveFile.npcs.Find(e =>
        {
            return e.npcId == npcName;
        });
    }

    public static bool ContainNpc(this INpcMgr npcMgr,string npcName)
    {
        return npcMgr.GetNpcs().Contains(npcName);
    }
    public static bool ContainNpc(this INpcMgr npcMgr,NpcCardModel npc)
    {
        return npcMgr.GetNpcs().Contains(npc.npcId);
    }
}