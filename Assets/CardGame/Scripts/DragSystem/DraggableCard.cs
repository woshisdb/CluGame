using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// 简化事件定义：统一使用单一事件类型（无需为每个事件单独定义密封类，减少冗余）
//[System.Serializable]
//public class CardDragUnityEvent : UnityEvent<CardDragEventArgs> { }

/// <summary>
/// 可拖拽卡牌核心类（仅保留单击、拖拽功能，无双击逻辑）
/// </summary>
[RequireComponent(typeof(Collider))]
public class DraggableCard : MonoBehaviour
{
    [Header("交互配置")]
    [Tooltip("区分点击和拖拽的像素阈值")]
    public float clickThreshold = 5f;
    public Camera mainCamera{
        get
        {
            return Camera.main;
        }}

    [Header("状态调试")]
    [SerializeField] private bool isDragging;
    [SerializeField] private bool isTouching;

    [Header("设置Y轴的位置")] public float yDragPos;
    // 事件定义
    /// <summary>
    /// 单击事件（非拖拽状态下抬起鼠标时触发）
    /// </summary>
    public CardDragUnityEvent OnCardClicked;
    public void Click(CardDragEventArgs cardDragEventArgs)
    {
        Debug.Log("Click");
    }
    /// <summary>
    /// 首次触摸事件（鼠标按下时立即触发）
    /// </summary>
    public CardDragUnityEvent OnCardTouch;
    public void Touch(CardDragEventArgs cardDragEventArgs)
    {
        Debug.Log("Touch");
    }

    /// <summary>
    /// 拖拽移动事件（拖拽过程中持续触发）
    /// </summary>
    public CardDragUnityEvent OnCardMove;
    public void Move(CardDragEventArgs cardDragEventArgs)
    {
        Debug.Log("Move");
    }

    /// <summary>
    /// 放置完成事件（拖拽结束并放置时触发）
    /// </summary>
    public CardDragUnityEvent OnCardPlaced;
    public void Placed(CardDragEventArgs cardDragEventArgs)
    {
        Debug.Log("Placed");
    }

    // 内部状态变量（删除双击相关的 clickCount/lastClickTime/isDoubleClickProcessed）
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector2 firstTouchPos;

    public void SetFirstTouchPos(Vector2 pos)
    {
        firstTouchPos = pos;
    }
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void OnMouseDown()
    {
        transform.parent = null;
        isTouching = true;
        firstTouchPos = Input.mousePosition;
        GetComponent<Rigidbody>().isKinematic = false;
        // 触发首次触摸事件
        var touchArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position);
        OnCardTouch?.Invoke(touchArgs);
    }

    public void OnMouseDrag()
    {
        // 计算鼠标移动距离，超过阈值判定为拖拽
        float moveDistance = Vector2.Distance(Input.mousePosition, firstTouchPos);
        if (moveDistance > clickThreshold && !isDragging)
        {
            isDragging = true;
        }

        // 拖拽中更新位置并触发移动事件
        if (isDragging)
        {
            Plane dragPlane = new Plane(Vector3.up, originalPosition.y);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 newPos = ray.GetPoint(distance);
                transform.position = new Vector3(newPos.x,yDragPos,newPos.z);
                var moveArgs = new CardDragEventArgs(this, Input.mousePosition, newPos);
                OnCardMove?.Invoke(moveArgs);
            }
        }
    }

    public void OnMouseUp()
    {
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // 关键：设置QueryTriggerInteraction.Collide以检测触发器
            RaycastHit[] hits = Physics.RaycastAll(
                ray
            );
            bool hasSat = false;
            for (int i = 0; i < hits.Length; i++)//检测所有的
            {
                // 拖拽结束：检测目标插槽并触发放置事件
                CardSlot targetSlot = null;
                targetSlot = hits[i].collider.GetComponent<CardSlot>();
                if(targetSlot)
                {
                    var placeArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position)
                    {
                        TargetSlot = targetSlot
                    };
                    hasSat = true;
                    OnCardPlaced?.Invoke(placeArgs);
                    break;
                }
            }
            if (!hasSat)
            {
                OnCardPlaced?.Invoke(new CardDragEventArgs(this, Input.mousePosition, transform.position));
            }
            isDragging = false;
        }
        else
        {
            // 非拖拽状态：直接触发单击事件
            var clickArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position);
            OnCardClicked?.Invoke(clickArgs);
        }

        isTouching = false;
    }

    /// <summary>
    /// 重置卡牌到初始位置
    /// </summary>
    public void ResetToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}

///// <summary>
///// 卡牌拖拽事件参数（删除 IsDoubleClick 属性）
///// </summary>
//public class CardDragEventArgs
//{
//    //public DraggableCard Card { get; }          // 触发事件的卡牌实例
//    public Vector2 ScreenPosition { get; }      // 事件触发时的屏幕坐标（鼠标位置）
//    public Vector3 WorldPosition { get; }       // 事件触发时的世界坐标（卡牌位置）
//    public CardSlot TargetSlot { get; set; }    // 放置时命中的卡牌插槽（仅放置事件有效）

//    public CardDragEventArgs(DraggableCard card, Vector2 screenPos, Vector3 worldPos)
//    {
//        this.Card
//        ScreenPosition = screenPos;
//        WorldPosition = worldPos;
//    }
//}