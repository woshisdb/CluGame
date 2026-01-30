using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableCard : MonoBehaviour, IPointerClickHandler
{
    // 暴露到面板的UnityEvent，分别对应左键和右键点击
    [Header("点击事件绑定")]
    public UnityEvent onLeftClick;  // 左键点击触发的事件
    public UnityEvent onRightClick; // 右键点击触发的事件

    // 实现IPointerClickHandler接口的点击检测方法
    public void OnPointerClick(PointerEventData eventData)
    {
        
        // 根据点击的鼠标按键，触发对应的UnityEvent
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (onLeftClick != null)
            {
                onLeftClick.Invoke(); // 执行所有绑定在左键事件上的方法
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (onRightClick != null)
            {
                onRightClick.Invoke(); // 执行所有绑定在右键事件上的方法
            }
        }
    }

    public void TestClick()
    {
        Debug.Log(11111);
    }
}