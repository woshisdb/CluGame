using UnityEngine;

/// <summary>
/// 卡牌拖拽事件参数
/// </summary>
public class CardDragEventArgs : System.EventArgs
{
    /// <summary>
    /// 触发事件的卡牌
    /// </summary>
    public DraggableCard Card { get; }
    
    /// <summary>
    /// 目标插槽（仅在放置事件中有效）
    /// </summary>
    public CardSlot TargetSlot { get; set; }
    
    /// <summary>
    /// 当前鼠标位置（屏幕坐标）
    /// </summary>
    public Vector2 MousePosition { get; }
    
    /// <summary>
    /// 卡牌当前世界位置
    /// </summary>
    public Vector3 WorldPosition { get; }

    public CardDragEventArgs(DraggableCard card, Vector2 mousePos, Vector3 worldPos)
    {
        Card = card;
        MousePosition = mousePos;
        WorldPosition = worldPos;
    }
}

/// <summary>
/// 卡片拖拽Unity事件（支持在Inspector配置）
/// </summary>
[System.Serializable]
public class CardDragUnityEvent : UnityEvent<CardDragEventArgs> { }