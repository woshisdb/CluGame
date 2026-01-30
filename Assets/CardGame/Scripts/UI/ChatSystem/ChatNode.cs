using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ChatNode : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform root;
    public TextMeshProUGUI text;

    [Header("Layout")]
    public float maxWidth = 600f;
    public Vector2 padding = new Vector2(32f, 24f); // x=左右, y=上下
    [Button]
    public void SetText(string content)
    {
        text.text = content;

        // 1️⃣ 计算文本在最大宽度下需要的尺寸
        Vector2 textSize = text.GetPreferredValues(
            content,
            maxWidth - padding.x,
            0
        );

        float finalWidth  = Mathf.Min(textSize.x + padding.x, maxWidth);
        float finalHeight = textSize.y + padding.y;

        // 3️⃣ 设置 ChatNode 高度（宽度交给父级）
        root.sizeDelta = new Vector2(finalWidth, finalHeight);
    }
}