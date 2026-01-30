using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum WallDir
{
    left,
    right,
    up,
    down,
}
/// <summary>
/// 地图的行为
/// </summary>
public abstract class MapBehave
{
    public MapLoader map;
    public abstract void Run();

    public MapBehave(MapLoader map)
    {
        this.map = map;
    }
}

public class MapLoader : SerializedMonoBehaviour,IBelong
{
    [BoxGroup("地图系统")]
    /// <summary>
    /// 战斗系统
    /// </summary>
    public MapAttackSystem mapAttackSystem;
    /// <summary>
    /// 移动系统
    /// </summary>
    [BoxGroup("地图系统")]
    public MapMoveSystem mapMoveSystem;
    /// <summary>
    /// 观察系统
    /// </summary>
    [BoxGroup("地图系统")]
    public MapSeeSystem mapSeeSystem;

    [BoxGroup("地图系统")] 
    public MapAccidentSystem mapAccidentSystem;

    [BoxGroup("地图系统")] 
    public MapFindSystem mapFindSystem;
    [BoxGroup("地图系统")]
    public MapRelationSystem mapRelationSystem;
    
    public MapGeneratorCfg cfg;
    public Transform root;
    public float GridSize = 10f;
    /// <summary>
    /// 所有进来的NPC
    /// </summary>
    public List<string> npcs;
    /// <summary>
    /// 所属的地点
    /// </summary>
    public SpaceCardModel belong;
    public CellView[,] cellViews;
    public CellView[,] wallxs;
    public CellView[,] wallys;
    public List<GameObject> walls;
    public List<GameObject> floors;
    public CellView startView;

    public CellView GetCellViewByID(string id)
    {
        return transform.Find(id).GetComponent<CellView>();
    }

    [Button]
    public void Enter(NpcCardModel npc)
    {
        if (npcs == null)
        {
            npcs = new List<string>();
        }
        npcs.Add(npc.npcId);
        npc.SetFrom(this);
        startView.Enter(npc);
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
        return startView.transform;
    }

    [Button]
    public void InitID()
    {
        this.GetID();
    }
    [Button]
    public void InitSubSystem()
    {
        this.mapAccidentSystem.Bind(this);
        this.mapAttackSystem.Bind(this);
        this.mapFindSystem.Bind(this);
        this.mapMoveSystem.Bind(this);
        this.mapRelationSystem.Bind(this);
        this.mapSeeSystem.Bind(this);
    }
    [Button]
    public void LoadMap()
    {
        this.GetID();
        foreach (var obj in floors)
        {
            GameObject.DestroyImmediate(obj);
        }

        foreach (var obj in walls)
        {
            GameObject.DestroyImmediate(obj);
        }
        walls.Clear();
        floors.Clear();
        var grid = cfg.grid;
        var wallx = cfg.wallx;
        var wally = cfg.wally;
        var gx = grid.GetLength(0);
        var gy = grid.GetLength(1);
        cellViews = new CellView[gx,gy];
        wallxs = new CellView[gx,gy+1];
        wallys = new CellView[gx+1,gy];
        cfg.CellModels = new List<List<CellModel>>();
        for (int x = 0; x < grid.GetLength(0); x++)  // 第一维（行数）
        {
            cfg.CellModels.Add(new List<CellModel>());
            for (int y = 0; y < grid.GetLength(1); y++)  // 第二维（列数）
            {
                //对象
                var obj = GameObject.Instantiate(cfg.MapItemCfgs[grid[x, y]].view);
                obj.transform.parent = root;
                obj.transform.localPosition = new Vector3(x*GridSize, 0, y*GridSize);
                floors.Add(obj.gameObject);
                obj.name = "floor:" + x +","+ y;
                cellViews[x, y] = obj.GetComponent<CellView>();
                cellViews[x, y].CellModel.Value =(CellModel) GameFrameWork.Instance.gameConfig.CreateCard(new CellCreateInfo(this,cfg.MapItemCfgs[grid[x, y]],x,y)); //new CellModel(this);
                GameFrameWork.Instance.data.saveFile.cards.Add(cellViews[x, y].CellModel.Value);
                cfg.CellModels[x].Add(cellViews[x, y].CellModel);
                cfg.CellModels[x][y].pos = new Vector2Int(x, y);
                cellViews[x, y].CellModel.Value.CellView.Value = cellViews[x, y];
                if (x == 0 && y == 0)
                {
                    startView = obj.GetComponent<CellView>();
                }
            }
        }
        for (int x = 0; x < wallx.GetLength(0); x++)  // 第一维（行数）
        {
            for (int y = 0; y < wallx.GetLength(1); y++)  // 第二维（列数）
            {
                //对象
                var obj = GameObject.Instantiate(cfg.WallItemCfgs[wallx[x, y]].view);
                obj.transform.parent = root;
                obj.name = "wall:" + x +","+ y;
                obj.transform.localPosition = new Vector3(x*GridSize, 0, y*GridSize-0.5f*GridSize);
                walls.Add(obj.gameObject);
                wallxs[x, y] = obj.GetComponent<CellView>();
                wallxs[x, y].CellModel.Value =(CellModel) GameFrameWork.Instance.gameConfig.CreateCard(new CellCreateInfo(this,cfg.WallItemCfgs[wallx[x, y]],x,y)); //new CellModel(this)
                GameFrameWork.Instance.data.saveFile.cards.Add(wallxs[x, y].CellModel.Value);
                wallxs[x, y].CellModel.Value.CellView.Value = wallxs[x, y];
            }
        }
        for (int x = 0; x < wally.GetLength(0); x++)  // 第一维（行数）
        {
            for (int y = 0; y < wally.GetLength(1); y++)  // 第二维（列数）
            {
                //对象
                var obj = GameObject.Instantiate(cfg.WallItemCfgs[wally[x, y]].view);
                obj.transform.parent = root;
                obj.name = "wall:" + x +","+ y;
                obj.transform.Rotate(Vector3.up,90);
                obj.transform.localPosition = new Vector3(x*GridSize-0.5f*GridSize, 0, y*GridSize);
                walls.Add(obj.gameObject);
                wallys[x, y] = obj.GetComponent<CellView>();
                wallys[x, y].CellModel.Value =(CellModel) GameFrameWork.Instance.gameConfig.CreateCard(new CellCreateInfo(this,cfg.WallItemCfgs[wally[x, y]],x,y)); //new CellModel(this);
                GameFrameWork.Instance.data.saveFile.cards.Add(wallys[x, y].CellModel.Value);
                wallys[x, y].CellModel.Value.CellView.Value = wallys[x, y];
            }
        }
    }


    

    public List<string> GetNpcs()
    {
        return npcs;
    }

    public IDModel UID;
    public string SetID(Func<string> id)
    {
        if (UID == null)
        {
            UID = new IDModel();
            UID.id = id();
            GameFrameWork.Instance.MonoManager.IDMap[UID.id] = this;
        }

        return UID.id;
    }
}
