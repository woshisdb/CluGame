public class EatFoodComponent:BaseComponent,ISupplyTask,IAfterPlayerNpcLogic
{
    /// <summary>
    /// 每回合减少的精力水平
    /// </summary>
    public int ReduceEnergy;
    /// <summary>
    /// 每回合增加的精力水平
    /// </summary>
    public int AddEnergy;
    /// <summary>
    /// 剩余的精力水平
    /// </summary>

    public ContrainInt RemainEnergy;
    public ConsumeModule<SupplyFoodComponent> Consume;
    public EatFoodComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        var c = creator as EatFoodComponentCreator;
        this.AddEnergy = c.AddEnergy;
        this.ReduceEnergy = c.ReduceEnergy;
        RemainEnergy = new ContrainInt(c.MaxEnergy,c.MaxEnergy,0);
        Consume = new ConsumeModule<SupplyFoodComponent>(cardModel, e =>
        {
            return cardModel.GetComponent<NowTaskComponent>().IsFree();
        }, e =>
        {
            CardModel.GetComponent<NowTaskComponent>().SetTask(this);//配置当前任务
            e.GetSupply().StartUse(this);
        }, e =>
        {
            CardModel.GetComponent<NowTaskComponent>().ReleaseTask(this);//配置当前任务
            e.GetSupply().EndUse(this);
        });
    }
    public void WhenEating()
    {
        var rate = Consume.belong.Rate;
        RemainEnergy.Add((int)(AddEnergy * rate));
    }

    public void WhenNotEating()
    {
        RemainEnergy.Reduce((int)(ReduceEnergy));
    }

    public bool IsEating()
    {
        if (CardModel.GetComponent<NowTaskComponent>().IsFree())
        {
            return false;
        }
        else
        {
            if (CardModel.GetComponent<NowTaskComponent>().SupplyTask == this)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public SupplyTaskType TaskType {
        get
        {
            return SupplyTaskType.EatFood;
        }
    }

    public ConsumeModule<SupplyFoodComponent> GetConsume()
    {
        return Consume;
    }

    public void OnAfterPlayerNpcLogic()
    {
        if (IsEating())
        {
            WhenEating();
        }
        else
        {
            WhenNotEating();
        }
    }
}

public class EatFoodComponentCreator:BaseComponentCreator<EatFoodComponent>
{
    /// <summary>
    /// 每回合减少的精力水平
    /// </summary>
    public int ReduceEnergy;
    /// <summary>
    /// 每回合增加的精力水平
    /// </summary>
    public int AddEnergy;

    public int MaxEnergy;
    public override ComponentType ComponentName()
    {
        return ComponentType.EatFoodComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new EatFoodComponent(cardModel, this);
    }
}