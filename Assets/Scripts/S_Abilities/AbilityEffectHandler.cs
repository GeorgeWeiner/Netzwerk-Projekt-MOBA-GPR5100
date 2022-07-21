using Mirror;
using UnityEngine;
using UnityEngine.VFX;

namespace S_Abilities
{
    public class AbilityEffectHandler : NetworkBehaviour
    {
        //Receive the event of the sub ability.
        //Check weather the sub abilities Effect is null.
        //If the sub ability has an effect, execute it.
    
        //Create reference to the VFX-Graph.
        //Set properties like speed and duration in the VFX-Graph.

        private AbilityHandler _abilityHandler;

        public override void OnStartServer()
        {
            _abilityHandler = GetComponent<AbilityHandler>();
            _abilityHandler.SubAbilityExecuted += VisualEffectCallback; 
        }

        private void VisualEffectCallback(SubAbility subAbility)
        {
            Debug.Log("Attempting to find VFX.");
            if (!hasAuthority) return; 
            if (subAbility.visualEffectAsset == null) return;

            for (var i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).TryGetComponent(out VisualEffect asset)) continue;
                //Make sure this comparison is valid.
                if (asset.visualEffectAsset == subAbility.visualEffectAsset)
                {
                    Debug.Log("Found the VFX.");
                    CmdPlayVisualEffect(transform.GetChild(i).gameObject, subAbility.visualEffectPlaybackSpeed);
                }
            }
        }

        [Command]
        private void CmdPlayVisualEffect(GameObject subAbilityPrefab, float playbackSpeed)
        {
            var effectAsset = subAbilityPrefab.GetComponent<VisualEffect>();
            effectAsset.playRate = playbackSpeed;
            effectAsset.Play();
        }
    }
}
