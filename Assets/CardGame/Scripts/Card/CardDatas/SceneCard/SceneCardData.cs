using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneCardData : CardData
{
    public SceneCardData() : base()
    {
        title = "场景卡";
        description = "场景卡牌,包含多个房间";
        viewType = ViewType.SceneCard;
    }

    public override CardModel CreateModel()
    {
        var ret =new SceneCardModel(this);
        return ret;
    }
}



public class SceneCardModel : CardModel
{
    public static string Faction = "结社";
    public SceneCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
        SetDataByKey(Faction, new FactionManager());
    }
    public FactionManager GetFactionMgr()
    {
        return GetObjectByKey<FactionManager>(Faction);
    }
}