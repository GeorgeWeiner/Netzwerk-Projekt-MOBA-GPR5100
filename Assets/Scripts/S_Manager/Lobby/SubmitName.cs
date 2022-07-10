using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitName : NetworkBehaviour
{
    [SerializeField] ChampSelect champSelection;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject submitMenu;
    [SerializeField] GameObject champSelect;
    MobaPlayerData player;
    NetworkRoomPlayer roomPlayer;

    void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
        roomPlayer = NetworkClient.connection.identity.GetComponent<NetworkRoomPlayer>();
    }
    public void ChangeName()
    {
        player.CmdChangeName(nameInput.text);
        roomPlayer.readyToBegin = true;
        champSelect.SetActive(true);
        submitMenu.SetActive(false);
    }
}
