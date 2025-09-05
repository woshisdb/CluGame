using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 为IView提供扩展方法（Bind和Release）
/// </summary>
public static class IViewExtensions
{
    /// <summary>
    /// 绑定资源和事件的扩展方法
    /// </summary>
    public static void OnCreateView(this IView view)
    {
        var model = view.GetModel();
        GameFrameWork.Instance.viewModelManager.Bind(model,view);
    }

    /// <summary>
    /// 释放资源和事件的扩展方法
    /// </summary>
    public static void OnDestoryView(this IView view)
    {
        var model = view.GetModel();
        GameFrameWork.Instance.viewModelManager.ReleaseView(view);
    }

    public static void BindModel(this IView view, IModel model)
    {
        view.bindModel(model);
        view.OnCreateView();
        Refresh();
    }
}

public interface IView
{
    void bindModel(IModel model);
    IModel GetModel();
    void Refresh();
}
