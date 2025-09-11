using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("摄像头移动的速度")]
    public float moveSpeed = 10f;

    [Tooltip("是否使用时间缩放（Time.deltaTime）")]
    public bool useTimeScaling = true;

    [Header("缩放设置")]
    [Tooltip("滚轮缩放的灵敏度")]
    public float zoomSensitivity = 1f;

    [Tooltip("最小正交大小")]
    public float minOrthographicSize = 1f;

    [Tooltip("最大正交大小")]
    public float maxOrthographicSize = 20f;

    private Camera orthoCamera;

    void Start()
    {
        // 获取相机组件
        orthoCamera = GetComponent<Camera>();

        // 确保相机是正交模式
        if (!orthoCamera.orthographic)
        {
            Debug.LogWarning("相机不是正交模式，已自动切换为正交模式", this);
            orthoCamera.orthographic = true;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    // 处理移动逻辑
    void HandleMovement()
    {
        // 获取输入
        float horizontalInput = Input.GetAxis("Horizontal"); // 左右方向
        float verticalInput = Input.GetAxis("Vertical");     // 上下方向

        // 计算移动方向
        Vector3 moveDirection = new Vector3(horizontalInput,0, verticalInput).normalized;

        // 如果有移动输入才执行移动
        if (moveDirection.magnitude >= 0.1f)
        {
            // 计算移动距离，考虑时间缩放和当前正交大小（缩放时保持移动手感一致）
            float scaleFactor = orthoCamera.orthographicSize / maxOrthographicSize;
            float distance = moveSpeed * (useTimeScaling ? Time.deltaTime : 1f) * (1 + scaleFactor);

            // 计算移动向量
            Vector3 moveAmount = moveDirection * distance;

            // 移动摄像头
            transform.Translate(moveAmount, Space.World);
        }
    }

    // 处理滚轮缩放逻辑（正交相机通过改变orthographicSize实现）
    void HandleZoom()
    {
        // 获取鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 如果有滚轮输入
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // 计算新的正交大小（向前滚动缩小，向后滚动放大）
            float newSize = orthoCamera.orthographicSize - scrollInput * zoomSensitivity;

            // 限制在最小和最大范围内
            newSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);

            // 更新正交大小
            orthoCamera.orthographicSize = newSize;
        }
    }
}
