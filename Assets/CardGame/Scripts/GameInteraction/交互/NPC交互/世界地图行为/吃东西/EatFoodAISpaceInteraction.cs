using System.Threading.Tasks;

public class EatFoodAISpaceInteraction:IAISpaceInteraction
{
    public string GetStr(SpaceCardModel space, CardModel npc)
    {
        return "去吃饭";
    }

    public string GetDetailStr(SpaceCardModel space, CardModel npc)
    {
        return "去吃饭";
    }

    public string GetKey()
    {
        return "吃饭";
    }

    public bool IsSat(SpaceCardModel space, CardModel npc)
    {
        if(space.IsSatComponent<SupplyFoodComponent>() && npc.IsSatComponent<EatFoodComponent>())
        {
            var self = space.GetComponent<SupplyFoodComponent>();
            var other = npc.GetComponent<EatFoodComponent>();
            var supply = self.GetSupply();
            var consume = other.GetConsume();
            if (supply.CanUse(other)&&consume.CanUse(self))
            {
                return true;
            }
        }
        return false;
    }

    public AISustainBehave GetAISustainBehave(SpaceCardModel space, CardModel npc)
    {
        var eatFood = new EatFoodUntilEndBehave(space,npc as NpcCardModel);
        return eatFood;
    }
}

public class EatFoodUntilEndBehave:AISustainBehave
{
    private SpaceCardModel space;
    public int time = 10;
    public EatFoodUntilEndBehave(SpaceCardModel space, NpcCardModel npc):base()
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
        var other = npc.GetComponent<EatFoodComponent>();
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
        var self = space.GetComponent<SupplyFoodComponent>();
        var other = npc.GetComponent<EatFoodComponent>();
        other.GetConsume().StartUse(self);
        npc.BehavePointComponent.ReducePoint(1);
        return Task.CompletedTask;
    }
}