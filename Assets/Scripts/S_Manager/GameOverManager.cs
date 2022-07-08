using System;
using Mirror;
using UnityEngine;

namespace S_Manager
{
    public class GameOverManager : NetworkBehaviour
    {
        public static event Action OnGameOver;
            
        public static void HandleGameOver()
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
        }
    }
}