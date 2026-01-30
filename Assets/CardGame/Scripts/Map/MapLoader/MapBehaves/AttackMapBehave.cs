using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击行为
/// </summary>
public class AttackMapBehave:MapBehave
{
    /// <summary>
    /// 攻击的内容
    /// </summary>
    public MonsterAttackCfg attack;
    public AttackComponent from;
    public CanBeAttackComponent to;
    /// <summary>
    /// 全身肢体的部分
    /// </summary>
    public MonsterBody body;
    public AttackMapBehave(MapLoader map,AttackComponent from,CanBeAttackComponent to,MonsterAttackCfg attack,MonsterBody body):base(map)
    {
        this.from = from;
        this.to = to;
        this.attack = attack;
        this.body = body;
    }

    public override void Run()
    {
        map.mapAttackSystem.Attack(this);
    }
}