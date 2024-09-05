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
    enum Panels
    {
        LoginPanel,
        LoadingPanel,
    }

    public void Init()
    {
        BindGameObjects(typeof(Panels));

        GetGameObject((int)Panels.LoginPanel).SetActive(false);
        GetGameObject((int)Panels.LoadingPanel).gameObject.SetActive(true);

        Managers.Scene.GetCurrentScene<LoginScene>().OnLoginSceneStateChanged += HandleSceneUI;
    }

    private void OnDestroy()
    {
        Managers.Scene.GetCurrentScene<LoginScene>().OnLoginSceneStateChanged -= HandleSceneUI;
    }

    void HandleSceneUI(ELoginSceneState state)
    {
        switch (state)
        {
            case ELoginSceneState.ResourceLoadFinished:
                GetGameObject((int)Panels.LoginPanel).SetActive(true);
                break;
        }
    }

}
