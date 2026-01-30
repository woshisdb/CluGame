using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "怪物繁殖方式配置", menuName = "怪物/繁殖配置")]
public class MonsterReproductCfg:SerializedScriptableObject
{
    public ReproductionMethod reproductionMethod;
}