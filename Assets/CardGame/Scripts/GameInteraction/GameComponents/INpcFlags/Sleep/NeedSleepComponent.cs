using System.Collections.Generic;

/// <summary>
/// 需要睡觉的
/// </summary>
public class NeedSleepComponent:BaseComponent,ISupplyTask,IConsume<SleepProviderComponent>,IAfterPlayerNpcLogic,INeed
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
    public ConsumeModule<SleepProviderComponent> Consume;
    public NeedSleepComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        var c = creator as NeedSleepComponentCreator;
        this.AddEnergy = c.AddEnergy;
        this.ReduceEnergy = c.ReduceEnergy;
        RemainEnergy = new ContrainInt(c.MaxEnergy,c.MaxEnergy,0);
        Consume = new ConsumeModule<SleepProviderComponent>(cardModel, e =>
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
    public bool IsSleep()
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
    
    public int GetRemainEnergy()
    {
        return RemainEnergy.nowVal;
    }
    public void WhenSleep()
    {
        var rate = Consume.belong.Rate;
        RemainEnergy.Add((int)(AddEnergy * rate));
    }

    public void WhenAwake()
    {
        RemainEnergy.Reduce((int)(ReduceEnergy));
    }

    public SupplyTaskType TaskType {
        get
        {
            return SupplyTaskType.Sleep;
        }
    }

    public ConsumeModule<SleepProviderComponent> GetConsume()
    {
        return Consume;
    }

    public void OnAfterPlayerNpcLogic()
    {
        if (IsSleep())
        {
            WhenSleep();
        }
        else
        {
            WhenAwake();
        }
    }

    public string SatLevel()
    {
        var nowval = RemainEnergy.nowVal;
        var maxVal = RemainEnergy.maxVal;
        if (nowval>=maxVal/2)//当前值超一半
        {
            return "比较满足";
        }
        else
        {
            return "非常困";
        }
    }

    public string NeedDescription()
    {
        return "对休息，睡觉的需求";
    }

    public string NeedName()
    {
        return "休息需求";
    }

    public List<string> GetWhyNotSat()
    {
        return new List<string>(){"需要睡觉"};
    }
}

public class NeedSleepComponentCreator : BaseComponentCreator<NeedSleepComponent>
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
        return ComponentType.SleepComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NeedSleepComponent(cardModel, this);
    }

    public override bool NeedComponent(List<IComponentCreator> components)
    {
        return components.Exists(e =>
        {
            return e is NowTaskComponentCreator;
        });
    }
}