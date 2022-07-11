using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{
    public bool PressedAbilityButton() => Keyboard.current.digit1Key.wasPressedThisFrame;
    public bool ClickedRightMouseButton() => Mouse.current.rightButton.wasPressedThisFrame;
    public bool ClickedLeftMouseButton => Mouse.current.leftButton.wasPressedThisFrame;
    public bool HoldingMiddleMouseButton => Mouse.current.middleButton.isPressed;
    public Vector2 GetMousePos() => Mouse.current.position.ReadValue();
    public static event Action<AbilitySlot> OnPressedAbility; 

    public void SetUpPlayerInput(InputActionAsset inputActionAsset)
    {
        NetworkClient.connection.identity.GetComponent<PlayerInput>().actions = inputActionAsset;
    }

    #region Broadcast Messages

    //This might not be the ideal solution. Open for suggestions.
    public void OnAbilityOne()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityOne);
    }
    
    public void OnAbilityTwo()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityTwo);
    }
    
    public void OnAbilityThree()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityThree);
    }
    
    public void OnAbilityFour()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityFour);
    }
    
    public void OnAbilityFive()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityFive);
    }
    
    public void OnAbilityUltimate()
    {
        OnPressedAbility?.Invoke(AbilitySlot.AbilityUltimate);
    }

    #endregion
}
