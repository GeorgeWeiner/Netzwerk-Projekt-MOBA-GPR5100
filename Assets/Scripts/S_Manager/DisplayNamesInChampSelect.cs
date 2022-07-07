using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayNamesInChampSelect : MonoBehaviour
{
    [SerializeField] TMP_Text[] playerNames;

    void Update()
    {
        for (int i = 0; i < MobaNetworkRoomManager.players.Count; i++)
        {
            playerNames[i].text = MobaNetworkRoomManager.players[i].playerName;
        }
    }
}
