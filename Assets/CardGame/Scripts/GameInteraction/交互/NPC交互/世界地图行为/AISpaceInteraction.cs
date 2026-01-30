public interface IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space,CardModel npc);
    public string GetDetailStr(SpaceCardModel space,CardModel npc);
    public string GetKey();
    public bool IsSat(SpaceCardModel space,CardModel npc);
    public AISustainBehave GetAISustainBehave(SpaceCardModel space,CardModel npc);
}
