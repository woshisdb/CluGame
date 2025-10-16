using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 卡牌放置判断的事件参数（用于在UnityAction中传递判断结果）
/// </summary>
[System.Serializable]
public class CardPlaceCheckArgs : UnityEvent<CardPlaceCheckArgs>
{
    public DraggableCard Card { get; set; } // 要放置的卡牌
    public bool IsAllowPlace { get; set; } = true; // 是否允许放置（默认允许）
}
[System.Serializable]
public class OnCardPlaceArgs: UnityEvent<OnCardPlaceArgs>
{
    public CardView Card { get; set; }
}

/// <summary>
/// 卡牌插槽类（使用UnityAction实现自定义放置判断，支持UI配置）
/// </summary>
//[RequireComponent(typeof(Renderer))]
public class CardSlot : MonoBehaviour
{
    [Header("插槽配置")]
    [Tooltip("卡牌放置偏移量")]
    public Vector3 cardOffset = Vector3.up * 0.1f;
    [Tooltip("是否允许放置卡牌（基础开关）")]
    public bool isAcceptingCards = true;

    [Header("放置判断事件（可在Inspector中绑定）")]
    [Tooltip("放置前的自定义判断逻辑，通过IsAllowPlace返回是否允许放置")]
    public CardPlaceCheckArgs OnBeforePlaceCheck;

    //private Renderer slotRenderer;
    private DraggableCard currentCard;
    public OnCardPlaceArgs onCardPlaceArgs;

    /// <summary>
    /// 插槽是否已被占用
    /// </summary>
    public bool IsOccupied => currentCard != null;

    private void Start()
    {
        //slotRenderer = GetComponent<Renderer>();
        //UpdateSlotColor();
    }

    /// <summary>
    /// 尝试放置卡牌到插槽（基础校验 + 自定义回调校验）
    /// </summary>
    /// <param name="card">要放置的卡牌</param>
    /// <returns>是否放置成功</returns>
    public bool TryPlaceCard(DraggableCard card,bool justCheck = false)
    {
        // 1. 基础放置条件校验（插槽开关、是否占用、卡牌是否有效）
        if (!isAcceptingCards || IsOccupied || card == null)
        {
            return false;
        }

        // 2. 自定义放置判断（通过UnityAction传递参数，获取外部规则）
        bool isAllowPlace = true;
        if (OnBeforePlaceCheck != null)
        {
            // 初始化判断参数
            var checkArgs = new CardPlaceCheckArgs
            {
                Card = card,
                IsAllowPlace = true // 默认允许放置
            };

            // 触发判断事件（外部可在回调中修改IsAllowPlace）
            OnBeforePlaceCheck.Invoke(checkArgs);
            isAllowPlace = checkArgs.IsAllowPlace;
        }

        // 3. 自定义规则不允许，则放置失败
        if (!isAllowPlace)
            return false;
        if (!justCheck)
        {
            // 4. 满足所有条件，执行放置逻辑
            currentCard = card;
            currentCard.transform.position = GetSlotPosition();
            currentCard.transform.rotation = transform.rotation;
            currentCard.GetComponent<Rigidbody>().isKinematic = true;
            onCardPlaceArgs.Invoke(new OnCardPlaceArgs
            {
                Card = currentCard.GetComponent<CardView>()
            });
        }
        return true;
    }

    /// <summary>
    /// 从插槽移除卡牌
    /// </summary>
    public void RemoveCard()
    {
        currentCard = null;
    }

    /// <summary>
    /// 获取插槽的卡牌放置位置
    /// </summary>
    public Vector3 GetSlotPosition()
    {
        return transform.position + cardOffset;
    }

}
