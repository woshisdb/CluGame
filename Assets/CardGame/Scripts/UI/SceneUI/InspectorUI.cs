using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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
        RefreshLayout(listUI.content.GetComponent<RectTransform>());
    }
    public static void RefreshLayout(RectTransform rect)
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
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
            RefreshLayout(go.GetComponent<RectTransform>());
        }
        else if (item.GetType() == typeof(ButtonBinder))
        {
            GameObject go = GameObject.Instantiate(GameFrameWork.Instance.gameConfig.buttonUI);
            var comp = go.GetComponent<ButtonUI>();
            comp.BindObj(item);
            listUI.Add(go);
            RefreshLayout(go.GetComponent<RectTransform>());
        }
        else
        {
            GameObject go = GameObject.Instantiate(GameFrameWork.Instance.gameConfig.tableItemUI);
            var comp = go.GetComponent<TableItemUI>();
            comp.BindObj((TableItemBinder)item);
            listUI.Add(go);
            RefreshLayout(go.GetComponent<RectTransform>());
        }
    }
}