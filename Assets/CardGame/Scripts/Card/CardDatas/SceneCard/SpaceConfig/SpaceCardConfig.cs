using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SpaceEnum
{
    space1=0,
    cinema=1,
    bookstore=2,
    foodstore=3,
}

public class  WayCfg
{
    public int distance;
    public SpaceCardConfig aim;
}

[CreateAssetMenu(fileName = "空间", menuName = "空间/空间配置")]
public class SpaceCardConfig:BaseCardConfig,CardSObj<SpaceEnum>
{
    public string title;
    public string descirption;
    public bool hasMap;
    public int goToMapTime;
    public SpaceEnum SpaceEnum;

    public bool CanEnter;
    // public List<WayCfg> WayCfgs = new List<WayCfg>();
    public SpaceEnum GetEnum()
    {
        return SpaceEnum;
    }

    public override CardModel FindCardModel()
    {
        foreach (var card in GameFrameWork.Instance.data.saveFile.Space)
        {
            if (card.cfg.Value == this)
            {
                return card;
            }
        }

        return null;
    }

    public SpaceCardConfig() : base()
    {
        
    }
}

public class SpaceCreatorRef
{
    public string name;
    public string detail;

    /// <summary>
    /// 所有相邻可以去的区域
    /// </summary>
    public List<SpaceCreatorRef> spaces = new();
    [Button]
    public SpaceCardConfig CreateCfg()
    {
        try
        {
            var cfg = new SpaceCardConfig();
            cfg.title = name;
            cfg.descirption = detail;
            cfg.ComponentCreators.Add(new DrawLineComponentCreator());
            cfg.ComponentCreators.Add(new PathComponentCreator());
            return cfg;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
}