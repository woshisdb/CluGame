using System.Collections.Generic;

public class WorldMapSystem
{
    /// <summary>
    /// 一系列的场景,--------
    /// </summary>
    public List<SpaceCardModel> Spaces
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.Space;
        }
    }

    // public List<AISpaceDecisionSet> GetAllDecisions(NpcCardModel npc)
    // {
    //     var ret = new List<AISpaceDecisionSet>();
    //     foreach (var space in Spaces)
    //     {
    //         var desc = space.GetWorldMapAIBehave(npc);
    //         if (desc.Count>0)
    //         {
    //             ret.Add(new AISpaceDecisionSet(space,desc));
    //         }
    //     }
    //
    //     return ret;
    // }
}