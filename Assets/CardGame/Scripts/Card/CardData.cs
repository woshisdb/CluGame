using System.Collections.Generic;

using System;
using System.Reflection;using Sirenix.OdinInspector;
using UnityEngine;

public static class InterfaceHelper
{
    public static HashSet<TEnum> GetBindEnums<TEnum>(Type classType) where TEnum : struct, Enum
    {
        var result = new HashSet<TEnum>();

        foreach (var iface in classType.GetInterfaces())
        {
            CollectEnumsRecursive<TEnum>(iface, result);
        }

        return result;
    }

    private static void CollectEnumsRecursive<TEnum>(Type iface, HashSet<TEnum> result) where TEnum : struct, Enum
    {
        // 读取自身 Attribute
        var attr = iface.GetCustomAttribute<InterfaceBindAttribute>();
        if (attr?.BindType is TEnum enumVal)
        {
            result.Add(enumVal);
        }

        // 递归父接口
        foreach (var parent in iface.GetInterfaces())
        {
            CollectEnumsRecursive<TEnum>(parent, result);
        }
    }
}


public interface CardDataDic<T, F> 
    where T : Enum
    where F : class
{
    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <returns></returns>
    string GetResStr();

    Dictionary<T, F> GetDic();
}

public interface CardSObj<T>
where T:Enum
{
    T GetEnum();
}

public static class CardDataDicExt
{
    public static void Init<T, F>(this CardDataDic<T, F> dic,ref Dictionary<T,F> map)
        where T : Enum
        where F : class,CardSObj<T>
    {
        map = new Dictionary<T, F>();
        // F[] allConfigs = Resources.LoadAll<F>(dic.GetResStr());
        // foreach (var x in allConfigs)
        // {
        //     map[x.GetEnum()] = x;
        // }
    }
}

public abstract class CardData
{
    public virtual string Title
    {
        get
        {
            return title;
        }
    }

    public virtual string Description
    {
        get
        {
            return description;
        }
    }
    public string title;
    public string description;
    public ViewType viewType;
    public HashSet<CardFlag> cardFlags;
    public bool needRefresh;
    /// <summary>
    /// 只能有一个
    /// </summary>
    public bool onlyOne;
    public CardData()
    {
        cardFlags = new HashSet<CardFlag>();
    }
    public abstract CardEnum GetCardType();
    public abstract CardModel CreateModel(CardCreateInfo CardCreateInfo);

    public bool hasFlag(CardFlag cardFlag)
    {
        return cardFlags.Contains(cardFlag);
    }

    public virtual void InitCardLineMgr(CardLineMgr cardLineMgr)
    {

    }
    public virtual CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        return null;
    }

    public virtual void AfterCreate(CardModel cardModel)
    {
        
    }
}