using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : UIItem
{
    public TextMeshProUGUI key;
    public Button value;
    public override void BindObj(UIItemBinder tableItemBinder)
    {
        binder = tableItemBinder;
    }
    public void Update()
    {
        var item = (ButtonBinder)binder;
        key.SetText(item.getKey());
        //value.SetText(item.getValue());
    }
    public void Click()
    {
        var item = (ButtonBinder)binder;
        if(item!=null&&item.getValue!=null)
        item.getValue();
    }
}
