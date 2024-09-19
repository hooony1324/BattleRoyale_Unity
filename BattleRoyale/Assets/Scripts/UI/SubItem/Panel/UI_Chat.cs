using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Chat : UI_Base
{
    const int CHATCONTENT_MAX = 8;
    enum InputFields
    {
        ChatInputField,
    }

    enum Buttons
    {
        SendButton,
    }

    enum GameObjects
    {
        ChatContent,
    }

    private GameObject _contentChat;
    private List<string> _chats = new List<string>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindGameObjects(typeof(GameObjects));

        _contentChat = GetGameObject((int)GameObjects.ChatContent);
        

        GetButton((int)Buttons.SendButton).gameObject.BindEvent(OnClickSendButton);


        return true;
    }

    private void OnEnable()
    {
        Managers.Chat.OnChatEvent += HandleChatEvent;
    }

    private void OnDisable()
    {
        Managers.Chat.OnChatEvent -= HandleChatEvent;
    }

    void OnClickSendButton(PointerEventData eventData)
    {
        TMP_InputField inputField = GetTMPInputField((int)InputFields.ChatInputField);
        var inputText = inputField.text;
        if (string.IsNullOrEmpty(inputText))
            return;
        
        inputField.text = "";
        Managers.Chat.SendMessage(inputText);
        
    }

    void HandleChatEvent(string message)
    {
        GameObject go = Managers.Resource.Instantiate(nameof(UI_ChatItem), _contentChat.transform);
        go.GetComponent<UI_ChatItem>().SetInfo(message);
        
        RefreshUI();
    }

    void RefreshUI()
    {
        foreach (Transform child in _contentChat.transform)
        {
            UI_ChatItem slot = child.GetComponent<UI_ChatItem>();
            slot.RefreshUI();
        }
    }


}