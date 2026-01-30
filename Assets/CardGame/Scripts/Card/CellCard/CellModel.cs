using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public struct CellCreateInfo:CardCreateInfo
{
    public MapLoader MapLoader;
    public MapItemCfg Cfg;
    public int x;
    public int y;
    public CellCreateInfo(MapLoader mapLoader,MapItemCfg cfg,int x,int y)
    {
        this.MapLoader = mapLoader;
        this.Cfg = cfg;
        this.x = x;
        this.y = y;
    }
    public CardEnum Belong()
    {
        return CardEnum.cell;
    }
}

public class CellData:CardData
{
    public override CardEnum GetCardType()
    {
        return CardEnum.cell;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return null;
    }

    public override CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        var ret = (CellCreateInfo)data;
        return new CellModel(ret.MapLoader,this,ret.Cfg,ret.x,ret.y);
    }
}

public class CellModel:CardModel,IBelong
{
    public ObjectRef<CellView> CellView;
    public ObjectRef<MapLoader> mapLoader;

    public CellViewType CellViewType
    {
        get
        {
            return cfg.CellViewType;
        }
    }
    /// <summary>
    /// 一系列的NPC
    /// </summary>
    public List<string> npcs;
    /// <summary>
    /// 可能存在的一系列物品
    /// </summary>
    public List<CardModel> objs;

    public int x;
    public int y;
    public MapItemCfg cfg;

    public Dictionary<string, object> datas;
    public Vector2Int pos;
    public CellModel(MapLoader mapLoader,CardData card,MapItemCfg cfg,int x,int y):base(card)
    {
        this.cfg = cfg;
        this.x = x;
        this.y = y;
        CellView = new ObjectRef<CellView>();
        this.mapLoader = new ObjectRef<MapLoader>();
        this.mapLoader.Value = mapLoader;
        npcs = new List<string>();
        objs = new List<CardModel>();
        datas = new Dictionary<string, object>();
    }

    public void Enter(NpcCardModel npc)
    {
        if (npcs == null)
        {
            npcs = new List<string>();
        }
        npcs.Add(npc.npcId);
        npc.SetFrom(this);
    }
    [Button]
    public void CreateObj(CardModel obj)
    {
        objs.Add(obj);
    }
    public void Exit(NpcCardModel npc)
    {
        if (npcs == null)
        {
            npcs = new List<string>();
        }
        npcs.Remove(npc.npcId);
        npc.SetFrom(null);
    }

    public Transform GetTransform()
    {
        if (CellView.Value)
        {
            return CellView.Value.transform;
        }
        return null;
    }

    public List<string> GetNpcs()
    {
        return npcs;
    }

    public void WhenBeSee(SeeMapBehave seeMapBehave)
    {
        throw new System.NotImplementedException();
    }

    // public override IAISummary GetNpcSummary(NpcCardModel npc)
    // {
    //     var ret = new AICellSummary();
    //     if (npcs.Contains(npc.Id))
    //     {
    //         ret.description = "我在这";
    //         ret.distance = 0;
    //         return ret;
    //     }
    //     else
    //     {
    //         var path = MapMoveSystem.FindPath(((CellModel)npc.GetFrom()).CellView, CellView);
    //         if (path == null)
    //         {
    //             ret.description = "我无法到这里";
    //             ret.distance = -1;
    //             return ret;
    //         }
    //         ret.distance = (path.Count - 1);
    //         ret.description = "距离此地" + (path.Count - 1) + "步";
    //         return ret;
    //     }
    // }
}