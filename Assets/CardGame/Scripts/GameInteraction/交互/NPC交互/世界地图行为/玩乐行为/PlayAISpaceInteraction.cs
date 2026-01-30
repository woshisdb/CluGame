using System.Collections.Generic;
/// <summary>
/// 玩乐序列行为
/// </summary>
public class PlayAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        throw new System.NotImplementedException();
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        throw new System.NotImplementedException();
    }

    public string GetKey()
    {
        throw new System.NotImplementedException();
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        throw new System.NotImplementedException();
    }
}