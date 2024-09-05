using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPanel : UI_Base
{
    enum Texts
    {
        ResourceNameText,
        PercentageText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));

        return true;
    }


    public void OnLoadingResourceChanged(string name, int current, int total)
    {
        GetTMPText((int)Texts.ResourceNameText).text = name;
        GetTMPText((int)Texts.PercentageText).text = $"{current / total}";
    }
}
