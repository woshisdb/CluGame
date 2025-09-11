using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum UIEnum
{
    cellUI,
    DialogueUI,
    DayUI,
    BuyUI,
}

public class UIManager : SerializedMonoBehaviour
{
    public InspectorUI inspector;
    public GameObject kvItemUI;
    public GameObject tableItemUI;
    public GameObject buttonUI;
    public GameObject cellUI;
    public GameObject mapUI;
    public Dictionary<UIEnum,GameObject> map;
    public void ToSceneUI(UIEnum uienum)
    {
        foreach (var kv in map)
        {
            kv.Value.SetActive(false);
        }
        if (map[uienum])
        {
            map[uienum].SetActive(true);
        }
        //if(uienum == UIEnum.cellUI)
        //{
        //    //cellUI.SetActive(true);
        //    //mapUI.SetActive(false);

        //}
        //else
        //{
        //    //cellUI.SetActive(false);
        //    //mapUI.SetActive(true);
        //}
    }
}
