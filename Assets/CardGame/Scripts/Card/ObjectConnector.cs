using UnityEngine;
using TMPro;   // 如果你用 TextMeshPro

[RequireComponent(typeof(LineRenderer))]
public class ObjectConnector : MonoBehaviour
{
    public Transform objA;   // 起点
    public Transform objB;   // 终点
    public string description = "连接描述"; // 显示的文字
    public TextMeshPro textPrefab; // 拖一个 TextMeshPro 预制体进来

    private LineRenderer line;
    private TextMeshPro textInstance;

    public void Bind(Transform x,Transform y,string name)
    {
        this.objA = x;
        this.objB = y;
        description = name;
    }
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        // 初始化文字
        if (textPrefab != null)
        {
            textInstance = Instantiate(textPrefab, transform);
            textInstance.text = description;
        }
    }

    void Update()
    {
        if (objA == null || objB == null) GameObject.Destroy(gameObject);

        // 设置线的位置
        line.SetPosition(0, objA.position);
        line.SetPosition(1, objB.position);

        // 文字放在中点
        if (textInstance != null)
        {
            Vector3 midPoint = (objA.position + objB.position) / 2;
            textInstance.transform.position = midPoint + Vector3.up * 0.2f; // 稍微抬高一点
            textInstance.text = description;
            textInstance.transform.rotation = Camera.main.transform.rotation;
            // 朝向相机，避免看不见
        }
    }
}
