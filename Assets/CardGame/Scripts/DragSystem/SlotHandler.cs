using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 卡片验证事件（支持通过引用修改验证结果）
/// </summary>
[System.Serializable]
public class CardValidationEvent : UnityEvent<DraggableCard, SlotHandler, ref bool> { }

/// <summary>
/// 卡片事件（传递卡片和插槽信息）
/// </summary>
[System.Serializable]
public class CardEvent : UnityEvent<DraggableCard, SlotHandler> { }

/// <summary>
/// 简化版插槽处理器，提供基础的卡片管理功能
/// 仅通过事件与外部交互，逻辑判断由外部监听者处理
/// </summary>
public class SlotHandler : MonoBehaviour
{
    [Header("位置配置")]
    [Tooltip("卡片放置的基础偏移量")]
    [SerializeField] private Vector3 baseOffset = Vector3.up * 0.1f;

    // 存储当前插槽中的卡片
    private List<DraggableCard> containedCards = new List<DraggableCard>();

    public int MaxSum;
    [Header("事件配置")]
    [Tooltip("当卡片尝试被添加时触发（用于验证是否可放置）")]
    public CardValidationEvent OnCardTryAdd;

    [Tooltip("当卡片成功添加到插槽时触发")]
    public CardEvent OnCardAdded;

    [Tooltip("当卡片从插槽移除时触发")]
    public CardEvent OnCardRemove;

    /// <summary>
    /// 将卡片添加到插槽
    /// </summary>
    /// <param name="card">要添加的卡片</param>
    public void AddCard(DraggableCard card)
    {
        if (card == null || containedCards.Contains(card))
            return;

        // 添加卡片到列表
        containedCards.Add(card);
        
        // 设置卡片位置
        UpdateCardPosition(card);

        // 关联卡片与插槽
        card.transform.SetParent(transform);
    }

    /// <summary>
    /// 从插槽移除卡片
    /// </summary>
    /// <param name="card">要移除的卡片</param>
    public void RemoveCard(DraggableCard card)
    {
        if (card == null || !containedCards.Contains(card))
            return;

        // 从列表移除
        containedCards.Remove(card);
        
        // 解除父级关联（可选，根据需求决定）
        card.transform.SetParent(null);
    }

    /// <summary>
    /// 触发卡片尝试添加事件（由外部调用，如CardHandler的OnCardPlaced之后）
    /// </summary>
    /// <param name="card">尝试添加的卡片</param>
    public void TriggerCardTryAdd(DraggableCard card)
    {
        if (card != null)
        {
            OnCardTryAdd?.Invoke(this, new SlotCardEventArgs(card, this));
        }
    }

    /// <summary>
    /// 触发卡片移除事件（通常在卡片被触摸时调用）
    /// </summary>
    public void TriggerCardRemove()
    {
        // 只在有卡片时触发
        if (containedCards.Count > 0)
        {
            // 默认触发最后添加的卡片（可根据需求修改）
            var topCard = containedCards[containedCards.Count - 1];
            OnCardRemove?.Invoke(this, new SlotCardEventArgs(topCard, this));
        }
    }

    /// <summary>
    /// 更新卡片在插槽中的位置
    /// </summary>
    private void UpdateCardPosition(DraggableCard card)
    {
        card.transform.position = transform.position + baseOffset;
        card.transform.rotation = transform.rotation;
    }

    /// <summary>
    /// 检查插槽是否包含指定卡片
    /// </summary>
    public bool ContainsCard(DraggableCard card) => containedCards.Contains(card);

    /// <summary>
    /// 获取插槽中所有卡片
    /// </summary>
    public List<DraggableCard> GetAllCards() => new List<DraggableCard>(containedCards);
}

/// <summary>
/// 插槽卡片事件参数
/// </summary>
public class SlotCardEventArgs : System.EventArgs
{
    /// <summary>
    /// 涉及的卡片
    /// </summary>
    public DraggableCard Card { get; }

    /// <summary>
    /// 发生事件的插槽
    /// </summary>
    public SlotHandler Slot { get; }

    public SlotCardEventArgs(DraggableCard card, SlotHandler slot)
    {
        Card = card;
        Slot = slot;
    }
}
