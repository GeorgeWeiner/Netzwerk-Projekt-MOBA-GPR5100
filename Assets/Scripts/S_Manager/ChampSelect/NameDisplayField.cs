using Mirror;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays the name of a player
/// </summary>
public class NameDisplayField : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerNameField;
    [SyncVar(hook = nameof(UpdatePlayerName))] public string playerName;

    [ClientRpc]
    public void UpdatePlayerName(string newName)
    {
        playerName = newName;
    }

    private void UpdatePlayerName(string old, string newName)
    {
        playerNameField.text = newName;
    }
}
