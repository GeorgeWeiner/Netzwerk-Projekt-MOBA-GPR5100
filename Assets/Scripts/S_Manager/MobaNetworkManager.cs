using Mirror;
using UnityEngine;

namespace S_Manager
{
    public class MobaNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject heroPrefab;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            //base.OnServerAddPlayer(conn);
            //var heroInstance = Instantiate(heroPrefab, conn.identity.transform.position,
            //    conn.identity.transform.rotation);
            //NetworkServer.Spawn(heroInstance, conn);
        }
    }
}
