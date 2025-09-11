using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ΪIView�ṩ��չ������Bind��Release��
/// </summary>
public static class IViewExtensions
{
    /// <summary>
    /// ����Դ���¼�����չ����
    /// </summary>
    public static void onBindView(this IView view)
    {
        if (view is MonoBehaviour)
        {
            GameFrameWork.Instance.viewModelManager.Bind(view.GetModel(),view);
        }
    }

    /// <summary>
    /// �ͷ���Դ���¼�����չ����
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
