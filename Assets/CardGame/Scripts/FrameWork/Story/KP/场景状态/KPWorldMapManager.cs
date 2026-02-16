using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
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
    /// 对地点的描述，获取要去的地方的文字描述，再根据现有地图判断是否添加地点还是已经有地图了。向GPT询问
    /// </summary>
    public async Task<SpaceCardModel> TryFindWorldPlace(string description)
    {
        // 1) 尝试在现有空间中匹配描述（Name/Description 字段兼容性处理）
        var spaces = GetAllSpaces() ?? new List<SpaceCardModel>();
        foreach (var space in spaces)
        {
            
        }
        return null;
    }

    /// <summary>
    /// 前往指定的位置，通过向GPTSystem询问
    /// </summary>
    /// <param name="name"></param>
    /// <param name="space"></param>
    /// <returns></returns>
    public async Task<GptRet<bool>> EnterSpaceCardGPT(string name,SpaceCardModel space)
    {
        // 简单示例：返回进入结果，留给具体实现扩展
        // return new GptRet<bool> { retData = true, description = "Entered space: " + (space?.name ?? name) };
        return new GptRet<bool>();
    }
    /// <summary>
    /// 离开地区，通过向GPT询问
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<GptRet<bool>> LeaveSpaceCardGPT(string name)
    {
        return null;
        // return new GptRet<bool> { retData = true, description = "Left space: " + name };
    }
    /// <summary>
    /// 获取所有的场景信息
    /// </summary>
    /// <returns></returns>
    public List<SpaceCardModel> GetAllSpaces()
    {
        return GameFrameWork.Instance.WorldMapSystem.Spaces;
    }

    public NpcCardModel FindNpcById(string name)
    {
        return GameFrameWork.Instance.FindNpcById(name);
    }
    /// <summary>
    /// 根据对npc的描述来寻找角色信息
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public NpcCardModel FindNpcByDescription(string description)
    {
        return null;
    }

    public KPWorldMapManager()
    {
        
    }
}
