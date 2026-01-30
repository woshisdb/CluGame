using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler ,IPointerUpHandler
{
    public UnityEvent onLeftClick;
    public UnityEvent onDrag;
    public UnityEvent onRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }
    }

    public void OnMouseDrag()
    {
        onDrag?.Invoke();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        onRelease?.Invoke();
    }
}