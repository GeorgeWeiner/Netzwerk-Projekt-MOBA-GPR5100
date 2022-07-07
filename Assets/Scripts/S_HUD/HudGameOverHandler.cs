using System;
using S_Manager;
using UnityEngine;

namespace S_HUD
{
    public class HudGameOverHandler : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverHUD;
        
        private void Start()
        {
            GameOverManager.OnGameOver += DisplayGameOverScreen;
        }

        private void DisplayGameOverScreen()
        {
            Debug.Log("Game Over event triggered.");
            gameOverHUD.SetActive(true);
        }
    }
}