using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{
    public bool PressedAbilityButton() => Keyboard.current.digit1Key.wasPressedThisFrame;
    public bool ClickedRightMouseButton() => Mouse.current.rightButton.wasPressedThisFrame;
    public bool ClickedLeftMouseButton => Mouse.current.leftButton.wasPressedThisFrame;
    public bool HoldingMiddleMouseButton => Mouse.current.middleButton.isPressed;
    public Vector2 GetMousePos() => Mouse.current.position.ReadValue();
}
