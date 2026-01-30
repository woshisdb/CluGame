using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "怪物弱点配置", menuName = "怪物/弱点配置")]
public class MonsterWeakCfg:SerializedScriptableObject
{
    public AttackType weaknessType;
    public int attackRate = 60;
}