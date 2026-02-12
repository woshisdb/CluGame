using System.Threading.Tasks;

public class GptRet<T>
{
    public T retData;
    public string description;
}

/// <summary>
/// 用来管理整个世界的状态，例如地点，和转发各个地点发生的事情到其他地点
/// </summary>
public class KPWorldMapManager
{
    /// <summary>
    /// 对地点的描述，获取要去的地方的文字描述，再根据现有地图判断是否添加地点。
    /// </summary>
    public async Task<SpaceCardModel> TryFindWorldPlace(string description)
    {
        return null;
    }

    /// <summary>
    /// 前往指定的位置
    /// </summary>
    /// <param name="name"></param>
    /// <param name="space"></param>
    /// <returns></returns>
    public async Task<GptRet<bool>> EnterSpaceCard(string name,SpaceCardModel space)
    {
        
    }

    public async Task<GptRet<bool>> LeaveSpaceCard(string name)
    {
        
    }
}