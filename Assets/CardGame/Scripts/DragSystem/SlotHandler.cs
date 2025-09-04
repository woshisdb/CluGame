using UnityEngine;

/// <summary>
/// 卡牌插槽类（用于接收卡牌）
/// </summary>
public class CardSlot : MonoBehaviour
{
    [Header("插槽配置")]
    [Tooltip("卡牌放置偏移量")]
    public Vector3 cardOffset = Vector3.up * 0.1f;
    [Tooltip("是否允许放置卡牌")]
    public bool isAcceptingCards = true;
    [Tooltip("允许放置的卡牌标签（空则不限制）")]
    public string allowedCardTag;

    [Header("视觉反馈")]
    public Color normalColor = Color.gray;
    public Color highlightColor = Color.green;
    public Color blockedColor = Color.red;

    private Renderer slotRenderer;
    private DraggableCard currentCard;

    /// <summary>
    /// 插槽是否已被占用
    /// </summary>
    public bool IsOccupied => currentCard != null;

    private void Start()
    {
        slotRenderer = GetComponent<Renderer>();
        if (slotRenderer != null)
            slotRenderer.material.color = normalColor;
    }

    /// <summary>
    /// 尝试放置卡牌到插槽
    /// </summary>
    /// <param name="card">要放置的卡牌</param>
    /// <returns>是否放置成功</returns>
    public bool TryPlaceCard(DraggableCard card)
    {
        // 检查放置条件
        if (!isAcceptingCards || IsOccupied)
            return false;

        // 检查标签限制
        if (!string.IsNullOrEmpty(allowedCardTag) && !card.CompareTag(allowedCardTag))
            return false;

        // 放置卡牌
        currentCard = card;
        currentCard.transform.position = GetSlotPosition();
        currentCard.transform.rotation = transform.rotation;
        
        // 更新视觉反馈
        if (slotRenderer != null)
            slotRenderer.material.color = blockedColor;

        return true;
    }

    /// <summary>
    /// 从插槽移除卡牌
    /// </summary>
    public void RemoveCard()
    {
        currentCard = null;
        if (slotRenderer != null)
            slotRenderer.material.color = normalColor;
    }

    /// <summary>
    /// 获取插槽的卡牌放置位置
    /// </summary>
    public Vector3 GetSlotPosition()
    {
        return transform.position + cardOffset;
    }

    // 鼠标悬停视觉反馈
    private void OnMouseEnter()
    {
        if (!IsOccupied && isAcceptingCards && slotRenderer != null)
            slotRenderer.material.color = highlightColor;
    }

    private void OnMouseExit()
    {
        if (!IsOccupied && slotRenderer != null)
            slotRenderer.material.color = normalColor;
    }
}
    