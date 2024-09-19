using BackEnd;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectLoginPanel : UI_Base
{
    enum Buttons
    {
        CustomLoginButton,
        GoogleLoginButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.CustomLoginButton).gameObject.BindEvent(OnClickLoginButton);
        GetButton((int)Buttons.GoogleLoginButton).gameObject.BindEvent(OnClickGoogleLoginButton);

        return true;
    }

    void OnClickLoginButton(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_SignInPopup>();
    }

    void OnClickGoogleLoginButton(PointerEventData eventData)
    {
        TheBackend.ToolKit.GoogleLogin.Android.GoogleLogin(true, GoogleLoginCallback);
    }

    void GoogleLoginCallback(bool isSuccess, string errorMessage, string token)
    {
        if (isSuccess == false)
        {
            Debug.LogError(errorMessage);
            return;
        }

        Debug.Log($"구글 토큰 : {token}");
        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
        Debug.Log($"페데레이션 로그인 결과 : {bro}");
        Managers.Scene.LoadScene(EScene.Lobby);
    }

    //// SignOut
    // public void SignOutGoogleLogin()
    // {
    //     TheBackend.ToolKit.GoogleLogin.Android.GoogleSignOut(true, GoogleSignOutCallback);
    // }

    // private void GoogleSignOutCallback(bool isSuccess, string error)
    // {
    //     if (isSuccess == false)
    //     {
    //         Debug.Log("구글 로그아웃 에러 응답 발생 : " + error);
    //     }
    //     else
    //     {
    //         Debug.Log("로그아웃 성공");
    //     }
    // }
}