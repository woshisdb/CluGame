using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class ChatInput
{
    public string SpeakerId;   // 玩家ID
    public string TargetNpcId; // 当前对话NPC
    public string Content;     // 玩家输入文本
}

public interface IChatPanelListener
{
    void OnSubmit(ChatInput input);
    void OnClose();
}


public class ChatPanel : MonoBehaviour
{
    [Header("UI")]
    public Button closeButton;
    public Button submitButton;
    public InputField inputField;
    public Text npcDialogText;

    private IChatPanelListener listener;
    private string currentNpcId;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(string npcId, string npcDialog, IChatPanelListener listener)
    {
        this.listener = listener;
        this.currentNpcId = npcId;
        npcDialogText.text = npcDialog;

        closeButton.onClick.AddListener(Close);
        submitButton.onClick.AddListener(Submit);
    }

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        var input = new ChatInput
        {
            SpeakerId = "Player",
            TargetNpcId = currentNpcId,
            Content = inputField.text
        };

        listener?.OnSubmit(input);

        inputField.text = string.Empty;
    }

    private void Close()
    {
        listener?.OnClose();
        gameObject.SetActive(false);
    }

}
