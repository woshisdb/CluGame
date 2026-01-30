using System.Threading.Tasks;
using JetBrains.Annotations;

/// <summary>
/// 直至睡醒的行为
/// </summary>
public class SleepUntilEndAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "能在这里睡觉";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        return "能在这里睡觉";
    }

    public string GetKey()
    {
        return "睡觉";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        var self = space.GetComponent<SleepProviderComponent>();
        var other = npc.GetComponent<NeedSleepComponent>();
        if (self!=null&&other!=null)
        {
            var supply = space.GetComponent<SleepProviderComponent>().GetSupply();
            var consume = npc.GetComponent<NeedSleepComponent>().GetConsume();
            return (supply.CanUse(other) && consume.CanUse(self));
        }
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        return new SleepUntilEndBehave(space, npc as NpcCardModel);
    }
}

public class SleepUntilEndBehave:AISustainBehave
{
    private SpaceCardModel space;
    public int time = 10;
    public SleepUntilEndBehave(SpaceCardModel space, NpcCardModel npc):base()
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
        var other = npc.GetComponent<NeedSleepComponent>();
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
        var self = space.GetComponent<SleepProviderComponent>();
        var other = npc.GetComponent<NeedSleepComponent>();
        other.GetConsume().StartUse(self);
        npc.BehavePointComponent.ReducePoint(1);
        return Task.CompletedTask;
    }
}