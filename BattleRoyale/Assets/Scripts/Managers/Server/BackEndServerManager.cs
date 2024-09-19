using System;
using System.Collections.Generic;
using UnityEngine;

using BackEnd;
using static BackEnd.SendQueue;

public class BackEndServerManager : MonoBehaviour
{
    struct ServerToken
    {
        public string Id;
        public string Pw;
        public string Language;
    }
    public bool IsLoginSuccessed { get; private set; }
    private string _tempNickName;               // 설정할 닉네임
    public string MyNickname;                   // 로그인한 계정의 닉네임
    public string MyInDate;                     // 로그인한 InDate(데이터 발생한 시간)

    public string UserToken {get; private set;}

    private Action<bool, string> OnLoginSuccessed = null;
    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    private void Start()
    {
        IsLoginSuccessed = false;
        
        var settings = Resources.Load<TheBackendSettings>("TheBackendSettings");

        BackendCustomSetting customSetting = new BackendCustomSetting();
        customSetting.clientAppID = settings.clientAppID;
        customSetting.signatureKey = settings.signatureKey;
        customSetting.functionAuthKey = settings.functionAuthKey;

        customSetting.networkType = NetworkType.HTTP_CLIENT;
        var bro = Backend.Initialize(customSetting);

        if (bro.IsSuccess())
        {
            Debug.Log("Backend초기화 성공");

            // Backend초기화한 뒤 SendQueueManager초기화
            Managers.SendQueueManager.Init();
            
            //TODO Chat.Init시점 == 로그인 뒤에
            //Managers.Chat.Init();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_MessagePopup>($"초기화중 에러 발생\n\n{bro}");
        }
    }

    public void CustomLogin(string id, string pw, Action<bool, string> action)
    {
        Enqueue(Backend.BMember.CustomLogin, id, pw, callback => 
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 로그인 성공");
                OnLoginSuccessed = action;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log($"커스텀 로그인 실패\n{callback}");
            action.Invoke(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    public void CustomSingUp(string id, string pw, Action<bool, string> action)
    {
        _tempNickName = id;

        Enqueue(Backend.BMember.CustomSignUp, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 회원가입 성공");
                OnLoginSuccessed = action;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.LogError($"커스텀 회원가입 실패\n{callback}");
            action.Invoke(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }


    public void UpdateNickname(string nickname, Action<bool, string> action)
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"닉네임 생성 실패\n{callback}");
                action.Invoke(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }

            OnLoginSuccessed = action;
            OnBackendAuthorized();
        });
    }

    public void BackendTokenLogin(Action<bool, string> action)
    {
        Enqueue(Backend.BMember.LoginWithTheBackendToken, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 로그인 성공");
                OnLoginSuccessed = action;

                OnPrevBackendAuthorized();                
                return;
            }

            Debug.Log($"토큰 로그인 실패\n{callback}");
            action(false, string.Empty);
        });
    }

    void OnPrevBackendAuthorized()
    {
        IsLoginSuccessed = true;

        OnBackendAuthorized();
    }
    void OnBackendAuthorized()
    {
        Enqueue(Backend.BMember.GetUserInfo, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"유저 정보 불러오기 실패\n{callback}");
                OnLoginSuccessed?.Invoke(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }

            Debug.Log($"유저정보\n{callback}");

            var info = callback.GetReturnValuetoJSON()["row"];
            if (info["nickname"] == null)
            {
                Managers.UI.ShowPopupUI<UI_InsertNicknamePopup>();
                return;
            }


            MyNickname = info["nickname"].ToString();
            MyInDate = info["inDate"].ToString();

            

            if (OnLoginSuccessed != null)
            {
                Managers.Match.GetMatchList(OnLoginSuccessed);
            }
        });
    }

}