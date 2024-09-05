using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using UnityEngine;



public class UI_LoginScene : UI_Scene
{
    enum Panels
    {
        LoginPanel,
        LoadingPanel,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(Panels));

        GetGameObject((int)Panels.LoginPanel).SetActive(false);
        GetGameObject((int)Panels.LoadingPanel).SetActive(true);

        return true;
    }

    public void ShowLoginPanel()
    {
        GetGameObject((int)Panels.LoginPanel).SetActive(true);
        GetGameObject((int)Panels.LoadingPanel).SetActive(false);
    }

}
