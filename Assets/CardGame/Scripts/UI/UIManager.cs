using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIEnum
{
    cellUI,
    DialogueUI,
    DayUI,
    BuyUI,
}

public class UIManager : SerializedMonoBehaviour
{
    public PassStageAnim PassStageAnim;
    public InspectorUI inspector;
    public GameHandUI GameHandUI;
    public BehavePoint BehavePoint;
    public Button ButtonUI;
    public Dictionary<UIEnum,GameObject> map;
    public ObjectDetailPanel ObjectDetailPanel;
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

    public void ShowPlayerUI()
    {
        BehavePoint.gameObject.SetActive(true);
        GameHandUI.gameObject.SetActive(true);
        inspector.gameObject.SetActive(true);
        // PassStageAnim.gameObject.SetActive(true);
        ButtonUI.gameObject.SetActive(true);
    }

    public void HidePlayerUI()
    {
        BehavePoint.gameObject.SetActive(false);
        GameHandUI.gameObject.SetActive(false);
        inspector.gameObject.SetActive(false);
        // PassStageAnim.gameObject.SetActive(false);
        ButtonUI.gameObject.SetActive(false);
    }
}
