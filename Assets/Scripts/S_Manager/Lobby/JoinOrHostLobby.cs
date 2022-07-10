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
        string adress = adressInput.text;
        MobaNetworkRoomManager.singleton.networkAddress = adress;
        MobaNetworkRoomManager.singleton.StartClient();
    }

    public void HostGame()
    {
        MobaNetworkRoomManager.singleton.StartHost();
    }
}
