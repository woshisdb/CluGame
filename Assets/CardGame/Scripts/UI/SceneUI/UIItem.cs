using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIItemBinder
{
    /// <summary>
    /// 获取Key
    /// </summary>
    public Func<string> getKey;
    public UIItemBinder(Func<string> getKey)
    {
        this.getKey = getKey;
    }
}

public class KVItemBinder:UIItemBinder
{
    public Func<string> getValue;
    public KVItemBinder(Func<string> getKey, Func<string> getValue) :base(getKey)
    {
        this.getValue = getValue;
    }
}

public class TableItemBinder:UIItemBinder
{
    public List<UIItemBinder> items;

    public TableItemBinder(Func<string> getKey, List<UIItemBinder> items) : base(getKey)
    {
        this.items = items;
    }
}

public class ButtonBinder : UIItemBinder,ISendEvent
{
    public Action getValue;
    public ButtonBinder(Func<string> getKey, Action getValue,bool clearUI=true) : base(getKey)
    {
        if(clearUI==true)
        {
            this.getValue = () =>
            {
                getValue();
                this.SendEvent(new ClearUIEvent());
            };
        }
        else
        this.getValue = getValue;
    }
}

public abstract class UIItem : MonoBehaviour, UIInterface
{
    public UIItemBinder binder;
    /// <summary>
    /// 绑定对象
    /// </summary>
    public abstract void BindObj(UIItemBinder tableItemBinder);
}
