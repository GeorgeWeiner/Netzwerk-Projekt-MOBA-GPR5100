using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class JoinOrHostLobby : MonoBehaviour
{
    [SerializeField] TMP_InputField adressInput;

    public void JoinGame()
    {
        string address = adressInput.text;
        MobaNetworkRoomManager.singleton.networkAddress = address;
        MobaNetworkRoomManager.singleton.StartClient();
    }

    public void HostGame()
    {
        MobaNetworkRoomManager.singleton.StartHost();
    }
}
