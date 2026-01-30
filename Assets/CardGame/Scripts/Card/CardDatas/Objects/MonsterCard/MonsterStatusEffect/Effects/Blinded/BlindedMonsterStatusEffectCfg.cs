using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "目盲Buff设置", menuName = "怪物Buff配置")]
public class BlindedMonsterStatusEffectCfg:MonsterStatusEffectCfg
{
    public BlindedMonsterStatusEffectCfg() : base()
    {
        statusEffectType = StatusEffectType.Blinded;
    }
}