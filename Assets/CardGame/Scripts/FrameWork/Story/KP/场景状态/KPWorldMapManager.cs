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

public class WorldLocation
{
    public string name;
    public string description;
    public SpaceCardModel spaceModel;
    public List<string> connectedLocations = new List<string>();
    public string region;
}

public class PlayerMoveIntent
{
    public bool wantsToMove;
    public string targetLocation;
    public string description;
}

public class KPWorldMapManager
{
    public Dictionary<string, WorldLocation> worldLocations = new Dictionary<string, WorldLocation>();
    
    public SpaceCardModel currentSpace;
    
    /// <summary>
    /// 当前场景的故事管理器（从 SpaceCardModel 获取）
    /// </summary>
    public KPSpaceStoryManager currentStoryManager
    {
        get
        {
            return currentSpace?.spaceStoryManager;
        }
    }

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
        
        return null;
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
            Debug.Log($"未找到地点，正在生成新地点: {locationName}");
            var newPlaceDescription = await GeneratePlaceDescription(locationName, cocText);
            
            // 使用 WorldMapSystem.AddSpaceCard 创建新的 SpaceCardModel
            // SpaceCardModel 内部会自动创建 KPPlaceSpaceManager
            var newSpaceCardModel = GameFrameWork.Instance.WorldMapSystem.AddSpaceCard(locationName, newPlaceDescription);
            
            var newLocation = new WorldLocation
            {
                name = locationName,
                description = newPlaceDescription,
                spaceModel = newSpaceCardModel
            };
            worldLocations[locationName] = newLocation;
            currentSpace = newSpaceCardModel;
            
            // 初始化 SpaceCardModel 中的 KPSpaceStoryManager
            newSpaceCardModel.InitSpaceStoryManager(GameFrameWork.Instance.KP.KpWorldStoryManager, cocText);
            
            return newSpaceCardModel.spaceStoryManager;
        }
 
        currentSpace = targetSpace;
        
        // 如果 SpaceCardModel 还没有初始化 storyManager，则初始化
        if (currentSpace.spaceStoryManager == null)
        {
            currentSpace.InitSpaceStoryManager(GameFrameWork.Instance.KP.KpWorldStoryManager, cocText);
        }

        return currentSpace.spaceStoryManager;
    }

    private async Task<string> GeneratePlaceDescription(string locationName, string cocText)
    {
        var prompt = $@"你是一个克苏鲁神话跑团的故事生成器。
玩家正在前往一个新地点: {locationName}

【模组背景】
{cocText}

请生成这个新地点的详细描述，包含：
1. 地点名称
2. 地点环境描述
3. 玩家可能在这里发现的重要线索或信息

请用流畅的叙事方式描述，不要用JSON格式。";

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage { role = "system", content = prompt },
            new QwenChatMessage { role = "user", content = $"描述地点: {locationName}" }
        };

        var result = await GameFrameWork.Instance.GptSystem.ChatToGPT(messages);
        return result ?? $"玩家来到了{locationName}。";
    }

    public async Task<GptRet<bool>> EnterSpaceCardGPT(string name, SpaceCardModel space)
    {
        return new GptRet<bool>();
    }
    
    public async Task<GptRet<bool>> LeaveSpaceCardGPT(string name)
    {
        return null;
    }
    
    public List<SpaceCardModel> GetAllSpaces()
    {
        return GameFrameWork.Instance.WorldMapSystem.Spaces;
    }

    public NpcCardModel FindNpcById(string name)
    {
        return GameFrameWork.Instance.FindNpcById(name);
    }
    
    public NpcCardModel FindNpcByDescription(string description)
    {
        return null;
    }

    public KPWorldMapManager()
    {
        
    }
}
