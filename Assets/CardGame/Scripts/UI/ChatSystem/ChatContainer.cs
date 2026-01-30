using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ChatContainer : MonoBehaviour
{
    [SerializeField] private ChatNode chatNodePrefab;
    [SerializeField] private ScrollRect scrollRect;
    [Button]
    public void AddMessage(string message)
    {
        ChatNode node = Instantiate(chatNodePrefab, transform);
        node.SetText(message);

        Canvas.ForceUpdateCanvases();
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0;
    }
}