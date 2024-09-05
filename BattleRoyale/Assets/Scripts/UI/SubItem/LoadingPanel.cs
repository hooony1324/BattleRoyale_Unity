using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPanel : UI_Base
{
    enum Texts
    {
        LoadingInfoText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));

        return true;
    }

    public void SetLoadingInfoText(string text)
    {
        GetTMPText((int)Texts.LoadingInfoText).text = text;
    }

    
}
