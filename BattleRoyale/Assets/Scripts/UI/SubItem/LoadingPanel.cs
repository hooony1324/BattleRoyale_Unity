using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;

public class LoadingPanel : UI_Base
{
    enum Texts
    {
        LoadingInfoText,
    }

    public void Init()
    {
        BindTMPTexts(typeof(Texts));

        Managers.Scene.GetCurrentScene<LoginScene>().OnDownloadStateStateChanged += OnDownloadStateChanged;
    }

    private void OnDisable()
    {
        Managers.Scene.GetCurrentScene<LoginScene>().OnDownloadStateStateChanged -= OnDownloadStateChanged;
    }

    public void OnDownloadStateChanged(string text)
    {
        GetTMPText((int)Texts.LoadingInfoText).text = text;
    }

    
}
