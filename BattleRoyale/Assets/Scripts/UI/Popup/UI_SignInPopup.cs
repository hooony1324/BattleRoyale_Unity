using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SignInPopup : UI_Popup
{
    enum Buttons
    {
        SignInButton,
        SignUpButton,
    }

    enum InputFields
    {
        IDInputField,
        PWInputField,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindTMPInputFields(typeof(InputFields));

        GetButton((int)Buttons.SignInButton).gameObject.BindEvent(OnClickSignIn);
        GetButton((int)Buttons.SignUpButton).gameObject.BindEvent(OnClickSignUp);

        return true;
    }

    void OnClickSignIn(PointerEventData eventData)
    {
        // 체크
        var id = GetTMPInputField((int)InputFields.IDInputField).text;
        var pw = GetTMPInputField((int)InputFields.PWInputField).text;
        
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo("ID 혹은 PW를 입력해주세요", showConfirmButton: true);
            return;
        }

        Managers.Server.CustomLogin(id, pw, (bool result, string error) => 
        {
            Managers.Dispatcher.BeginInvoke(() =>
            {
                if (!result)
                {
                    Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo($"로그인 에러\n\n{error}", showConfirmButton: true);
                    return;
                }

                Managers.Scene.LoadScene(EScene.Lobby);
            });
        });
        
    }

    void OnClickSignUp(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_SignUpPopup>();
    }
}