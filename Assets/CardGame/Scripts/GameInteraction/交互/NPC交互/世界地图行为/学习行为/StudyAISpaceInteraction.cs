using System.Collections.Generic;
/// <summary>
/// 学习序列行为
/// </summary>
public class StudyAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "学习增长知识";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        return "学习增长知识";
    }

    public string GetKey()
    {
        return "学习";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        return null;
    }
}