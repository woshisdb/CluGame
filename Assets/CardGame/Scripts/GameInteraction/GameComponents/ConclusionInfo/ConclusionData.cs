
using System.Collections.Generic;
using System.Text;

public enum ConclusionEnum
{
    None = 0,
    SpaceInteractionConclusion = 1,
}
public abstract class ConclusionData
{
    public abstract string GetConclusion(SpaceCardModel space,NpcCardModel npc);
}

public static class ConclusionMap
{
    public static Dictionary<ConclusionEnum, ConclusionData> cmap = new()
    {
        { ConclusionEnum.None, new NoneConclusionData() },
        { ConclusionEnum.SpaceInteractionConclusion ,new SpaceConclusionData()},
    };
}

public class SpaceConclusionData : ConclusionData
{
    public override string GetConclusion(SpaceCardModel space,NpcCardModel npc)
    {
        var title = (space.cfg.Value as SpaceCardConfig).title;
        var strb = new StringBuilder();
        strb.Append("地点名:");
        strb.Append(title);
        strb.Append(".场景描述:");
        strb.Append(space.GetComponent<ConclusionComponent>().conclusionStr);
        strb.Append(".你可以在这个地点做的事情:");
        ///当前的可以执行的行为
        foreach (var x in AIInteractionRoot.AISpaceInteractions)
        {
            if (x.IsSat(space,npc))
            {
                strb.Append(x.GetStr(space,npc));
                strb.Append(",");
            }
        }

        return strb.ToString();
    }
}

public class NoneConclusionData : ConclusionData
{
    public override string GetConclusion(SpaceCardModel space,NpcCardModel npc)
    {
        var title = (space.cfg.Value as SpaceCardConfig).title;
        return "地点名:" + title + ",场景描述:" + space.GetComponent<ConclusionComponent>().conclusionStr;
    }
}