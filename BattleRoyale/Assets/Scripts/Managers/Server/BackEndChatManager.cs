using UnityEngine;

using BackndChat;
using System.Collections.Generic;
using System;
using IngameDebugConsole;

public class BackEndChatManager : MonoBehaviour, BackndChat.IChatClientListener
{
    public delegate void ChatEvent(string message);
    public ChatEvent OnChatEvent;
    private CircularBuffer<string> _chatLogs = new CircularBuffer<string>(500);

    private BackndChat.ChatClient ChatClient = null;
    
    public void Init()
    {
        ChatClient = new ChatClient(this, new ChatClientArguments
        {
            Avatar = "default"
        });

    }

    private void Update()
    {
        ChatClient?.Update();
    }

    public void OnJoinChannel(ChannelInfo channelInfo) 
    { 
        Debug.Log($"BackEndChatManager : {channelInfo.ChannelName}입장");
    }

    public void OnLeaveChannel(ChannelInfo channelInfo) 
    { 
        Debug.Log($"BackEndChatManager : {channelInfo.ChannelName}퇴장");
    }

    public void OnJoinChannelPlayer(string channelGroup, string channelName, UInt64 channelNumber, string gamerName, string avatar) 
    { 
        Debug.Log($"BackEndChatManager : {channelName}에 {gamerName}입장");
    }

    public void OnLeaveChannelPlayer(string channelGroup, string channelName, UInt64 channelNumber, string gamerName, string avatar) 
    { 
        Debug.Log($"BackEndChatManager : {channelName}에 {gamerName}퇴장");
    }

    public void OnChatMessage(MessageInfo messageInfo) 
    { 
        Debug.Log($"Group : {messageInfo.ChannelGroup}\nChannelName : {messageInfo.ChannelName}\nMessage : {messageInfo.Message}");

        string message = $"{messageInfo.GamerName} : {messageInfo.Message}";
        _chatLogs.Add(message);
        OnChatEvent?.Invoke(message);
    }

    public void OnWhisperMessage(WhisperMessageInfo messageInfo) { }

    public void OnTranslateMessage(List<MessageInfo> messages) { }

    public void OnHideMessage(MessageInfo messageInfo) { }

    public void OnDeleteMessage(MessageInfo messageInfo) { }

    public void OnSuccess(SUCCESS_MESSAGE success, object param)
    {
        switch(success)
        {
            default:
                break;
        }
    }

    public void OnError(ERROR_MESSAGE error, object param)
    {
        switch(error)
        {
            default:
                break;
        }
    }

    private void OnApplicationQuit()
    {
        ChatClient?.Dispose();
    }

    public void SendMessage(string message)
    {
        ChatClient.SendChatMessage("channel-1", "lobby", 1, message);
    }
}