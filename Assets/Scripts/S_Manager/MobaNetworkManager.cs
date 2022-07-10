using Mirror;
using UnityEngine;

namespace S_Manager
{
    public class MobaNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject heroPrefab;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            var heroInstance = Instantiate(heroPrefab, conn.identity.transform.position,
                conn.identity.transform.rotation);
            NetworkServer.Spawn(heroInstance, conn);
        }
        
        public static void SpawnPrefab(GameObject go)
        {
            var goInstance = Instantiate(go);
            NetworkServer.Spawn(goInstance);
        }
        
        public static void SpawnPrefab(GameObject go, Transform spawnPoint)
        {
            var goInstance = Instantiate(go, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(goInstance);
        }
        
        public static void SpawnPrefab(GameObject go, NetworkConnectionToClient conn)
        {
            var goInstance = Instantiate(go);
            NetworkServer.Spawn(goInstance, conn);
        }
    }
}
