using System.Collections.Generic;
/// <summary>
/// 路径信息
/// </summary>
public class PathInfo
{
    public CardModel CardModel;
    public int wasterTime;
}
public class PathInfoCreator
{
    public SpaceCardConfig SpaceCardConfig;
    public int wasterTime;

    public PathInfo Create()
    {
        var ret = new PathInfo();
        ret.wasterTime = wasterTime;
        ret.CardModel = SpaceCardConfig.FindCardModel();
        return ret;
    }
}
public class PathComponent:BaseComponent
{
    public List<PathInfo> PathInfos;
    public PathComponent(CardModel cardModel, PathComponentCreator creator) : base(cardModel, creator)
    {
        PathInfos = new List<PathInfo>();
        foreach (var x in creator.PathInfo)
        {
            PathInfos.Add(x.Create());
        }
    }
}

public class PathComponentCreator : BaseComponentCreator<PathComponent>
{
    public List<PathInfoCreator> PathInfo = new List<PathInfoCreator>();
    public override ComponentType ComponentName()
    {
        return ComponentType.PathComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new PathComponent(cardModel, this);
    }
}