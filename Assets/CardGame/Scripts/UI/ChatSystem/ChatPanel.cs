using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class ChatInput
{
    public ChatComponent Speaker;   // 玩家ID
    public ChatComponent TargetNpc; // 当前对话NPC
    public string Content;     // 玩家输入文本
    public ChatPanel panel;
}

public interface IChatPanelListener
{
    Task OnSubmit(ChatInput input);
    void OnClose();
}


public class ChatPanel : MonoBehaviour
{
    // Reference to the chat container UI to display messages
    [SerializeField] private ChatContainer chatContainer;
    public InputField inputField;
    private IChatPanelListener listener;
    private ChatComponent currentNpcId;
    private ChatComponent speaker;
    public Button submitBtn;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(ChatComponent speaker,ChatComponent npcId, IChatPanelListener listener)
    {
        this.listener = listener;
        this.currentNpcId = npcId;
        this.speaker = speaker;
        gameObject.SetActive(true);
    }

    public void Submit()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        var input = new ChatInput
        {
            Speaker = speaker,
            TargetNpc = currentNpcId,
            Content = inputField.text,
            panel = this,
        };

        // Also display the player's message in the chat container UI
        // so the conversation is visible immediately to the user
        chatContainer?.AddMessage(input.Content);
        // Clear the input field for the next message
        inputField.text = string.Empty;
        // Notify the listener about the submitted input
        listener?.OnSubmit(input);
    }

    public void AddMessage(string str)
    {
        chatContainer.AddMessage(str);
    }
    public void Close()
    {
        listener?.OnClose();
        // Clear chat messages when closing the panel
        chatContainer?.ClearAllMessages();
        gameObject.SetActive(false);
    }

}
