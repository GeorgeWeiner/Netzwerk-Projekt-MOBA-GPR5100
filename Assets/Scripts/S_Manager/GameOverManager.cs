using System;
using UnityEngine;

namespace S_Manager
{
    public class GameOverManager : BaseSingleton<GameOverManager>
    {
        public static event Action OnGameOver;
            
        public static void HandleGameOver()
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
        }
    }
}