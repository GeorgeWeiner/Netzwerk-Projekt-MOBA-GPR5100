using UnityEngine;
using UnityEngine.SceneManagement;

namespace S_HUD
{
    public class ButtonSwitchSceneResponse : ButtonResponse
    {
        public string sceneName;
        
        public override void TriggerButtonResponse()
        {
            Debug.Log($"Loading Scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }
}