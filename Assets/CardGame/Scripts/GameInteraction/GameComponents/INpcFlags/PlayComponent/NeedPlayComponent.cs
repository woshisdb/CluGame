using System.Collections.Generic;

/// <summary>
/// 需要娱乐
/// </summary>
public class NeedPlayComponent : BaseComponent,
    ISupplyTask,
    IConsume<ProvidePlayComponent>,
    IAfterPlayerNpcLogic,
    INeed
{
    /// <summary>
    /// 每回合减少的娱乐值
    /// </summary>
    public int ReduceFun;

    /// <summary>
    /// 每回合增加的娱乐值
    /// </summary>
    public int AddFun;

    /// <summary>
    /// 当前娱乐值
    /// </summary>
    public ContrainInt RemainFun;

    public ConsumeModule<ProvidePlayComponent> Consume;

    public NeedPlayComponent(CardModel cardModel, IComponentCreator creator)
        : base(cardModel, creator)
    {
        var c = creator as NeedPlayComponentCreator;

        AddFun = c.AddFun;
        ReduceFun = c.ReduceFun;

        RemainFun = new ContrainInt(c.MaxFun, c.MaxFun, 0);

        Consume = new ConsumeModule<ProvidePlayComponent>(
            cardModel,
            e =>
            {
                // 只有在空闲状态才能去娱乐
                return cardModel.GetComponent<NowTaskComponent>().IsFree();
            },
            e =>
            {
                // 占用当前任务
                CardModel.GetComponent<NowTaskComponent>().SetTask(this);
                e.GetSupply().StartUse(this);
            },
            e =>
            {
                // 释放任务
                CardModel.GetComponent<NowTaskComponent>().ReleaseTask(this);
                e.GetSupply().EndUse(this);
            }
        );
    }

    public bool IsPlaying()
    {
        var task = CardModel.GetComponent<NowTaskComponent>();
        return !task.IsFree() && task.SupplyTask == this;
    }

    public int GetRemainFun()
    {
        return RemainFun.nowVal;
    }

    public void WhenPlay()
    {
        var rate = Consume.belong.Rate;
        RemainFun.Add((int)(AddFun * rate));
    }

    public void WhenBored()
    {
        RemainFun.Reduce(ReduceFun);
    }

    public SupplyTaskType TaskType
    {
        get { return SupplyTaskType.Play; }
    }

    public ConsumeModule<ProvidePlayComponent> GetConsume()
    {
        return Consume;
    }

    public void OnAfterPlayerNpcLogic()
    {
        if (IsPlaying())
        {
            WhenPlay();
        }
        else
        {
            WhenBored();
        }
    }

    // ================= INeed =================

    public string SatLevel()
    {
        var now = RemainFun.nowVal;
        var max = RemainFun.maxVal;

        if (now >= max / 2)
        {
            return "心情不错";
        }
        else
        {
            return "无聊透顶";
        }
    }

    public string NeedDescription()
    {
        return "对娱乐、消遣的需求";
    }

    public string NeedName()
    {
        return "娱乐需求";
    }

    public List<string> GetWhyNotSat()
    {
        return new List<string> { "需要娱乐" };
    }
}

public class NeedPlayComponentCreator : BaseComponentCreator<NeedPlayComponent>
{
    /// <summary>
    /// 每回合减少的娱乐值
    /// </summary>
    public int ReduceFun;

    /// <summary>
    /// 每回合增加的娱乐值
    /// </summary>
    public int AddFun;

    /// <summary>
    /// 最大娱乐值
    /// </summary>
    public int MaxFun;

    public override ComponentType ComponentName()
    {
        return ComponentType.PlayComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NeedPlayComponent(cardModel, this);
    }

    public override bool NeedComponent(List<IComponentCreator> components)
    {
        return components.Exists(e => e is NowTaskComponentCreator);
    }
}
