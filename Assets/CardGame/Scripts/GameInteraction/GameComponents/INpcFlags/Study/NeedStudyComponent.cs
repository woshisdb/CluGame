using System.Collections.Generic;

/// <summary>
/// 需要学习
/// </summary>
public class NeedStudyComponent : BaseComponent,
    ISupplyTask,
    IConsume<ProvideStudyComponent>,
    IAfterPlayerNpcLogic,
    INeed
{
    /// <summary>
    /// 每回合减少的学习满足度
    /// </summary>
    public int ReduceStudy;

    /// <summary>
    /// 每回合增加的学习满足度
    /// </summary>
    public int AddStudy;

    /// <summary>
    /// 当前学习满足度
    /// </summary>
    public ContrainInt RemainStudy;

    public ConsumeModule<ProvideStudyComponent> Consume;

    public NeedStudyComponent(CardModel cardModel, IComponentCreator creator)
        : base(cardModel, creator)
    {
        var c = creator as NeedStudyComponentCreator;

        ReduceStudy = c.ReduceStudy;
        AddStudy = c.AddStudy;

        RemainStudy = new ContrainInt(c.MaxStudy, c.MaxStudy, 0);

        Consume = new ConsumeModule<ProvideStudyComponent>(
            cardModel,
            e =>
            {
                // 只有空闲时才能去学习
                return cardModel.GetComponent<NowTaskComponent>().IsFree();
            },
            e =>
            {
                CardModel.GetComponent<NowTaskComponent>().SetTask(this);
                e.GetSupply().StartUse(this);
            },
            e =>
            {
                CardModel.GetComponent<NowTaskComponent>().ReleaseTask(this);
                e.GetSupply().EndUse(this);
            }
        );
    }

    public bool IsStudying()
    {
        var task = CardModel.GetComponent<NowTaskComponent>();
        return !task.IsFree() && task.SupplyTask == this;
    }

    public int GetRemainStudy()
    {
        return RemainStudy.nowVal;
    }

    public void WhenStudy()
    {
        var rate = Consume.belong.Rate;
        RemainStudy.Add((int)(AddStudy * rate));
    }

    public void WhenIdle()
    {
        RemainStudy.Reduce(ReduceStudy);
    }

    public SupplyTaskType TaskType
    {
        get { return SupplyTaskType.Study; }
    }

    public ConsumeModule<ProvideStudyComponent> GetConsume()
    {
        return Consume;
    }

    public void OnAfterPlayerNpcLogic()
    {
        if (IsStudying())
        {
            WhenStudy();
        }
        else
        {
            WhenIdle();
        }
    }

    // ================= INeed =================

    public string SatLevel()
    {
        var now = RemainStudy.nowVal;
        var max = RemainStudy.maxVal;

        if (now >= max / 2)
        {
            return "求知欲得到满足";
        }
        else
        {
            return "渴望学习";
        }
    }

    public string NeedDescription()
    {
        return "对学习、提升认知的需求";
    }

    public string NeedName()
    {
        return "学习需求";
    }

    public List<string> GetWhyNotSat()
    {
        return new List<string> { "需要学习" };
    }
}

public class NeedStudyComponentCreator : BaseComponentCreator<NeedStudyComponent>
{
    /// <summary>
    /// 每回合减少的学习满足度
    /// </summary>
    public int ReduceStudy;

    /// <summary>
    /// 每回合增加的学习满足度
    /// </summary>
    public int AddStudy;

    /// <summary>
    /// 最大学习满足度
    /// </summary>
    public int MaxStudy;

    public override ComponentType ComponentName()
    {
        return ComponentType.StudyComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NeedStudyComponent(cardModel, this);
    }

    public override bool NeedComponent(List<IComponentCreator> components)
    {
        return components.Exists(e => e is NowTaskComponentCreator);
    }
}
