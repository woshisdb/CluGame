using System.Collections.Generic;

public static class AIInteractionRoot
{
    public static List<IAIMapInteraction> AIMapInteractions = new()
    {
        
    };
    public static List<IAISpaceInteraction> AISpaceInteractions = new()
    {
        new AttackItAISpaceInteraction(),
        new EatFoodAISpaceInteraction(),
        new PlayAISpaceInteraction(),
        new ReadBookAISpaceInteraction(),
        new SleepUntilEndAISpaceInteraction(),
        new StudyAISpaceInteraction(),
        new WorkUntilEndAISpaceInteraction(),
        new TryFindJobAISpaceInteraction(),
        new SocialAISpaceInteraction(),
    };

    public static List<(string,AISustainBehave)> GetWorldMapBehave(SpaceCardModel space,NpcCardModel npc)
    {
        var ret = new List<(string,AISustainBehave)>();
        foreach (var x in AISpaceInteractions)
        {
            if (x.IsSat(space,npc))
            {
                ret.Add((x.GetStr(space,npc),x.GetAISustainBehave(space, npc)));
            }
        }
        return ret;
    }
    public static List<(string,AISustainBehave)> GetMapBehave(MapLoader map,SpaceCardModel space,NpcCardModel npc)
    {
        var ret = new List<(string,AISustainBehave)>();
        foreach (var x in AIMapInteractions)
        {
            if (x.IsSat(map,space,npc))
            {
                ret.Add((x.GetStr(map,space,npc),x.GetAISustainBehave(map,space, npc)));
            }
        }
        return ret;
    }
}