using UnityEngine;
/// <summary>
/// 卡片拖拽事件参数类，扩展支持目标插槽信息
/// </summary>
public class CardDragEventArgs : System.EventArgs
{
    /// <summary>
    /// 触发事件的卡片实例
    /// </summary>
    public DraggableCard Card { get; }

    /// <summary>
    /// 事件发生时鼠标的屏幕坐标（像素）
    /// </summary>
    public Vector3 MousePosition { get; }

    /// <summary>
    /// 事件发生时卡片的世界坐标位置
    /// </summary>
    public Vector3 CardPosition { get; }

    /// <summary>
    /// 卡片放置的目标插槽（可选，仅在放置事件中有效）
    /// </summary>
    public CardSlot TargetSlot { get; set; }

    /// <summary>
    /// 构造函数，初始化事件参数
    /// </summary>
    /// <param name="card">触发事件的卡片</param>
    /// <param name="mousePos">鼠标屏幕坐标</param>
    /// <param name="cardPos">卡片世界坐标</param>
    public CardDragEventArgs(DraggableCard card, Vector3 mousePos, Vector3 cardPos)
    {
        Card = card;
        MousePosition = mousePos;
        CardPosition = cardPos;
        TargetSlot = null; // 默认为空，需要时在事件触发前设置
    }
}