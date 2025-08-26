using System.Collections.Generic;

using System;
using System.Reflection;

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


public abstract class CardData
{
    public string title;
    public string description;
    public ViewType viewType;
    public HashSet<CardFlag> cardFlags;
    public CardEnum cardEnum;
    public bool needRefresh;
    public CardData()
    {
        cardFlags = new HashSet<CardFlag>();
    }
    public abstract CardEnum GetCardType();
    public abstract CardModel CreateModel();

    public bool hasFlag(CardFlag cardFlag)
    {
        return cardFlag.ContainKey(cardFlag);
    }

    public void InitCardFlags(params Type types[])
    {
        foreach (var t in types)
        {
            var enums = InterfaceHelper.GetBindEnums<ServiceType>(t);
            cardFlags.UnionWith(enums);
        }
    }
}