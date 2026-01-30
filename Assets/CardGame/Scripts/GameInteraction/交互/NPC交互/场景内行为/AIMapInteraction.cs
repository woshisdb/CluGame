public interface IAIMapInteraction
{
    public string GetStr(MapLoader map,SpaceCardModel space,CardModel npc);
    public string GetKey();
    public bool IsSat(MapLoader map,SpaceCardModel space,CardModel npc);
    public AISustainBehave GetAISustainBehave(MapLoader map,SpaceCardModel space,CardModel npc);
}