using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;


/// <summary>
/// 提供职业的，例如警局
/// </summary>
public class NeedJobComponent:BaseComponent,ISupplyTask,IConsume<ProvideJobComponent>
{
    public ProvideJobItem ProvideJobItem;
    public ConsumeModule<ProvideJobComponent> Consume;
    public NeedJobComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        Consume = new ConsumeModule<ProvideJobComponent>(cardModel, e =>
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

    public bool HasJob()
    {
        return ProvideJobItem!=null;
    }

    public bool IsWorking()
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
    public void GetJob(ProvideJobItem job)
    {
        ProvideJobItem = job;
    }
    public void ReleaseJob()
    {
        ProvideJobItem = null;
    }

    public SupplyTaskType TaskType { get; }
    public ConsumeModule<ProvideJobComponent> GetConsume()
    {
        return Consume;
    }
}

public class NeedJobComponentCreator:BaseComponentCreator<NeedJobComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.NeedJobComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NeedJobComponent(cardModel, this);
    }
}