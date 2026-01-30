using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "燃烧Buff设置", menuName = "怪物Buff配置")]
public class BurningMonsterStatusEffectCfg:MonsterStatusEffectCfg
{
    public BurningMonsterStatusEffectCfg() : base()
    {
        statusEffectType = StatusEffectType.Burning;
    }
}