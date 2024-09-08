using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_JoystickLeft : UI_Base 
{
    enum GameObjects
    {
        Background,
        ControlStick,
    }

    private RectTransform _background;
    private RectTransform _controlStick;
    private float _radius;
    private Vector2 _originPos;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

        _background = GetGameObject((int)GameObjects.Background).GetComponent<RectTransform>();
        _controlStick = GetGameObject((int)GameObjects.ControlStick).GetComponent<RectTransform>();
        _radius = _background.GetComponent<RectTransform>().rect.width * 0.5f;

        gameObject.BindEvent(OnPointerDown, type: EUIEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, type: EUIEvent.PointerUp);
        gameObject.BindEvent(OnDrag, type: EUIEvent.Drag);



        return true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("드래그중...");
        _controlStick.anchoredPosition = Vector2.zero;
        Managers.Game.MoveDir = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 clampedPos = eventData.position - (Vector2)_background.position;
        clampedPos = Vector2.ClampMagnitude(clampedPos, _radius);
        _controlStick.localPosition = clampedPos;

        Managers.Game.MoveDir = clampedPos.normalized;

        Debug.Log("드래그중...");
    }
    
}