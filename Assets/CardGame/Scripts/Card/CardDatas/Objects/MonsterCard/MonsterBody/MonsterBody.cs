using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 储存在npc的肢体数据
/// </summary>
public class MonsterBodyData
{
    /// <summary>
    /// 血量
    /// </summary>
    public int partRatio;

    /// <summary>
    /// 肢体破坏
    /// </summary>
    public bool isBreak
    {
        get
        {
            return remainAttackHp <= 0;
        }
    }
    /// <summary>
    /// 被攻击后的处理
    /// </summary>
    public Action beBreakFunc;
    public MonsterBody cfg;
    public MonsterCardModel monster;
    public int remainAttackHp;
    public int maxHP;
    public MonsterBodyData(MonsterCardModel monster,int partRatio,MonsterBody cfg,HPComponent hpModule)
    {
        this.monster = monster;
        this.partRatio = partRatio;
        this.cfg = cfg;
        maxHP = hpModule.HP * partRatio / 100;
        remainAttackHp = maxHP;
    }
    /// <summary>
    /// 获取攻击收到的伤害
    /// </summary>
    /// <param name="attackMapBehave"></param>
    /// <returns></returns>
    public int GetBeAttackVal(MonsterAttackCfg monsterAttackCfg)
    {
        int ret = 0;
        foreach (var attack in monsterAttackCfg.AttackTypes)
        {
            var attackType = attack.Key;
            var attackData = attack.Value;
            if (cfg.weakCards.ContainsKey(attackType))
            {
                var realAttack = (cfg.weakCards[attackType].attackRate * attackData.hp) / 100;//真正的伤害
                ret += realAttack;
            }
        }
        return ret;
    }

    public bool TryAttack(int attackVal)
    {
        var befBreak = isBreak;
        if (attackVal<0)//扣血
        {
            remainAttackHp -= -attackVal;
        }
        else
        {
            remainAttackHp += attackVal;
        }
        remainAttackHp = Math.Max(remainAttackHp, 0);
        var afterBreak = isBreak;
        if (!befBreak && afterBreak)
        {
            OnBreak();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnBreak()
    {
        beBreakFunc?.Invoke();
    }
}

/// <summary>
/// 怪物的肢体
/// </summary>
[CreateAssetMenu(fileName = "怪物肢体", menuName = "怪物/怪物肢体")]
public class MonsterBody:SerializedScriptableObject
{
    /// <summary>
    /// 肢体的名字
    /// </summary>
    public string name;

    public string description;
    /// <summary>
    /// 肢体的占血量的比例
    /// </summary>
    public int partRatio;
    /// <summary>
    /// 一系列的子肢体
    /// </summary>
    public List<MonsterBody> monsterBodys;
    /// <summary>
    /// 一系列的弱点
    /// </summary>
    public Dictionary<AttackType,MonsterWeakCfg> weakCards;

    public MonsterBody()
    {
        monsterBodys = new List<MonsterBody>();
        weakCards = new Dictionary<AttackType,MonsterWeakCfg>();
    }

    public void InitBodyHp(MonsterCardModel cardModel,HPComponent hpModule)
    {
        hpModule.bodyDatas[name] = new MonsterBodyData(cardModel,partRatio,this,hpModule);
    }
}