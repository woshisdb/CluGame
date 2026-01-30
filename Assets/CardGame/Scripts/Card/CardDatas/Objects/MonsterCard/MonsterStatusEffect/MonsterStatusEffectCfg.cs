using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 状态效果的枚举
/// </summary>
public enum StatusEffectType
{
    [LabelText("目盲")]
    Blinded=1,

    [LabelText("魅惑")]
    Charmed=2,

    [LabelText("耳聋")]
    Deafened=3,

    [LabelText("恐惧")]
    Frightened=4,

    [LabelText("被擒抱")]
    Grappled=5,

    [LabelText("丧失行动能力")]
    Incapacitated=6,

    [LabelText("隐形")]
    Invisible=7,

    [LabelText("麻痹")]
    Paralyzed=8,

    [LabelText("石化")]
    Petrified=9,

    [LabelText("中毒")]
    Poisoned=10,

    [LabelText("倒地")]
    Prone=11,

    [LabelText("束缚")]
    Restrained=12,

    [LabelText("震慑")]
    Stunned=13,

    [LabelText("昏迷")]
    Unconscious=14,
    
    [LabelText("力竭")]
    Exhaustion=15,

    [LabelText("沉默")]
    Silenced=16,

    [LabelText("虚弱")]
    Weakened=17,

    [LabelText("易伤")]
    Vulnerable=18,

    [LabelText("被标记")]
    Marked=19,

    [LabelText("压制")]
    Suppressed=20,

    [LabelText("流血")]
    Bleeding=21,

    [LabelText("燃烧")]
    Burning=22,

    [LabelText("冻结")]
    Frozen=23
}

public interface IMonsterStatusEffect
{
    
}

[CreateAssetMenu(fileName = "怪物Buff配置", menuName = "怪物Buff配置")]
public class MonsterStatusEffectCfg:SerializedScriptableObject,CardSObj<StatusEffectType>
{
    public string name;
    public string descirption;
    public StatusEffectType statusEffectType;
    public void InitStatusEffect()
    {
        
    }

    public StatusEffectType GetEnum()
    {
        return statusEffectType;
    }
}