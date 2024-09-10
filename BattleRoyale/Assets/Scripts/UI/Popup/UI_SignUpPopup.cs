using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SignUpPopup : UI_Popup
{
    enum InputFields
    {
        IDInputField,
        PWInputField,
    }

    enum Buttons
    {
        RegisterButton,
        CancelButton,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        
        GetButton((int)Buttons.RegisterButton).gameObject.BindEvent(OnClickRegister);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancel);

        return true;
    }


    void OnClickRegister(PointerEventData eventData)
    {
        var id = GetTMPInputField((int)InputFields.IDInputField).text;
        var pw = GetTMPInputField((int)InputFields.PWInputField).text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo("ID 혹은 PW를 입력해주세요", showConfirmButton: true);
            return;
        }

        Managers.Server.CustomSingUp(id, pw, (bool result, string error) =>
        {
            Managers.Dispatcher.BeginInvoke(() =>
            {
                if (!result)
                {
                    Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo($"로그인 에러\n\n{error}", showConfirmButton: true, confirmButtonText: "확인");
                    return;
                }

                Managers.Scene.LoadScene(EScene.Lobby);
            });
        });
    }

    void OnClickCancel(PointerEventData eventData)
    {
        ClosePopupUI();
    }
}