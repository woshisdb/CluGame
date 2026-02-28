using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 场景中物品的状态
/// </summary>
public enum ItemState
{
    /// <summary>
    /// 正常状态
    /// </summary>
    Normal,
    /// <summary>
    /// 已被检查/调查过
    /// </summary>
    Examined,
    /// <summary>
    /// 已被拿取
    /// </summary>
    Taken,
    /// <summary>
    /// 已被破坏
    /// </summary>
    Destroyed,
    /// <summary>
    /// 隐藏状态（不可见）
    /// </summary>
    Hidden,
    /// <summary>
    /// 锁定状态
    /// </summary>
    Locked,
    /// <summary>
    /// 已解锁
    /// </summary>
    Unlocked
}

/// <summary>
/// 场景中物品的信息
/// </summary>
public class SceneItemInfo
{
    /// <summary>
    /// 物品ID
    /// </summary>
    public string itemId;
    
    /// <summary>
    /// 物品名称
    /// </summary>
    public string itemName;
    
    /// <summary>
    /// 物品描述
    /// </summary>
    public string description;
    
    /// <summary>
    /// 物品位置
    /// </summary>
    public string location;
    
    /// <summary>
    /// 物品状态
    /// </summary>
    public ItemState state;
    
    /// <summary>
    /// 物品是否可见
    /// </summary>
    public bool isVisible => state != ItemState.Hidden;
    
    /// <summary>
    /// 物品是否可交互
    /// </summary>
    public bool isInteractable => state != ItemState.Taken && state != ItemState.Destroyed;
}

/// <summary>
/// 场景状态快照（用于保存和恢复）
/// </summary>
public class SceneStateSnapshot
{
    /// <summary>
    /// 场景名称
    /// </summary>
    public string sceneName;
    
    /// <summary>
    /// 物品状态列表
    /// </summary>
    public List<SceneItemInfo> items;
    
    /// <summary>
    /// 环境状态（如门是否打开、灯是否亮等）
    /// </summary>
    public Dictionary<string, bool> environmentStates;
    
    /// <summary>
    /// 自定义状态数据
    /// </summary>
    public Dictionary<string, string> customStates;
}

/// <summary>
/// KP场景空间管理器 - 管理场景中物品和状态
/// </summary>
public class KPPlaceSpaceManager
{
    /// <summary>
    /// 场景名称
    /// </summary>
    public string sceneName { get; private set; }
    
    /// <summary>
    /// 场景描述
    /// </summary>
    public string sceneDescription { get; private set; }
    
    /// <summary>
    /// 场景中的物品列表
    /// </summary>
    private Dictionary<string, SceneItemInfo> sceneItems;
    
    /// <summary>
    /// 环境状态（如门是否打开、灯是否亮等）
    /// </summary>
    private Dictionary<string, bool> environmentStates;
    
    /// <summary>
    /// 自定义状态数据
    /// </summary>
    private Dictionary<string, string> customStates;
    
    /// <summary>
    /// 初始化 KPPlaceSpaceManager
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="sceneDescription">场景描述</param>
    public void Init(string sceneName, string sceneDescription)
    {
        this.sceneName = sceneName;
        this.sceneDescription = sceneDescription;
        this.sceneItems = new Dictionary<string, SceneItemInfo>();
        this.environmentStates = new Dictionary<string, bool>();
        this.customStates = new Dictionary<string, string>();
        
        Debug.Log($"KPPlaceSpaceManager 初始化完成: {sceneName}");
    }
    
    /// <summary>
    /// 添加物品到场景
    /// </summary>
    public void AddItem(string itemId, string itemName, string description, string location, ItemState initialState = ItemState.Normal)
    {
        if (sceneItems.ContainsKey(itemId))
        {
            Debug.LogWarning($"物品已存在: {itemId}");
            return;
        }
        
        var itemInfo = new SceneItemInfo
        {
            itemId = itemId,
            itemName = itemName,
            description = description,
            location = location,
            state = initialState
        };
        
        sceneItems[itemId] = itemInfo;
        Debug.Log($"添加物品到场景: {itemName} ({itemId})");
    }
    
    /// <summary>
    /// 获取物品信息
    /// </summary>
    public SceneItemInfo GetItem(string itemId)
    {
        return sceneItems.TryGetValue(itemId, out var item) ? item : null;
    }
    
    /// <summary>
    /// 获取所有可见物品
    /// </summary>
    public List<SceneItemInfo> GetVisibleItems()
    {
        return sceneItems.Values.Where(i => i.isVisible).ToList();
    }
    
    /// <summary>
    /// 获取所有可交互物品
    /// </summary>
    public List<SceneItemInfo> GetInteractableItems()
    {
        return sceneItems.Values.Where(i => i.isInteractable).ToList();
    }
    
    /// <summary>
    /// 更新物品状态
    /// </summary>
    public bool UpdateItemState(string itemId, ItemState newState)
    {
        if (!sceneItems.ContainsKey(itemId))
        {
            Debug.LogWarning($"物品不存在: {itemId}");
            return false;
        }
        
        sceneItems[itemId].state = newState;
        Debug.Log($"更新物品状态: {itemId} -> {newState}");
        return true;
    }
    
    /// <summary>
    /// 检查物品是否在指定状态
    /// </summary>
    public bool IsItemInState(string itemId, ItemState state)
    {
        var item = GetItem(itemId);
        return item != null && item.state == state;
    }
    
    /// <summary>
    /// 设置环境状态
    /// </summary>
    public void SetEnvironmentState(string key, bool value)
    {
        environmentStates[key] = value;
        Debug.Log($"设置环境状态: {key} = {value}");
    }
    
    /// <summary>
    /// 获取环境状态
    /// </summary>
    public bool GetEnvironmentState(string key, bool defaultValue = false)
    {
        return environmentStates.TryGetValue(key, out var value) ? value : defaultValue;
    }
    
    /// <summary>
    /// 设置自定义状态
    /// </summary>
    public void SetCustomState(string key, string value)
    {
        customStates[key] = value;
    }
    
    /// <summary>
    /// 获取自定义状态
    /// </summary>
    public string GetCustomState(string key, string defaultValue = null)
    {
        return customStates.TryGetValue(key, out var value) ? value : defaultValue;
    }
    
    /// <summary>
    /// 生成场景状态描述（用于GPT理解）
    /// </summary>
    public string GenerateSceneStateDescription()
    {
        var description = $"【场景名称】{sceneName}\n";
        description += $"【场景描述】{sceneDescription}\n";
        
        // 添加可见物品
        var visibleItems = GetVisibleItems();
        if (visibleItems.Any())
        {
            description += "\n【场景中的物品】\n";
            foreach (var item in visibleItems)
            {
                description += $"- {item.itemName}({item.itemId}): {item.description}\n";
                description += $"  位置: {item.location}\n";
                description += $"  状态: {item.state}\n";
            }
        }
        
        // 添加环境状态
        if (environmentStates.Any())
        {
            description += "\n【环境状态】\n";
            foreach (var env in environmentStates)
            {
                description += $"- {env.Key}: {(env.Value ? "开启" : "关闭")}\n";
            }
        }
        
        return description;
    }
    
    /// <summary>
    /// 创建状态快照
    /// </summary>
    public SceneStateSnapshot CreateSnapshot()
    {
        return new SceneStateSnapshot
        {
            sceneName = sceneName,
            items = sceneItems.Values.ToList(),
            environmentStates = new Dictionary<string, bool>(environmentStates),
            customStates = new Dictionary<string, string>(customStates)
        };
    }
    
    /// <summary>
    /// 从快照恢复状态
    /// </summary>
    public void RestoreFromSnapshot(SceneStateSnapshot snapshot)
    {
        sceneName = snapshot.sceneName;
        sceneItems = snapshot.items.ToDictionary(i => i.itemId);
        environmentStates = new Dictionary<string, bool>(snapshot.environmentStates);
        customStates = new Dictionary<string, string>(snapshot.customStates);
        
        Debug.Log($"从快照恢复场景状态: {sceneName}");
    }
    
    /// <summary>
    /// 这里有什么东西（兼容旧接口）
    /// </summary>
    public string WhatInThisPlace(string placeId, string description)
    {
        return GenerateSceneStateDescription();
    }
}
