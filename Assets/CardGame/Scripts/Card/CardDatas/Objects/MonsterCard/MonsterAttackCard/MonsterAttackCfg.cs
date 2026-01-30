using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MonsterAttackEnum
{
    attack1,
}

public class AttackData
{
    public int hp;
}

[CreateAssetMenu(fileName = "怪物攻击方式配置", menuName = "怪物/攻击配置")]
public class MonsterAttackCfg:SerializedScriptableObject,CardSObj<MonsterAttackEnum>
{
    public string cardName;
    public string cardDescription;
    public MonsterAttackEnum MonsterAttackEnum;
    /// <summary>
    /// 一系列的攻击属性和造成的伤害
    /// </summary>
    public Dictionary<AttackType,AttackData> AttackTypes;
    /// <summary>
    /// 最大距离
    /// </summary>
    public int maxDistance;
    /// <summary>
    /// 最小距离
    /// </summary>
    public int minDistance;
    /// <summary>
    /// 在攻击前的行为
    /// </summary>

    /// <summary>
    /// 攻击成功
    /// </summary>
    /// <param name="attackMapBehave"></param>
    public void OnSuccAttack(AttackMapBehave attackMapBehave)
    {
        
    }

    /// <summary>
    /// 攻击失败
    /// </summary>
    /// <param name="attackMapBehave"></param>
    public void OnFailAttack(AttackMapBehave attackMapBehave)
    {
        
    }
    public MonsterAttackEnum GetEnum()
    {
        return MonsterAttackEnum;
    }

    public MonsterAttackCfg() : base()
    {
        this.AttackTypes = new Dictionary<AttackType, AttackData>();
    }
}