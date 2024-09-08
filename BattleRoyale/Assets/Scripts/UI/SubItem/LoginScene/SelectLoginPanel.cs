using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectLoginPanel : UI_Base
{
    enum Buttons
    {
        LoginButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(TestLobbyStart);

        return true;
    }

    void TestLobbyStart(PointerEventData pointerEventData)
    {
        Managers.Scene.LoadScene(EScene.Lobby);
    }
}