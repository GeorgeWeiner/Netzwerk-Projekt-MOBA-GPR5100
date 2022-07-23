using S_Manager;
using UnityEngine;
using Mirror;

namespace S_HUD
{
    public class HudGameOverHandler : NetworkBehaviour
    {
        [SerializeField] private GameObject gameOverHUD;

        public override void OnStartServer()
        {
            GameOverManager.OnGameOver += DisplayGameOverScreen;
        }


        //Ist das okay so meinst du?
        [ClientRpc]
        private void DisplayGameOverScreen()
        {
            Debug.Log("Game Over event triggered.");
            gameOverHUD.SetActive(true);
        }
    }
}