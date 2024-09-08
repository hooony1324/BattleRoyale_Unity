using UnityEngine;
using UnityEngine.EventSystems;

public class UI_LobbyScene : UI_Scene
{
    enum Buttons
    {
        StartButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(StartGame);

        return true;
    }

    void StartGame(PointerEventData pointerEventData)
    {
        Managers.Scene.LoadScene(EScene.Game);
    }
}