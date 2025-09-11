using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KVItemUI : UIItem
{
    public TextMeshProUGUI key;
    public TextMeshProUGUI value;
    public override void BindObj(UIItemBinder tableItemBinder)
    {
        binder = tableItemBinder;
    }
    public void Update()
    {
        var item = (KVItemBinder)binder;
        key.SetText(item.getKey());
        value.SetText(item.getValue());
    }
}
