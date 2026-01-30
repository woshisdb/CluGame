using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 尝试找工作行为
/// </summary>
public class TryFindJobAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "如果没工作或对工作不满，可以在这找工作";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        throw new System.NotImplementedException();
    }

    public string GetKey()
    {
        return "找工作";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        return new TryFindJobAIBehave(space, npc as NpcCardModel);
    }
}
/// <summary>
/// 去找工作
/// </summary>
public class TryFindJobAIBehave : AISustainBehave
{
    private SpaceCardModel space;
    public TryFindJobAIBehave(SpaceCardModel space, NpcCardModel npc)
    {
        this.space = space;
        this.npc = npc;
    }
    public override Task<AIBehave> GetNowBehave()
    {
        return Task.FromResult<AIBehave>(null);
    }

    public override bool IsComplete()
    {
        return true;
    }

    public override void OnComplete()
    {
        var givenJob = space.GetComponent<ProvideJobComponent>();
        var needJob = npc.GetComponent<NeedJobComponent>();
        if (givenJob.HasEmptyJob(needJob))
        {
            givenJob.RequestJob(needJob);
        }
        return;
    }

    public override void OnBreak()
    {
        return;
    }

    public override bool CanStart()
    {
        return true;
    }

    public override Task Start()
    {
        return Task.CompletedTask;
    }
}