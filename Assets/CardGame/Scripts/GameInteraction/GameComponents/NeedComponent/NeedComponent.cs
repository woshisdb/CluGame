using System.Collections.Generic;

public interface INeed
{
    /// <summary>
    /// 需求的满足水平
    /// </summary>
    /// <returns></returns>
    string SatLevel();
    /// <summary>
    /// 对需求的描述
    /// </summary>
    /// <returns></returns>
    public string NeedDescription();
    /// <summary>
    /// 需求的名字
    /// </summary>
    /// <returns></returns>
    public string NeedName();
    /// <summary>
    /// 获取没有满足的原因
    /// </summary>
    /// <returns></returns>
    public List<string> GetWhyNotSat();
}
public class NeedComponent:BaseComponent
{
    public NeedComponent(CardModel cardModel, NeedComponentCreator creator) : base(cardModel, creator)
    {
    }
    /// <summary>
    /// 评估当前需求
    /// </summary>
    /// <param name="needs"></param>
    /// <returns></returns>
    INeed EvaluatePrimaryNeed(IEnumerable<INeed> needs)
    {
        return null;
    }
}

public class NeedComponentCreator:BaseComponentCreator<NeedComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.NeedComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new NeedComponent(cardModel, this);
    }
}