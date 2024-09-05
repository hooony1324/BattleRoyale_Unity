using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MessagePopup : UI_Popup
{
    enum Texts
    {
        MessageText,
    }
    enum Buttons
    {
        ConfirmButton,      // left Button
        CancelButton,       // right Button
        ExitButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        return true;
    }

    
    public void SetInfo(string msgText, bool confirmButtonOn = false, bool cancelButtonOn = false, Action<PointerEventData> confirmCallback = null, Action<PointerEventData> cancelCallback = null, bool exitButtonOn = false)
    {
        GetTMPText((int)Texts.MessageText).text = msgText;

        Button confirmButton = GetButton((int)Buttons.ConfirmButton);
        confirmButton.gameObject.SetActive(confirmButtonOn);
        confirmButton.gameObject.BindEvent(confirmCallback);

        Button cancelButton = GetButton((int)Buttons.CancelButton);
        cancelButton.gameObject.SetActive(cancelButtonOn);
        confirmButton.gameObject.BindEvent(cancelCallback ?? OnClickExitButton);

        GetButton((int)Buttons.ExitButton).gameObject.SetActive(exitButtonOn);
    }

    void OnClickExitButton(PointerEventData data)
    {
        base.ClosePopupUI();
    }
}
