using System.Collections.Generic;
using UnityEngine;

public class WorldMapSystem
{
    /// <summary>
    /// 一系列的场景,--------
    /// </summary>
    public List<SpaceCardModel> Spaces
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.Space;
        }
    }

    /// <summary>
    /// 添加新的空间卡片
    /// </summary>
    /// <param name="title">地点名称</param>
    /// <param name="description">地点描述</param>
    /// <param name="connectedSpaces">连接的SpaceCardConfig列表，Key为目标SpaceCardConfig，Value为花费时间</param>
    /// <returns>创建成功的SpaceCardModel</returns>
    public SpaceCardModel AddSpaceCard(string title, string description, Dictionary<SpaceCardConfig, int> connectedSpaces = null)
    {
        // 1. 创建 SpaceCardConfig
        var cfg = new SpaceCardConfig();
        cfg.title = title;
        cfg.descirption = description;
        cfg.CanEnter = true;
        cfg.hasMap = false;
        cfg.goToMapTime = 0;
        
        // 添加必要的组件
        cfg.ComponentCreators.Add(new DrawLineComponentCreator());
        
        // 添加路径组件
        var pathCreator = new PathComponentCreator();
        if (connectedSpaces != null)
        {
            foreach (var connected in connectedSpaces)
            {
                pathCreator.PathInfo.Add(new PathInfoCreator
                {
                    SpaceCardConfig = connected.Key,
                    wasterTime = connected.Value
                });
            }
        }
        cfg.ComponentCreators.Add(pathCreator);

        // 2. 创建 SpaceCardModel
        var createInfo = new SpaceCardCreateInfo { cfg = cfg };
        var spaceCardData = new SpaceCardData();
        var spaceCardModel = (SpaceCardModel)spaceCardData.CreateModel(createInfo);

        // 3. 添加到保存系统
        GameFrameWork.Instance.data.saveFile.cards.Add(spaceCardModel);
        GameFrameWork.Instance.data.saveFile.Space.Add(spaceCardModel);

        Debug.Log($"添加新地点: {title}");
        return spaceCardModel;
    }

    // /// <summary>
    // /// 添加新的空间卡片（使用已存在的SpaceCardModel作为连接）
    // /// </summary>
    // public SpaceCardModel AddSpaceCard(string title, string description, Dictionary<SpaceCardModel, int> connectedSpaceModels = null)
    // {
    //     var connectedConfigs = new Dictionary<SpaceCardConfig, int>();
    //     if (connectedSpaceModels != null)
    //     {
    //         foreach (var connected in connectedSpaceModels)
    //         {
    //             connectedConfigs[connected.space] = connected.Value;
    //         }
    //     }
    //     return AddSpaceCard(title, description, connectedConfigs);
    // }
}