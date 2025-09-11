using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TableItemUI : UIItem
{
    public TextMeshProUGUI key;
    public ListUI listUI;

    public override void BindObj(UIItemBinder tableItemBinder)
    {
        binder = tableItemBinder;
        key.SetText(binder.getKey());
        var b = (TableItemBinder)binder;
        foreach (var item in b.items)
        {
            InspectorUI.GenItems(item, listUI);
        }
    }
}
