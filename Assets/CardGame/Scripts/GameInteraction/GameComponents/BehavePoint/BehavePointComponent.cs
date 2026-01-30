using System;

/// <summary>
/// 行动点管理模块
/// </summary>
public class BehavePointComponent:BaseComponent
{
    public int maxPointNum =1;
    public int pointNum;
    public Action AfterBehave;
    public void AddPoint(int num =1)
    {
        pointNum = Math.Min(pointNum+num,maxPointNum);
    }

    public void ReducePoint(int num)
    {
        pointNum = Math.Max(pointNum - num, 0);
        // if (pointNum ==0)
        // {
        //     AfterBehave?.Invoke();
        // }
    }

    public void SetAfterBehave(Action AfterBehave)
    {
        this.AfterBehave = AfterBehave;
    }

    public bool IsSatBehavePoint(int num = 1)
    {
        return pointNum >= num;
    }
    
    public void FillPoint()
    {
        pointNum = maxPointNum;
    }

    public void EmptyPoint()
    {
        pointNum = 0;
    }

    public BehavePointComponent(CardModel card,BehavePointComponentCreator creator) : base(card,creator)
    {
        this.maxPointNum = creator.maxPointNum;
        pointNum = maxPointNum;
    }
}
public class BehavePointComponentCreator:BaseComponentCreator<BehavePointComponent>
{
    public int maxPointNum =1;
    public override ComponentType ComponentName()
    {
        return ComponentType.BehavePointComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new BehavePointComponent(cardModel,this);
    }
}