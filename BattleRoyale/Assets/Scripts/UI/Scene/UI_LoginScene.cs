using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using UnityEngine;
using static LoginScene;

public class UI_LoginScene : UI_Scene
{
    enum GameObjects
    {
        SelectLoginPanel,
        LoadingPanel,
    }

    public void Setup()
    {
        BindGameObjects(typeof(GameObjects));

        GetGameObject((int)GameObjects.LoadingPanel).gameObject.SetActive(true);
        GetGameObject((int)GameObjects.SelectLoginPanel).gameObject.SetActive(false);
        
        Managers.Scene.GetCurrentScene<LoginScene>().OnLoginSceneStateChanged += HandleLoginSceneState;
    }

    private void OnDestroy()
    {
        Managers.Scene.GetCurrentScene<LoginScene>().OnLoginSceneStateChanged -= HandleLoginSceneState;
    }

    void HandleLoginSceneState(ELoginSceneState state)
    {
        switch (state)
        {
            // 다운로드 끝났으면 로그인 선택 상태로 변경
            case ELoginSceneState.ResourceLoadFinished:
                TryLoginByBackendToken();
                break;
        }
    }

    void TryLoginByBackendToken()
    {
        Managers.UI.ShowPopupUI<UI_LoadingBlockerPopup>();
        
        Managers.Server.BackendTokenLogin((bool result, string error) => 
        {
            Managers.Dispatcher.BeginInvoke(() =>
            {
                if (result)
                {
                    // ChangeLobby
                    Debug.Log("Load Lobby scene");
                    Managers.Scene.LoadScene(EScene.Lobby);
                    return;
                }

                Managers.UI.ClosePopupUI();
                if (!string.IsNullOrEmpty(error))
                {
                    Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo($"유저 정보 불러오기 실패\n\n{error}", showConfirmButton:true);
                    return;
                }

                GetGameObject((int)GameObjects.SelectLoginPanel).gameObject.SetActive(true);
            });
        });
        
    }
}

