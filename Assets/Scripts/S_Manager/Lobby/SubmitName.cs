using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class which handles changing the name of a player
/// </summary>
public class SubmitName : NetworkBehaviour
{
    [SerializeField] ChampSelect champSelection;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject submitMenu;
    [SerializeField] GameObject champSelect;
    
    public void ChangeName()
    {
        champSelection.UpdatePlayersName(nameInput.text);
        champSelect.SetActive(true);
        submitMenu.SetActive(false);
    }
}
