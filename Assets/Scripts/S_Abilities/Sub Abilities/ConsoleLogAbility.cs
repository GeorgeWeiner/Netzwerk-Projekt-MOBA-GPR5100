using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Console-Log", fileName = "New Console-Log Ability")]
    public class ConsoleLogAbility : SubAbility
    {
        public string printMessage;
        public override void ExecuteSubAbility()
        {
            Debug.Log(printMessage);
        }
    }
}