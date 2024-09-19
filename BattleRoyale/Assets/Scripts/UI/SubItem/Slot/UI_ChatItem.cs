using UnityEngine;
using UnityEngine.Assertions;

public class UI_ChatItem : UI_Base
{
    enum Texts
    {
        ChatText,
    }

    string chatText;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));


        return true;
    }

    public void SetInfo(string text)
    {
        chatText = text;
    }

    public void RefreshUI()
    {
        GetTMPText((int)Texts.ChatText).text = chatText;
    }
}