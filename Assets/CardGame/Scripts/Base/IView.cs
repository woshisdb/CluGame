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
    public static void onBindView(this IView view)
    {
        if (view is MonoBehaviour)
        {
            GameFrameWork.Instance.viewModelManager.Bind(view.GetModel(),view);
        }
    }

    /// <summary>
    /// 释放资源和事件的扩展方法
    /// </summary>
    public static void OnDestroyView(this IView view)
    {
        if (view is MonoBehaviour&& GameFrameWork.instance != null)
        {
            GameFrameWork.Instance.viewModelManager.ReleaseView(view);
        }
    }
}

public interface IView
{
    void bindModel(IModel model);
    IModel GetModel();
    void Refresh();
}
