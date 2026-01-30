using System.Threading.Tasks;

public class ReadBookAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "选择书籍并阅读";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        return "选择并阅读书籍";
    }

    public string GetKey()
    {
        return "阅读";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        return new ReadBookAISequenceBehave(space, npc as NpcCardModel);
    }
}

public class ReadBookAISequenceBehave : AISustainBehave
{
    private SpaceCardModel space;
    public ReadBookAISequenceBehave(SpaceCardModel space, NpcCardModel npc)
    {
        this.space = space;
        this.npc = npc;
    }
    public override Task<AIBehave> GetNowBehave()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsComplete()
    {
        throw new System.NotImplementedException();
    }

    public override void OnComplete()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBreak()
    {
        throw new System.NotImplementedException();
    }

    public override bool CanStart()
    {
        throw new System.NotImplementedException();
    }

    public override Task Start()
    {
        throw new System.NotImplementedException();
    }
}