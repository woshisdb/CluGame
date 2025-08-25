using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : SerializedScriptableObject
{
    public GameObject slotTemplate;
    public Dictionary<ViewType, GameObject> viewDic;
    public GameObject taskTemplate;
}
