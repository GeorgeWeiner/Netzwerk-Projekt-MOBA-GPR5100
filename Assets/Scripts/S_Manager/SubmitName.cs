using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;

public class SubmitName : NetworkBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject champSelect;
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
        gameObject.SetActive(false);
    }
}
