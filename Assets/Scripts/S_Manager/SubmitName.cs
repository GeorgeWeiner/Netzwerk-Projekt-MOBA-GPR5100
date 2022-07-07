using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitName : NetworkBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button submitButton;
    [SerializeField] GameObject champSelect;
    [SerializeField] DisplayNamesInChampSelect displayNames;
    MobaPlayer player;
    NetworkRoomPlayer roomPlayer;

    void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<MobaPlayer>();
        roomPlayer = NetworkClient.connection.identity.GetComponent<NetworkRoomPlayer>();
    }
    public void ChangeName()
    {
        player.CmdChangeName(nameInput.text);
        champSelect.SetActive(true);
        submitButton.gameObject.SetActive(false);
    }
}
