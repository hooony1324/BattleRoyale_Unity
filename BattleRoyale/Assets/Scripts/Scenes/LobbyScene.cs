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

        SceneType = EScene.Lobby;
        Managers.Scene.SetCurrentScene(this);

        Managers.UI.ShowSceneUI<UI_LobbyScene>();

        return true;
    }


    public override void Clear()
    {
        
    }
}
