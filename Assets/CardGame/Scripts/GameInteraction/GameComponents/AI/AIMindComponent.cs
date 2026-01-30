public class AIMindComponent:BaseComponent
{
    public AINpcMindCfg AINpcMindCfg;
    public bool isActivite;
    /// <summary>
    /// 最重要的需求
    /// </summary>
    public INeed importantNeed;
    /// <summary>
    /// 当前行为目标是什么
    /// </summary>
    public string myAim;
    /// <summary>
    /// 当前要前往的场景
    /// </summary>
    /// <param name="cardModel"></param>
    /// <param name="creator"></param>
    public SpaceCardModel aimSpace;
    /// <summary>
    /// 当前要交互的对象
    /// </summary>
    public CardModel aimTarget;

    public NPCNeedAILogic NPCNeedAILogic;
    /// <summary>
    /// 当前执行的行为
    /// </summary>
    public AISustainBehave nowBehave;
    public AIMindComponent(CardModel cardModel, AIMindComponentCreator creator) : base(cardModel, creator)
    {
        this.isActivite = creator.isActivite;
        NPCNeedAILogic = new NPCNeedAILogic(CardModel as NpcCardModel);
        AINpcMindCfg = creator.AINpcMindCfg;
    }

    public void ClearAIMind()
    {
        importantNeed = null;
        myAim = null;
        aimSpace = null;
    }
    public void SetNowBehave(AISustainBehave behave)
    {
        this.nowBehave = behave;
    }
    /// <summary>
    /// 正常结束任务
    /// </summary>
    public void FinishBehave()
    {
        this.nowBehave = null;
    }
    /// <summary>
    /// 因为某些原因而打破任务
    /// </summary>
    /// <param name="breakReason"></param>
    public void BreakBehave(string breakReason)
    {
        nowBehave.OnBreak();
    }

    public bool HasTask()
    {
        return nowBehave != null;
    }
}

public class AIMindComponentCreator : BaseComponentCreator<AIMindComponent>
{
    public AINpcMindCfg AINpcMindCfg;
    public bool isActivite;
    public override ComponentType ComponentName()
    {
        return ComponentType.AIMind;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new AIMindComponent(cardModel, this);
    }
}