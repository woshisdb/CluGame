using System.Collections.Generic;

public class NowTaskComponent:BaseComponent,IGetDetail
{
    /// <summary>
    /// 当前任务是什么
    /// </summary>
    public ISupplyTask SupplyTask;

    public void SetTask(ISupplyTask task)
    {
        this.SupplyTask = task;
    }

    public void ReleaseTask(ISupplyTask task)
    {
        if (SupplyTask==task)
        {
            SupplyTask = null;
        }
    }
    public NowTaskComponent(CardModel cardModel, IComponentCreator creator) : base(cardModel, creator)
    {
        
    }

    public bool IsFree()
    {
        return SupplyTask == null;
    }
    public SupplyTaskType GetTaskType()
    {
        if (IsFree())
        {
            return SupplyTaskType.Free;
        }
        else
        {
            return SupplyTask.TaskType;
        }
    }

    public string GetName()
    {
        return "当前任务";
    }

    public List<UIItemBinder> GetDetail()
    {
        var ret = new List<UIItemBinder>();
        ret.Add(new KVItemBinder(() =>
        {
            return "当前状态";
        }, () =>
        {
            return GetTaskType().ToString();
        }));
        return ret;
    }
}

public class NowTaskComponentCreator:BaseComponentCreator<NowTaskComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.NowTaskComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NowTaskComponent(cardModel, this);
    }
}