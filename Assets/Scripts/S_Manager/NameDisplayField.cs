using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class NameDisplayField : NetworkBehaviour
{
    [SerializeField] DisplayNamesInChampSelect display;
    [SerializeField] TMP_Text playerNameField;
    [SyncVar(hook = nameof(UpdatePlayerName))] public string playerName;

    void Awake()
    {
        Debug.Log("ADdded");
        display.names.Add(this);
    }
    void UpdatePlayerName(string old, string newName)
    {
        playerNameField.text = newName;
    }
}
