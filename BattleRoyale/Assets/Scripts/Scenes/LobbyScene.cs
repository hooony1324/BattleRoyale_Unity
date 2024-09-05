using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo("테스트입니당");

        return true;
    }


    public override void Clear()
    {
        
    }
}
