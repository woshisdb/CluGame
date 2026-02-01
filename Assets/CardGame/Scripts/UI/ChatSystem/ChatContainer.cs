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

    /// <summary>
    /// Clears all chat message nodes under this container.
    /// </summary>
    public void ClearAllMessages()
    {
        // Destroy all child message nodes from the end to avoid indexing issues
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Canvas.ForceUpdateCanvases();
        ScrollToBottom();
    }
}
