using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<Vector2> OnMoveDirChanged;
    private Vector2 _moveDir = Vector2.zero;
    public Vector2 MoveDir 
    {
        get { return _moveDir; }
        set 
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }


}
