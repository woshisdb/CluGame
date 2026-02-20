using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatInput
{
    public ChatComponent Speaker;
    public ChatComponent TargetNpc;
    public string SelectedNpcName;
    public string Content;
    public ChatPanel panel;
}


public class ChatPanel : MonoBehaviour
{
    [SerializeField] private ChatContainer chatContainer;
    public InputField inputField;
    private IChatPanelListener listener;
    private ChatComponent currentNpcId;
    private ChatComponent speaker;
    public Button submitBtn;
    public TMP_Dropdown Dropdown;

    public void Init(ChatComponent speaker, ChatComponent npcId, IChatPanelListener listener)
    {
        this.listener = listener;
        this.currentNpcId = npcId;
        this.speaker = speaker;
        gameObject.SetActive(true);
    }

    public void UpdateDropdownOptions(List<string> options)
    {
        if (Dropdown == null)
        {
            Debug.LogWarning("Dropdown is not assigned!");
            return;
        }

        Dropdown.ClearOptions();
        
        var dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option));
        }
        
        Dropdown.AddOptions(dropdownOptions);
    }

    public string GetSelectedOption()
    {
        if (Dropdown == null || Dropdown.options == null || Dropdown.options.Count == 0)
            return string.Empty;
        
        return Dropdown.options[Dropdown.value].text;
    }

    public void Submit()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        var selectedNpc = GetSelectedOption();
        var targetNpc = currentNpcId;

        if (!string.IsNullOrEmpty(selectedNpc) && selectedNpc != "KP")
        {
            targetNpc = null;
        }

        var input = new ChatInput
        {
            Speaker = speaker,
            TargetNpc = targetNpc,
            SelectedNpcName = selectedNpc,
            Content = inputField.text,
            panel = this,
        };

        chatContainer?.AddMessage(input.Content);
        inputField.text = string.Empty;
        listener?.OnSubmit(input);
    }

    public void AddMessage(string str)
    {
        chatContainer.AddMessage(str);
    }

    public void Close()
    {
        listener?.OnClose();
        chatContainer?.ClearAllMessages();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 调用函数
    /// </summary>
    /// <param name="str"></param>
    public void CallFunc(string str)
    {
        
    }
}

public interface IChatPanelListener
{
    Task OnSubmit(ChatInput input);
    void OnClose();
}

