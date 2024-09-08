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
        LoginPanels,
        LoadingPanel,
    }
    public enum ELoginSection
    {
        SelectLogin,
        SignIn,
        SignUp,

        Count,
        Idle,
    }

    private ELoginSection _loginSection = ELoginSection.Idle;
    public ELoginSection LoginSection
    {
        get { return _loginSection; }
        set
        {
            if (_loginSection != value)
            {
                _loginSection = value;
                for (int i = 0; i < (int)ELoginSection.Count; i++)
                {
                    _uiPanels[i].SetActive(i == (int)_loginSection);
                }
            }
        }
    }

    private GameObject[] _uiPanels = new GameObject[(int)ELoginSection.Count];
    public void Setup()
    {
        BindGameObjects(typeof(GameObjects));

        int index = 0;
        foreach (Transform child in GetGameObject((int)GameObjects.LoginPanels).transform)
        {
            _uiPanels[index] = child.gameObject;
            index++;
        }
        GetGameObject((int)GameObjects.LoadingPanel).gameObject.SetActive(true);

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
                LoginSection = ELoginSection.SelectLogin;
                break;
        }
    }

}

