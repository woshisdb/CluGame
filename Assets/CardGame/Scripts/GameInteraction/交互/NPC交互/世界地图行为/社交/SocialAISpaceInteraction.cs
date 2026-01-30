using System.Collections.Generic;
/// <summary>
/// 社交行为
/// </summary>
public class SocialAISpaceInteraction:IAISpaceInteraction
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