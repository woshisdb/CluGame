using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 可拖拽卡牌核心类
/// </summary>
[RequireComponent(typeof(Collider))]
public class DraggableCard : MonoBehaviour
{
    [Header("交互配置")]
    [Tooltip("区分点击和拖拽的像素阈值")]
    public float clickThreshold = 5f;
    [Tooltip("双击检测的时间间隔（秒）")]
    public float doubleClickTime = 0.3f;
    [Tooltip("引用主相机（默认自动获取）")]
    public Camera mainCamera;

    [Header("状态调试")]
    [SerializeField] private bool isDragging;
    [SerializeField] private bool isTouching;

    // 事件定义
    /// <summary>
    /// 双击事件（仅在快速点击两次且无拖拽时触发）
    /// </summary>
    public CardDragUnityEvent OnCardClicked;
    
    /// <summary>
    /// 首次触摸事件（鼠标按下时立即触发）
    /// </summary>
    public CardDragUnityEvent OnCardTouch;
    
    /// <summary>
    /// 拖拽移动事件（拖拽过程中持续触发）
    /// </summary>
    public CardDragUnityEvent OnCardMove;
    
    /// <summary>
    /// 放置完成事件（拖拽结束并放置时触发）
    /// </summary>
    public CardDragUnityEvent OnCardPlaced;

    // 内部状态变量
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector2 firstTouchPos;
    private float lastClickTime;
    private int clickCount;
    private bool isDoubleClickProcessed;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnMouseDown()
    {
        isTouching = true;
        firstTouchPos = Input.mousePosition;
        clickCount++;

        // 触发首次触摸事件
        var touchArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position);

        // 双击检测逻辑
        if (clickCount >= 2)
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= doubleClickTime && !isDragging)
            {
                // 触发双击事件
                var clickArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position);
                OnCardClicked?.Invoke(clickArgs);
                isDoubleClickProcessed = true;
                clickCount = 0; // 重置点击计数
            }
            else
            {
                // 超过双击时间阈值，重置为单次点击
                clickCount = 1;
            }
        }
        else
        {
            OnCardTouch?.Invoke(touchArgs);
        }

        lastClickTime = Time.time;
    }

    private void OnMouseDrag()
    {
        // 计算鼠标移动距离
        float moveDistance = Vector2.Distance(Input.mousePosition, firstTouchPos);
        
        // 超过阈值则判定为拖拽
        if (moveDistance > clickThreshold && !isDragging)
        {
            isDragging = true;
            clickCount = 0; // 拖拽状态下重置点击计数
        }

        // 拖拽中触发移动事件
        if (isDragging)
        {
            // 计算世界空间位置（基于卡片初始高度的平面）
            Plane dragPlane = new Plane(Vector3.up, originalPosition.y);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 newPos = ray.GetPoint(distance);
                transform.position = newPos;
                
                // 触发移动事件
                var moveArgs = new CardDragEventArgs(this, Input.mousePosition, newPos);
                OnCardMove?.Invoke(moveArgs);
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            // 拖拽结束，检测是否放置到插槽
            CardSlot targetSlot = null;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetSlot = hit.collider.GetComponent<CardSlot>();
            }

            // 触发放置事件
            var placeArgs = new CardDragEventArgs(this, Input.mousePosition, transform.position)
            {
                TargetSlot = targetSlot
            };
            OnCardPlaced?.Invoke(this, placeArgs);

            // 重置拖拽状态
            isDragging = false;
        }
        else
        {
            // 非拖拽状态下处理点击计数
            if (!isDoubleClickProcessed && clickCount == 1)
            {
                // 单次点击但未形成双击，延迟重置（避免影响双击检测）
                Invoke(nameof(ResetClickCount), doubleClickTime);
            }
        }

        isTouching = false;
        isDoubleClickProcessed = false;
    }

    private void ResetClickCount()
    {
        clickCount = 0;
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
    