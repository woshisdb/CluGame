using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum CellViewType
{
    wall,
    floor,
    empty
}

public interface ICellSelector
{
    List<UIItemBinder> GetCellSelect(CellModel cellModel);
}

public class CellView : SerializedMonoBehaviour,IUISelector,ISendEvent,IRegisterID
{
    public IDModel UID;
    public ObjectRef<MapLoader> MapLoader
    {
        get
        {
            return CellModel.Value.mapLoader;
        }
    }

    public CellSeeContainer CellSee;
    public TextMeshPro text;
    public Renderer visLayer;
    public Vector2Int pos
    {
        get
        {
            return CellModel.Value.pos;
        }
    }
    
    public ObjectRef<CellModel> CellModel = new ObjectRef<CellModel>();
    
    [ExecuteAlways]
    public void Awake()
    {
        this.GetID();
    }

    public void Start()
    {
    }
    public void TouchCell()
    {
        this.SendEvent<RefreshViewEvent>(new RefreshViewEvent(this));
    }

    public void Update()
    {
        if (visLayer&&CellSee)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            visLayer.GetPropertyBlock(mpb);
            Color c = mpb.GetColor("_Color");  // 或 _Color
            float vis = 1f;
            if (CellSee.allRay <= 0.1)
            {
                vis = 1f;
            }
            else
            {
                vis = 1-(CellSee.seeRay / CellSee.allRay);
            }

            c.a = vis;
            mpb.SetColor("_Color", c);
            visLayer.SetPropertyBlock(mpb);
        }
        if (CellModel.Value!=null&&CellModel.Value.npcs !=null)
        {
            if (this.text)
            {
                var text = "";
                if (CellModel.Value.objs!=null&&CellModel.Value.objs.Count!=0)
                {
                    text += "物品:\n";
                    foreach (var obj in CellModel.Value.objs)
                    {
                        text += obj.GetTitle()+",";
                    }
                }

                text += "\n";
                var npcs = CellModel.Value.npcs;
                if (npcs!=null&&npcs.Count!=0)
                {
                    text += "角色:\n";
                    foreach (var npc in npcs)
                    {
                        text += npc+",";
                    }
                }
                this.text.text = text;
            }
        }
        else
        {
            if (this.text)
            {
                this.text.text = "";
            }
        }
    }

    public List<UIItemBinder> GetUI()
    {
        var ret = new List<UIItemBinder>();
        if (CellModel.Value.npcs.Find(e => { return e == GameFrameWork.Instance.playerManager.nowName;})!=null)
        {
            ret.AddRange(PlayerInOperator(GameFrameWork.Instance.playerManager.nowPlayer));
            //-------------------------------------------
            //与npc的交互
            var npcs = CellModel.Value.npcs;
            var objs = CellModel.Value.objs;
            //与物体的交互
            foreach (var obj in objs)
            {
                ret.Add(new TableItemBinder(() => { return obj.GetTitle();},
                    obj.GetMapClickUI(CellModel.Value.mapLoader.Value,GameFrameWork.Instance.playerManager.nowPlayer)));
            }
            //与npc的交互
            foreach (var npc in npcs)
            {
                ret.Add(new TableItemBinder(() => { return npc;}, 
                    this.CellModel.Value.FindNpc(npc).GetMapClickUI(CellModel.Value.mapLoader,GameFrameWork.Instance.playerManager.nowPlayer)));
            }
        }
        else
        {
            var npc = GameFrameWork.Instance.playerManager.nowPlayer;
            ret.AddRange(this.CellModel.Value.GetMapClickUI(MapLoader,npc));
            // ret.Add(new ButtonBinder(() =>
            // {
            //     return "前往";
            // }, () =>
            // {
            //     Debug.Log("前往");
            //     var behave = new RunMapBehave(GameFrameWork.Instance.playerManager.nowPlayer,MapLoader);
            //     behave.start = ((CellModel)(GameFrameWork.Instance.playerManager.nowPlayer.GetFrom())).CellView.Value;
            //     behave.aim = this;
            //     behave.Run();
            // }));
        }
        return ret;
    }

    // public AICellDecisionSet GetDecisions(NpcCardModel npcModel)
    // {
    //     return null;
    //     // var ret = new AICellDecisionSet();
    //     // //在本地的话可以交互
    //     // if (CellModel.Value.npcs.Find(e => { return e == npcModel.Id;})!=null)
    //     // {
    //     //     //-------------------------------------------
    //     //     //与npc的交互
    //     //     var npcs = CellModel.Value.npcs;
    //     //     var objs = CellModel.Value.objs;
    //     //     //与物体的交互
    //     //     foreach (var obj in objs)
    //     //     {
    //     //         ret.npcDecisions.Add(new AINPcDecisionSet(obj,obj.GetMapAIDeconsUI(CellModel.Value.mapLoader.Value,npcModel)));
    //     //     }
    //     //     //与npc的交互
    //     //     foreach (var npc in npcs)
    //     //     {
    //     //         ret.npcDecisions.Add(new AINPcDecisionSet(this.CellModel.Value.FindNpc(npc),this.CellModel.Value.FindNpc(npc).GetMapAIDeconsUI(CellModel.Value.mapLoader.Value,npcModel)));
    //     //     }
    //     // }
    //     // else//不在的话就不用交互了
    //     // {
    //     //     // var retData = new Dictionary<string,CardModel>();
    //     //     // var moveBehave = new AIBehave(null, null, true).SetEndAction(e =>
    //     //     // {
    //     //     //     var behave = new RunMapBehave(npcModel,MapLoader);
    //     //     //     behave.start = ((CellModel)(npcModel.GetFrom())).CellView;
    //     //     //     behave.aim = this;
    //     //     //     behave.Run();
    //     //     // }).SetRetData(retData);
    //     //     // ret.cellDecisionSet = (new AINPcDecisionSet(CellModel,new List<AIDecision>()
    //     //     // {
    //     //     //     new AIDecision("前往","前往这个地方",moveBehave)
    //     //     // }));
    //     //     ret.cellDecisionSet = new AINPcDecisionSet(CellModel,CellModel.Value.GetMapAIDeconsUI(CellModel.Value.mapLoader.Value,npcModel));
    //     // }
    //
    //     // ret.CellModel = CellModel;
    //     // return ret;
    // }

    [Button]
    public void Enter(NpcCardModel npc)
    {
        if (CellModel.Value.npcs == null)
        {
            CellModel.Value.npcs = new List<string>();
        }
        CellModel.Value.npcs.Add(npc.npcId);
        npc.SetFrom(this.CellModel.Value);
    }

    public void Exit(NpcCardModel npc)
    {
        if (CellModel.Value.npcs == null)
        {
            CellModel.Value.npcs = new List<string>();
        }
        CellModel.Value.npcs.Remove(npc.npcId);
        npc.SetFrom(null);
    }

    public List<CellView> GetNeighbor()
    {
        return CellModel.Value.mapLoader.Value.mapMoveSystem.GetNeighbor(pos);
    }

    public List<string> GetNpcs()
    {
        return CellModel.Value.npcs;
    }

    public List<UIItemBinder> PlayerInOperator(NpcCardModel npc)
    {
        return this.CellModel.Value.GetMapClickUI(MapLoader.Value,npc);
    }
    
    public List<UIItemBinder> PlayerNotInOperator(NpcCardModel npc)
    {
        return this.CellModel.Value.GetMapClickUI(MapLoader.Value,npc);
    }

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
    [Button]
    public void AddObjectByCreator(CardCreateInfo creator)
    {
        var model = GameFrameWork.Instance.gameConfig.CreateCard(creator);
        CellModel.Value.CreateObj(model);
    }
}
