using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonoManager : SerializedMonoBehaviour
{
    public Dictionary<string, IRegisterID> IDMap;
}
