using System.Threading.Tasks;

/// <summary>
/// 工作到任务结束
/// </summary>
public class WorkUntilEndAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "如果有工作，则在这工作";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        return "如果有工作，则在这工作";
    }

    public string GetKey()
    {
        return "工作";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        var eatFood = new WorkFoodUntilEndBehave(space,npc as NpcCardModel);
        return eatFood;
    }
}
public class WorkFoodUntilEndBehave:AISustainBehave
{
    private SpaceCardModel space;
    public int time = 10;
    public WorkFoodUntilEndBehave(SpaceCardModel space, NpcCardModel npc):base()
    {
        this.npc = npc;
        this.space = space;
    }
    public override Task<AIBehave> GetNowBehave()
    {
        return Task.FromResult<AIBehave>(null);
    }

    public override bool IsComplete()
    {
        time--;
        if (time<=0)
        {
            return true;
        }

        return false;
    }

    public override void OnComplete()
    {
        var other = npc.GetComponent<NeedJobComponent>();
        other.GetConsume().EndUse();
        npc.BehavePointComponent.ReducePoint(1);
        return;
    }

    public override void OnBreak()
    {
        throw new System.NotImplementedException();
    }

    public override bool CanStart()
    {
        return true;
    }

    public override Task Start()
    {
        var self = space.GetComponent<ProvideJobComponent>();
        var other = npc.GetComponent<NeedJobComponent>();
        other.GetConsume().StartUse(self);
        npc.BehavePointComponent.ReducePoint(1);
        return Task.CompletedTask;
    }
}