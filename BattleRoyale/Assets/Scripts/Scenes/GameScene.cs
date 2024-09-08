using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.ShowSceneUI<UI_GameScene>();
        
        GameObject player = Managers.Resource.Instantiate("Player");
        player.transform.position = new Vector3(0, 0, 0);

        var cam = Camera.main.transform.GetComponent<CinemachineVirtualCamera>();
        cam.LookAt = player.transform;
        cam.Follow = player.transform;
        
        return true;
    }


    public override void Clear()
    {
        
    }
}
