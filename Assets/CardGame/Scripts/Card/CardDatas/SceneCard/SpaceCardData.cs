using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MainStreetSceneCard : SpaceCardData
// {
//     public MainStreetSceneCard():base()
//     {
//         title = "雾森镇";
//         description = "宽敞明亮";
//         hasMap = true;
//     }
//
//     public override CardEnum GetCardType()
//     {
//         return CardEnum.FogforestTown;
//     }
//
//     public override BaseGoToSpaceDelegate GetMapDelegate(string id)
//     {
//         return new GoToRoomDelegate()
//         {
//             npcId = id,
//             spaceType = SpaceType.house,
//         };
//     }
// }


// public enum SpaceType
// {
//     space1,
//     house
// }
/// <summary>
/// 地图管理器
/// </summary>

public abstract class BaseGoToSpaceDelegate
{
    // public SpaceType spaceType;
    // /// <summary>
    // /// 前往指定场景后的处理
    // /// </summary>
    // public Action<Action> OnEnter;
    // /// <summary>
    // /// 离开场景后的处理
    // /// </summary>
    // public Action<Action> OnExit;
    /// <summary>
    /// 那个npc前往指定地点
    /// </summary>
    public string npcId;

    public abstract void GoTo();
}
/// <summary>
/// 前往某个卡片的位置
/// </summary>
public class GoToSpaceCardDelegate:BaseGoToSpaceDelegate
{
    public SpaceCardModel toObj;
    public int time;
    public override void GoTo()
    {
        var to = GameFrameWork.Instance.cardsManager.FindCardByCfg(toObj.cfg);
        var npc = GameFrameWork.Instance.FindNpcById(npcId);
        var from = npc.GetFrom();
        from?.Exit(npc);
        if (time!=0)
        {
            GameFrameWork.Instance.GameTimeManager.AddTimeNode(new WaitTimeNode(time, () =>
            {
                ((SpaceCardModel)to.GetModel()).Enter(npc);
            }));
        }
        else
        {
            ((SpaceCardModel)to.GetModel()).Enter(npc);
        }
        if (npc.IsPlayer())//如果是玩家的行动则移动镜头
        {
            GameFrameWork.Instance.mainCamera.transform.position = to.transform.position + new Vector3(0,40,0);
        }
    }

    public GoToSpaceCardDelegate(SpaceCardModel toObj,int time,string npcId) : base()
    {
        this.toObj = toObj;
        this.time = time;
        this.npcId = npcId;
    }

}

public class GoToRoomDelegate : BaseGoToSpaceDelegate
{
    public SpaceEnum spaceType;
    public int time;
    public override void GoTo()
    {
        var npc = GameFrameWork.Instance.FindNpcById(npcId);
        var space = GameFrameWork.Instance.spaces[this.spaceType];
        var from = npc.GetFrom();
        from?.Exit(npc);
        if (time != 0)
        {
            GameFrameWork.Instance.GameTimeManager.AddTimeNode(new WaitTimeNode(time, () =>
            {
                space.mapMgr.Enter(npc);
            }));
        }
        else
        {
            space.mapMgr.Enter(npc);
        }
        if (npc.IsPlayer())//如果是玩家的行动则移动镜头
        {
            GameFrameWork.Instance.mainCamera.transform.position = space.pos.position + new Vector3(0,40,0);
        }
    }
}

public class SpaceConfig
{
    /// <summary>
    /// 空间类型
    /// </summary>
    public SpaceEnum spaceType;
    /// <summary>
    /// 摄像机到达的位置
    /// </summary>
    public Transform pos;

    public MapLoader mapMgr;
}

public class SpaceCardCreateInfo : CardCreateInfo
{
    public SpaceCardConfig cfg;
    public CardEnum Belong()
    {
        return CardEnum.SpaceCard;
    }
}

public class SpaceCardData : CardData,CardDataDic<SpaceEnum,SpaceCardConfig>
{
    public Dictionary<SpaceEnum, SpaceCardConfig> SpaceConfigs;
    // public Dictionary<CardEnum, WayCfg> wayCfgs;

    public SpaceCardData() : base()
    {
        title = "房间卡";
        description = "房间卡牌,可以互动的房间";
        viewType = ViewType.SpaceCard;
        needRefresh = true;
        // wayCfgs = new Dictionary<CardEnum, WayCfg>();
        // InitWayCfg();
        this.Init<SpaceEnum,SpaceCardConfig>(ref SpaceConfigs);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.SpaceCard;
    }

    public override CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        return CreateModel(data);
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        var sc = CardCreateInfo as SpaceCardCreateInfo;
        var scene = new SpaceCardModel(this,sc.cfg);
        return scene;
    }

    public override void AfterCreate(CardModel cardModel)
    {
        GameFrameWork.Instance.data.saveFile.cards.Add(cardModel);
        GameFrameWork.Instance.data.saveFile.Space.Add(cardModel as SpaceCardModel);
    }
    public virtual bool CanGo()
    {
        return false;
    }

    public string GetResStr()
    {
        return "卡片库/场景";
    }

    public Dictionary<SpaceEnum, SpaceCardConfig> GetDic()
    {
        return SpaceConfigs;
    }
}

public class SpaceCardModel : CardModel,IBelong,ISearchNode,AIConclusion
{
    public override bool needShowCard
    {
        get
        {
            return true;
        }
    }
    /// <summary>
    /// 是否有地图，如果有的话则可以直接前往对应的地图
    /// </summary>
    public bool hasMap{
        get
        {
            return space.hasMap;
        }
    }
    /// <summary>
    /// 一系列的npc;
    /// </summary>
    public List<string> npcs;
    
    /// <summary>
    /// 场景故事管理器 - 管理故事、对话、叙事
    /// </summary>
    public KPSpaceStoryManager spaceStoryManager;
 
    public SpaceCardData SpaceCardData
    {
        get
        {
            return (SpaceCardData)cardData;
        }
    }
 
    public SpaceCardConfig space
    {
        get
        {
            return cfg.Value as SpaceCardConfig;
        }
    }
 
    public SpaceCardModel(CardData cardData,SpaceCardConfig space) : base(cardData,space)
    {
        npcs = new List<string>();
         
        // 确保有 ContainerComponent
        if (!IsSatComponent<ContainerComponent>())
        {
            var containerCreator = new ContainerComponentCreator();
            Components.Add(containerCreator.ComponentName(), containerCreator.Create(this));
        }
         
        // KPSpaceStoryManager 在 StartStory 时初始化
        spaceStoryManager = null;
         
        // SetDataByKey("Jobs",new List<JobCardModel>());
        // 模型初始化逻辑
    }
    
    /// <summary>
    /// 初始化场景故事管理器
    /// </summary>
    /// <param name="cocStoryManager">世界故事管理器</param>
    /// <param name="cocText">CoC模组文本</param>
    public void InitSpaceStoryManager(KPWorldStoryManager cocStoryManager, string cocText)
    {
        spaceStoryManager = new KPSpaceStoryManager();
        spaceStoryManager.Init(space.descirption, cocText, cocStoryManager.worldMapManager);
    }
    
    /// <summary>
    /// 获取场景中的 ContainerComponent
    /// </summary>
    public ContainerComponent GetContainer()
    {
        return GetComponent<ContainerComponent>();
    }
    
    /// <summary>
    /// 添加物品到场景
    /// </summary>
    /// <param name="obj">物品</param>
    /// <param name="location">位置描述</param>
    public void AddObject(ObjectCardModel obj, string location = null)
    {
        var container = GetContainer();
        if (container != null)
        {
            container.TryAdd(obj);
            
            // 如果提供了位置描述，设置到 SceneItemStateComponent
            if (!string.IsNullOrEmpty(location))
            {
                var stateComp = obj.GetComponent<SceneItemStateComponent>();
                if (stateComp != null)
                {
                    stateComp.location = location;
                }
            }
        }
    }
    
    /// <summary>
    /// 移除物品
    /// </summary>
    public void RemoveObject(ObjectCardModel obj)
    {
        var container = GetContainer();
        if (container != null)
        {
            container.TryRemove(obj);
        }
    }
    
    /// <summary>
    /// 获取可见物品
    /// </summary>
    public List<ObjectCardModel> GetVisibleObjects()
    {
        var container = GetContainer();
        if (container == null) return new List<ObjectCardModel>();
        
        var visibleObjects = new List<ObjectCardModel>();
        foreach (var card in container.Contents)
        {
            if (card is ObjectCardModel obj)
            {
                var stateComp = obj.GetComponent<SceneItemStateComponent>();
                if (stateComp != null && stateComp.isVisible)
                {
                    visibleObjects.Add(obj);
                }
            }
        }
        return visibleObjects;
    }
    
    /// <summary>
    /// 生成场景描述（供GPT使用）
    /// </summary>
    public string GenerateSceneDescription()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"【场景名称】{space.title}");
        sb.AppendLine($"【场景描述】{space.descirption}");
        
        var visibleObjects = GetVisibleObjects();
        if (visibleObjects.Count > 0)
        {
            sb.AppendLine("\n【场景中的物品】");
            foreach (var obj in visibleObjects)
            {
                sb.AppendLine($"- {obj.GetTitle()}: {obj.GetDescription()}");
                
                var stateComp = obj.GetComponent<SceneItemStateComponent>();
                if (stateComp != null)
                {
                    if (!string.IsNullOrEmpty(stateComp.location))
                    {
                        sb.AppendLine($"  位置: {stateComp.location}");
                    }
                    sb.AppendLine($"  状态: {stateComp.state}");
                }
            }
        }
        
        return sb.ToString();
    }
    public override List<UIItemBinder> GetUI()
    {
        var ret = new List<UIItemBinder>();
        ret = this.GetWorldMapUI(GameFrameWork.Instance.playerManager.nowPlayer);
        foreach (var npc in npcs)
        {
            var str = npc;
            var x = GameFrameWork.Instance.FindNpcById(npc);
            var npcList= x.GetWorldMapUI(GameFrameWork.Instance.playerManager.nowPlayer);
            ret.Add(new TableItemBinder(() =>
            {
                return str; 
            },npcList));
        }
        return ret;
    }


    public string ProviderName()
    {
        return cardData.title;
    }

    public virtual BaseGoToSpaceDelegate GetMapDelegate(string id)
    {
        return new GoToRoomDelegate()
        {
            npcId = id,
            spaceType = space.SpaceEnum,
            time = space.goToMapTime,
        };
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
        var view = GameFrameWork.Instance.cardsManager.FindCardByCfg(cfg);
        if (view)
        {
            return view.transform;
        }
        return null;
    }

    public List<string> GetNpcs()
    {
        return npcs;
    }

    public override string GetTitle()
    {
        return space.title;
    }

    public override string GetDescription()
    {
        return space.descirption;
    }

    public bool IsGoal(ISearchNode goal)
    {
        return this == goal;
    }

    public IEnumerable<SearchAction> GetNextNode()
    {
        var paths = GetComponent<PathComponent>().PathInfos;
        var ret = new List<SearchAction>();
        foreach (var x in paths)
        {
            ret.Add(new SearchAction(x.wasterTime,x.CardModel as ISearchNode));
        }
        return ret;
    }

    public int WasterTime(SpaceCardModel space)
    {
        var paths = GetComponent<PathComponent>().PathInfos;
        foreach (var x in paths)
        {
            if (space == x.CardModel)
            {
                return x.wasterTime;
            }
        }

        return 0;
    }
    /// <summary>
    /// 总结场景的当前信息
    /// </summary>
    /// <returns></returns>
    public virtual string ConclusionAIInfo(NpcCardModel npc)
    {
        if (GetComponent<ConclusionComponent>() != null)
        {
            return GetComponent<ConclusionComponent>().GetConclusion(npc);
        }

        return GetTitle()+"：没有可以交互的东西";
    }
}