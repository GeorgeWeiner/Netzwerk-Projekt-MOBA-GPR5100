using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.VFX;

namespace S_Abilities
{
    public class AbilityEffectHandler : NetworkBehaviour
    {
        private AbilityHandler _abilityHandler;
        private NetworkAudioManager _audioManager;

        public override void OnStartClient()
        {
            _abilityHandler = GetComponent<AbilityHandler>();
            _audioManager = GetComponent<NetworkAudioManager>();
            
            _abilityHandler.SubAbilityExecuted += VisualEffectCallback; 
            _abilityHandler.SubAbilityExecuted += SoundEffectCallback; 
        }

        private void VisualEffectCallback(SubAbility subAbility)
        {
            //Authority check doesnt seem to work in this case. The client player-units don't get authority assigned.
            
            //if (!hasAuthority) return;
            
            if (subAbility == null)
            {
                Debug.LogWarning("Sub Ability was null. Returning.");
                return;
            }

            if (subAbility.visualEffectAsset == null)
            {
                Debug.Log("Sub Abilities visual effect asset was null. Returning.");
                return;
            }

            /*Loop through this.gameObject's Transform-Tree, and try to find the Visual Effect
            that references the matching VisualEffectAsset. If found, send a play command to the server.*/
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                
                if (!child.TryGetComponent(out VisualEffect asset)) continue;

                if (asset.visualEffectAsset == subAbility.visualEffectAsset)
                {
                    Debug.Log("Found the VFX.");
                    
                    CmdPlayVisualEffect(this.gameObject, i, subAbility.visualEffectPlaybackSpeed, 
                        subAbility.visualEffectDurationInSeconds);
                }
            }
        }

        private void SoundEffectCallback(SubAbility subAbility)
        {
            if (subAbility == null)
            {
                Debug.LogWarning("Sub Ability was null. Returning.");
                return;
            }

            if (subAbility.audioClip == null)
            {
                Debug.Log("Sub Abilities audio clip asset was null. Returning.");
                return;
            }

            if (_audioManager == null)
            {
                Debug.LogError("Audio Manager was null. Returning.");
                return;
            }
            
            _audioManager.PlayServerAudioFile(transform.position, AudioFileType.ability, subAbility.audioClip);
        }

        /*You cannot pass the child directly, as this is null on the server.
        Instead you pass the child's index in the transform-tree. The server can then reference the GameObject.*/
        [ClientRpc]
        private void CmdPlayVisualEffect(GameObject parent, int childIndex, float playbackSpeed, float duration)
        {
            if (parent == null) return;
            StartCoroutine(PlayVisualEffect(parent, childIndex, playbackSpeed, duration));
        }

        private IEnumerator PlayVisualEffect(GameObject parent, int childIndex, float playbackSpeed, float duration)
        {
            var effectAsset = parent.transform.GetChild(childIndex).GetComponent<VisualEffect>();
            
            effectAsset.playRate = playbackSpeed;
            effectAsset.Play();

            yield return new WaitForSeconds(duration);
            
            effectAsset.Stop();
        }
    }
}
