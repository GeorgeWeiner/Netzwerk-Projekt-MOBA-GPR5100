using TMPro;
using UnityEngine;

public class JoinOrHostLobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField adressInput;

    /// <summary>
    /// Joins the game via a button callback with the given adress from the input name field
    /// </summary>
    public void JoinGame()
    {
        string address = adressInput.text;
        MobaNetworkRoomManager.singleton.networkAddress = address;
        MobaNetworkRoomManager.singleton.StartClient();
    }
    /// <summary>
    /// Starts hosting a game
    /// </summary>
    public void HostGame()
    {
        MobaNetworkRoomManager.singleton.StartHost();
    }
}
