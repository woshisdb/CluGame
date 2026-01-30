using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectDetailPanel:SerializedMonoBehaviour
{
    public ListUI listUI;
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel(List<UIItemBinder> uis)
    {
        listUI.ClearUis();
        gameObject.SetActive(true);
        if(uis!=null)
        foreach (var item in uis)
        {
            InspectorUI.GenItems(item, listUI);
        }
        InspectorUI.RefreshLayout(listUI.content.GetComponent<RectTransform>());
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }
}