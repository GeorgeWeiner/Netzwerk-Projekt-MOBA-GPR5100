using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class NameDisplayField : NetworkBehaviour
{
    [SerializeField] TMP_Text playerNameField;
    [SyncVar(hook = nameof(UpdatePlayerName))] public string playerName;

    [ClientRpc]
    public void UpdatePlayerName(string newName)
    {
        playerName = newName;
    }
    void UpdatePlayerName(string old, string newName)
    {
        playerNameField.text = newName;
    }
}
