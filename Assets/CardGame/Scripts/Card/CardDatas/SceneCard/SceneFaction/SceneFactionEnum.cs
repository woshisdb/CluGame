using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SceneFactionEnum
{
    /// <summary>
    /// 教会
    /// </summary>
    ChurchAuthority,
    /// <summary>
    /// 镇长实例
    /// </summary>
    MayorFaction,
    /// <summary>
    /// 议员势力
    /// </summary>
    CouncilorsFaction,
    /// <summary>
    /// 工匠
    /// </summary>
    CraftsmensGuild,
    /// <summary>
    /// 商人
    /// </summary>
    MerchantAlliance,
    /// <summary>
    /// 伐木商会
    /// </summary>
    LumberMerchantsGuild,
    /// <summary>
    /// 护林员行会
    /// </summary>
    NatureWorshipers,
    /// <summary>
    /// 秘密结社
    /// </summary>
    SecretSociety,
    /// <summary>
    /// 黑帮势力
    /// </summary>
    GangsterForce,
}
/// <summary>
/// 结社
/// </summary>
public class Faction
{
    public string Name;
    /// <summary>
    /// 结社的成员
    /// </summary>
    public List<NpcCardModel> Cards;
}

public class FactionManager
{
    public Dictionary<string,Faction> Factions;
    [Button]
    public void AddFaction(string name)
    {
        Faction faction = new Faction();
        faction.Name = name;
        Factions.Add(name,faction);
    }
    public Faction GetFaction(string name)
    {
        return Factions[name];
    }
}