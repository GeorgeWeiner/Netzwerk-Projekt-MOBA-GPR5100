using Mirror;
using TMPro;
using UnityEngine;

/// <summary>
/// Class which handles changing the name of a player
/// </summary>
public class SubmitName : NetworkBehaviour
{
    [SerializeField] private ChampSelect champSelection;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject submitMenu;
    [SerializeField] private GameObject champSelect;
    
    public void ChangeName()
    {
        champSelection.UpdatePlayersName(nameInput.text);
        champSelect.SetActive(true);
        submitMenu.SetActive(false);
    }
}
