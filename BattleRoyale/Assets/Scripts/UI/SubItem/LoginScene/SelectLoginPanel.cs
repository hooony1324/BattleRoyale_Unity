using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectLoginPanel : UI_Base
{
    enum Buttons
    {
        LoginButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(OnClickLoginButton);

        return true;
    }

    void OnClickLoginButton(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_SignInPopup>();
    }
}