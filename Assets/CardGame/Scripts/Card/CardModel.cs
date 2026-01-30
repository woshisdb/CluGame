using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 创建卡片的信息
/// </summary>
public interface CardCreateInfo
{
    public CardEnum Belong();

}

public static class CardCreateInfo_Extend
{
    public static CardData GetCardData(this CardCreateInfo info)
    {
        return GameFrameWork.Instance.gameConfig.CardMap[info.Belong()];
    }
}

[SerializeField]
public class CardModel:IModel,ICardFlag,IRegisterID
{
    public virtual bool needShowCard
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 代表这个卡牌还可以用吗
    /// </summary>
    public bool enable;
    public bool atLeastOne;
    public Vector3 tablePos;
    [SerializeField]
    public virtual CardData cardData
    {
        get {
            return GameFrameWork.Instance.gameConfig.CardMap[cardEnum];
        }
    }
    public bool OnlyOne { get { return cardData.onlyOne; } }
    public CardEnum cardEnum;
    /// <summary>
    /// 卡牌的数据信息
    /// </summary>
    // public Dictionary<string,int> cardInt;
    // public Dictionary<string, bool> cardBool;
    // public Dictionary<string, float> cardFloat;
    // public Dictionary<string, object> cardObjects;
    public Action onClickFunc;
    public IDModel UID;
    public ObjectRef<BaseCardConfig> cfg=new ObjectRef<BaseCardConfig>();
    /// <summary>
    /// 一系列的组件,这些组件提供一系列的功能
    /// </summary>
    public Dictionary<ComponentType,IComponent> Components;
    public virtual IView CreateView()
    {
        var template = GameFrameWork.Instance.gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(template);
        var cardView = obj.GetComponent<CardView>();
        return cardView;
    }

    public IView CreateViewUI()
    {
        var template = GameFrameWork.Instance.gameConfig.viewDicUI[cardData.viewType];
        var obj = GameObject.Instantiate(template);
        var cardView = obj.GetComponent<CardViewUI>();
        return cardView;
    }
    public bool hasFlag(CardFlag cardFlag)
    {
        return cardData.hasFlag(cardFlag);
    }

    public CardModel(CardData cardData = null, BaseCardConfig cfg =null,bool needReg = true)
    {
        if (needReg)
        {
            this.GetID();
        }
        this.cfg.Value = cfg;
        if (cardData!=null)
        {
            this.cardEnum = cardData.GetCardType();
        }
        // this.cardBool = new Dictionary<string, bool>();
        // this.cardFloat = new Dictionary<string, float>();
        // this.cardInt = new Dictionary<string, int>();
        // this.cardObjects = new Dictionary<string, object>();
        Components = new Dictionary<ComponentType, IComponent>();
        if(cfg!=null)
            InitComponent(cfg.ComponentCreators);
    }
    [Button]
    public void InitComponent()
    {
        Components.Clear();
        InitComponent(cfg.Value.ComponentCreators);
    }
    public void InitComponent(List<IComponentCreator> cmps)
    {
        if (cmps!=null)
        {
            foreach (var cmpc in cmps)///获取一系列的组件
            {
                Components.Add(cmpc.ComponentName(),cmpc.Create(this));//创建一个组件
            }
        }
    }
    public bool NeedRefresh()
    {
        return cardData.needRefresh;
    }

    public bool hasSwitch()
    {
        return false;
    }
    /// <summary>
    /// 直接重写这个就是点击时出现的交互
    /// </summary>
    /// <returns></returns>
    public virtual List<UIItemBinder> GetUI()
    {
        return new List<UIItemBinder>()
        {
            new KVItemBinder(()=>{return "ee1"; },()=>{return "ee2"; }),
            new KVItemBinder(()=>{return "ee2"; },()=>{return "ee2"; }),
            new ButtonBinder(()=>{return "t1"; },()=>{})
        };
    }

    /// <summary>
    /// 重写这个就是点击所在的MapCell后出现的UI
    /// </summary>
    /// <returns></returns>
    public virtual List<UIItemBinder> GetMapClickUI(MapLoader map,CardModel to)
    {
        var ret = this.GetMapUI(map,to);
        return ret;
    }

    // public virtual List<AIBehave> GetMapAIDeconsUI(MapLoader map,CardModel to)
    // {
    //     var ret = this.GetMapAIBehave(map,to);
    //     return ret;
    // }
    public virtual string GetTitle()
    {
        return cardData.Title;
    }
    public virtual string GetDescription()
    {
        return cardData.Description;
    }

    public void SetOnClick(Action onClick)
    {
        this.onClickFunc = onClick;
    }
    public void TryOnClick()
    {
        onClickFunc?.Invoke();
    }

    public string SetID(Func<string> id)
    {
        if (UID == null)
        {
            UID = new IDModel();
            UID.id = id();
            GameFrameWork.Instance.data.saveFile.idMap[UID.id] = this;
        }
        return UID.id;
    }
    // /// <summary>
    // /// 获取npc对他的印象
    // /// </summary>
    // /// <param name="npc"></param>
    // /// <returns></returns>
    // public virtual IAISummary GetNpcSummary(NpcCardModel npc)
    // {
    //     return new AIObjectSummary();
    // }
    public bool IsSatComponent(ComponentType type)
    {
        if (Components == null)
        {
            return false;
        }
        return Components.ContainsKey(type);
    }
    public bool IsSatComponent<T>() where T:IComponent
    {
        foreach (var cmp in Components)
        {
            if (cmp.Value is T)
            {
                return true;
            }
        }

        return false;
    }
    public T GetComponent<T>() where T:IComponent
    {
        foreach (var cmp in Components)
        {
            if (cmp.Value is T)
            {
                return (T)cmp.Value;
            }
        }
        return default(T);
    }
    public List<T> GetComponents<T>()
    {
        var ret = new List<T>();
        foreach (var cmp in Components)
        {
            if (cmp.Value is T)
            {
                ret.Add((T)cmp.Value);
            }
        }
        return ret;
    }
}
