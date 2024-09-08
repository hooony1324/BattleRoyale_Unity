using UnityEngine;

public class PlayerController : InitOnce
{
    Vector2 _moveDir = Vector2.zero;

    public float Speed {get; set;} = 5.0f;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;


        return true;
    }

    private void Update()
    {
        Vector3 dir = new Vector3(_moveDir.x, 0, _moveDir.y);
        Vector3 newPos = transform.position + dir * Time.deltaTime * Speed;;

        // newPos가능한지 체크

        transform.position = newPos;
    }

    void HandleOnMoveDirChanged(Vector2 moveDir)
    {
        _moveDir = moveDir;

        
    }
}