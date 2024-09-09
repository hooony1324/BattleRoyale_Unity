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
                GetGameObject((int)GameObjects.SelectLoginPanel).gameObject.SetActive(true);
                break;
        }
    }

}

