using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum RelationType
{
    father,
    mother,
    child,
}

public enum SexType
{
    male,
    female,
}

public class NpcCardData : ObjectCardData
{
    public NpcCardData():base()
    {
        viewType = ViewType.NPCCard;
    }
    
    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return null;
    }
    public override CardEnum GetCardType()
    {
        return CardEnum.npc;
    }

    public override void AfterCreate(CardModel cardModel)
    {
        GameFrameWork.Instance.data.saveFile.AddNpc(cardModel as NpcCardModel);
    }
}
/// <summary>
/// npc的物理个人信息
/// </summary>
public class NpcPhyInfo
{
    /// <summary>
    /// 年龄
    /// </summary>
    public int age;
    /// <summary>
    /// 性别
    /// </summary>
    public SexType sex;
    /// <summary>
    /// 类型
    /// </summary>
    public NpcType npcType;
    /// <summary>
    /// 与其他npc的关系
    /// </summary>
    public Dictionary<RelationType,string> relationship;

    public NpcCardModel npc;

    public NpcPhyInfo(NpcCardModel npc)
    {
        this.npc = npc;
        relationship = new Dictionary<RelationType, string>();
    }
}




public enum NpcType
{
    /// <summary>
    /// 机器人
    /// </summary>
    Npc,
    /// <summary>
    /// 玩家
    /// </summary>
    Player,
}
/// <summary>
/// 所有的动物都继承自这个
/// </summary>
public class NpcCardModel : ObjectCardModel,INpcLogicActor,ICellSelector
{
    // public List<PlayerDicHand> PlayerHands;
    /// <summary>
    /// 所在的地方
    /// </summary>
    // public ObjectRef<IBelong> belong = new ObjectRef<IBelong>();
    /// <summary>
    /// 角色的思考特性
    /// </summary>
    public NpcThink NpcThink;
    /// <summary>
    /// 角色的物理特性
    /// </summary>
    public NpcPhyInfo NpcPhyInfo;

    public BehavePointComponent BehavePointComponent
    {
        get
        {
            return this.GetComponent<BehavePointComponent>();
        }
    }
    public string npcId;
    [SerializeField]
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.CardMap[CardEnum.npc];
        }
    }
    public NpcCardModel(string npcId,CardData cardData,BaseCardConfig cfg) : base(cardData,cfg)
    {
        this.npcId = npcId;
        // PlayerHands = new List<PlayerDicHand>();
        NpcPhyInfo = new NpcPhyInfo(this);
        NpcThink = new NpcThink(this);
        // InitNpcHands();
        // InitBehavePointModule();
    }
    // [Button("行动点初始化")]
    // public void InitBehavePointModule()
    // {
    //     BehavePointModule = new BehavePointComponent(this);
    // }
    [Button]
    public void InitNpcThink()
    {
        this.NpcThink = new NpcThink(this);
    }
    public IBelong GetFrom()
    {
        return GetComponent<BelongComponent>().belong.Value;
    }

    public void SetFrom(IBelong belong)
    {
        GetComponent<BelongComponent>().belong.Value = belong;
    }

    public bool IsPlayer()
    {
        return NpcType.Player == NpcPhyInfo.npcType;
    }

    public virtual List<UIItemBinder> GetCellSelect(CellModel cellModel)
    {
        var ret = new List<UIItemBinder>();
        if (npcId == GameFrameWork.Instance.playerManager.nowName)///和自己交互
        {
            ret.Add(new ButtonBinder(() =>
            {
                return "自言自语";
            }, () =>
            {
                Debug.Log("自言自语");
            }));
        }
        else//其他人和他交互
        {
            ret.Add(new ButtonBinder(() =>
            {
                return "聊天";
            }, () =>
            {
                Debug.Log("聊天");
            }));
            ret.Add(new ButtonBinder(() =>
            {
                return "聊天";
            }, () =>
            {
                Debug.Log("聊天");
            }));
        }
        return ret;
    }
    // public virtual void OnBeAttackSucc(AttackMapBehave attackMapBehave)
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // public virtual void OnBeAttackFail(AttackMapBehave attackMapBehave)
    // {
    //     throw new System.NotImplementedException();
    // }
    // public void WhenSee(SeeMapBehave seeMapBehave)
    // {
    //     throw new System.NotImplementedException();
    // }

    // public virtual List<MonsterBody> GetBodys()
    // {
    //     return null;
    // }

    public string Id
    {
        get
        {
            return this.npcId;
        }
    }

    public virtual void OnStartPlayer(Action done)
    {
        done?.Invoke();
    }

    public bool CanJoin(MapLoader mapLoader)
    {
        if (mapLoader.ContainNpc(this))
        {
            return true;
        }
        else
        {
            return true;
        }
    }

    public virtual void OnEndPlayer(Action done)
    {
        done?.Invoke();
    }
}