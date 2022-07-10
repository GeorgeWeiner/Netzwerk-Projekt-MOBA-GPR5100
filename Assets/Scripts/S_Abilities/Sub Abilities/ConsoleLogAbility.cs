using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Console-Log", fileName = "New Console-Log Ability")]
    public class ConsoleLogAbility : SubAbility
    {
        public override void ExecuteSubAbility()
        {
            Debug.Log("Executing basic console log ability.");
        }
    }
}