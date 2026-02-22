using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GptRet<T>
{
    public T retData;
    public string description;
}

/// <summary>
/// 世界地图中的位置信息
/// </summary>
public class WorldLocation
{
    public string name;
    public string description;
    public SpaceCardModel spaceModel;
    public List<string> connectedLocations = new List<string>();
    public string region;
}

/// <summary>
/// 玩家移动意图
/// </summary>
public class PlayerMoveIntent
{
    public bool wantsToMove;
    public string targetLocation;
    public string description;
}

/// <summary>
/// 用来管理整个世界的状态，例如地点，和转发各个地点发生的事情到其他地点
/// </summary>
public class KPWorldMapManager
{
    /// <summary>
    /// 世界位置字典
    /// </summary>
    public Dictionary<string, WorldLocation> worldLocations = new Dictionary<string, WorldLocation>();
    
    /// <summary>
    /// 当前所在的位置
    /// </summary>
    public SpaceCardModel currentSpace;
    
    /// <summary>
    /// 玩家当前的故事管理器
    /// </summary>
    public KPSpaceStoryManager currentStoryManager;

    /// <summary>
    /// 初始化世界地图
    /// </summary>
    public void InitWorldMap()
    {
        var spaces = GetAllSpaces();
        worldLocations.Clear();
        
        foreach (var space in spaces)
        {
            var cfg = space.space;
            var location = new WorldLocation
            {
                name = cfg.title,
                description = cfg.descirption,
                spaceModel = space
            };
            worldLocations[cfg.title] = location;
        }
    }

    /// <summary>
    /// 对地点的描述，获取要去的地方的文字描述，再根据现有地图判断是否添加地点还是已经有地图了。向GPT询问
    /// </summary>
    public async Task<SpaceCardModel> TryFindWorldPlace(string description)
    {
        var spaces = GetAllSpaces() ?? new List<SpaceCardModel>();
        
        foreach (var space in spaces)
        {
            var cfg = space.space;
            if (cfg.title.Contains(description) || description.Contains(cfg.title))
            {
                return space;
            }
        }
        
        return await GenerateNewPlace(description);
    }
    public SpaceCardModel FindOrCreateSpace(string name, string description)
    {
        if (worldLocations.ContainsKey(name))
        {
            return worldLocations[name].spaceModel;
        }

        var spaces = GetAllSpaces();
        foreach (var space in spaces)
        {
            if (space.space.title == name)
            {
                return space;
            }
        }

        Debug.LogWarning($"Space not found: {name}");
        return null;
    }

    /// <summary>

    /// <summary>
    /// 根据描述生成新地点
    /// </summary>
    private async Task<SpaceCardModel> GenerateNewPlace(string description)
    {
        var prompt = $@"你是一个克苏鲁神话跑团的空间生成器。
根据以下描述生成一个新的地点：

{description}

请生成一个JSON格式的地点信息：
{{
    ""title"": ""地点名称"",
    ""description"": ""地点描述"",
    ""region"": ""所属区域""
}}";

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage { role = "system", content = prompt },
            new QwenChatMessage { role = "user", content = description }
        };

        var result = await GameFrameWork.Instance.GptSystem.ChatToGPT<dynamic>(messages);
        
        Debug.Log($"Generated new place: {result}");
        return null;
    }

    /// <summary>
    /// 检测玩家的移动意图
    /// </summary>
    public async Task<PlayerMoveIntent> DetectMoveIntent(string playerInput)
    {
        var moveKeywords = new[] { "去", "到", "走向", "前往", "离开", "去往", "goto", "go to", "leave" };
        
        var lowerInput = playerInput.ToLower();
        foreach (var keyword in moveKeywords)
        {
            if (lowerInput.Contains(keyword))
            {
                var intentResult = await AskGptForMoveIntent(playerInput);
                return intentResult;
            }
        }

        return new PlayerMoveIntent { wantsToMove = false };
    }

    private async Task<PlayerMoveIntent> AskGptForMoveIntent(string playerInput)
    {
        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content = @"你是一个动作分析器，判断玩家是否想要移动到另一个地点。"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"【玩家输入】
{playerInput}

请判断玩家是否想移动到某个地点。
如果是想移动，请给出目标地点名称。

JSON格式：
{{""wantsToMove"": true/false, ""targetLocation"": ""目标地点"", ""description"": ""移动描述""}}"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem.ChatToGPT<PlayerMoveIntent>(messages);
        return result ?? new PlayerMoveIntent { wantsToMove = false };
    }

    /// <summary>
    /// 切换到新位置
    /// </summary>
    public async Task<KPSpaceStoryManager> SwitchToLocation(string locationName, string cocText)
    {
        SpaceCardModel targetSpace = null;
        
        if (worldLocations.ContainsKey(locationName))
        {
            targetSpace = worldLocations[locationName].spaceModel;
        }
        else
        {
            targetSpace = await TryFindWorldPlace(locationName);
        }

        if (targetSpace == null)
        {
            Debug.LogWarning($"无法找到或创建位置: {locationName}");
            return null;
        }

        currentSpace = targetSpace;
        
        var newStoryManager = new KPSpaceStoryManager
        {
            context = cocText,
            worldMapManager = this
        };

        currentStoryManager = newStoryManager;
        newStoryManager.StartSpaceStory();

        var spaceName = targetSpace.space.title;
        if (GameFrameWork.Instance.ChatPanel != null)
        {
            GameFrameWork.Instance.ChatPanel.SetPlace(spaceName);
        }

        return newStoryManager;
    }

    /// <summary>
    /// 前往指定的位置，通过向GPTSystem询问
    /// </summary>
    public async Task<GptRet<bool>> EnterSpaceCardGPT(string name, SpaceCardModel space)
    {
        return new GptRet<bool>();
    }
    
    /// <summary>
    /// 离开地区，通过向GPT询问
    /// </summary>
    public async Task<GptRet<bool>> LeaveSpaceCardGPT(string name)
    {
        return null;
    }
    
    /// <summary>
    /// 获取所有的场景信息
    /// </summary>
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
    public NpcCardModel FindNpcByDescription(string description)
    {
        return null;
    }

    public KPWorldMapManager()
    {
        
    }
}
