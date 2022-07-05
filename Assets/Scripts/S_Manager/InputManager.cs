using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{

    public bool ClickedRightMouseButton()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return false;
        {
            return true;
        }
    }
    public Vector2 GetMousePos()
    {
        return Mouse.current.position.ReadValue();
    }

}
