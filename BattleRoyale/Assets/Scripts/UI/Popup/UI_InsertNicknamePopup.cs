using BackEnd.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InsertNicknamePopup : UI_Popup
{
    enum InputFields
    {
        InputField,
    }

    enum Buttons
    {
        ConfirmButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        return true;
    }

    void OnClickConfirmButton(PointerEventData eventData)
    {
        var nickname = GetTMPInputField((int)InputFields.InputField).text;

        if (string.IsNullOrEmpty(nickname))
        {
            Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo("닉네임을 먼저 입력해주세요", showConfirmButton: true,
            callback: (result)
            =>
            {
                if (result)
                {
                    Debug.Log("확인!");
                    //Managers.backend.updatenickname;
                }
                else
                {
                    Debug.Log("취소!");
                }
            });
        }
        else
        {
            Managers.BServer.UpdateNickname(nickname, (bool result, string error) => 
            {
                Managers.Dispatcher.BeginInvoke(() => 
                {
                    if (!result)
                    {
                        Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo($"닉네임 생성 오류\n\n{error}", showConfirmButton: true, confirmButtonText: "확인");
                        return;
                    }

                    // Change Lobby
                    Debug.Log("닉네임 생성 완료!");
                });
            });
        }
    }
}