using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc配置", menuName = "Npc/Npc配置")]
public class NpcConfig:SerializedScriptableObject
{
    public Dictionary<string, int> mapInfo;
}