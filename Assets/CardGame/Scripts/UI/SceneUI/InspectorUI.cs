using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public struct RefreshViewEvent : IEvent
{
    public IUISelector obj;
    public RefreshViewEvent(IUISelector obj)
    { this.obj = obj; }
}
public struct ClearUIEvent:IEvent
{

}

public class InspectorUI : MonoBehaviour,IRegisterEvent
{
    public IUISelector nowObj;
    public ListUI listUI;
    public void Start()
    {
        this.Register<RefreshViewEvent>(e =>
        {
            nowObj = e.obj;
            ShowUI(nowObj);
        });
        this.Register<ClearUIEvent>(e =>
        {
            ClearUI();
        });
    }
    public void ShowUI(IUISelector obj)
    {
        nowObj=obj;
        listUI.ClearUis();
        if(obj==null)
        {
            return;
        }
        var uis=obj.GetUI();
        if(uis!=null)
        foreach (var item in uis)
        {
            GenItems(item, listUI);
        }
    }
    public void ClearUI()
    {
        listUI.ClearUis();
    }
    public static void GenItems(UIItemBinder item, ListUI listUI)
    {
        if (item.GetType() == typeof(KVItemBinder))
        {
            GameObject go = GameObject.Instantiate(GameFrameWork.Instance.gameConfig.kvItemUI);
            var comp = go.GetComponent<KVItemUI>();
            comp.BindObj(item);
            listUI.Add(go);
        }
        else if (item.GetType() == typeof(ButtonBinder))
        {
            GameObject go = GameObject.Instantiate(GameFrameWork.Instance.gameConfig.buttonUI);
            var comp = go.GetComponent<ButtonUI>();
            comp.BindObj(item);
            listUI.Add(go);
        }
        else
        {
            GameObject go = GameObject.Instantiate(GameFrameWork.Instance.gameConfig.tableItemUI);
            var comp = go.GetComponent<TableItemUI>();
            comp.BindObj((TableItemBinder)item);
            listUI.Add(go);
        }
    }
}