using System;
using Mirror;
using S_Animations;
using S_Combat;
using UnityEngine;
using UnityEngine.AI;

namespace S_Player
{
    public class PlayerCommands : NetworkBehaviour
    {
        [SerializeField] private Targeter targeter;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] NetworkAudioManager audioManager;
        
        private NavMeshAgent _agent;
        private AnimationHandler _animationHandler;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animationHandler = GetComponent<AnimationHandler>();
        }

        private void Update()
        {
            if (_agent.velocity.magnitude < 0.3 && hasAuthority && _animationHandler != null)
            {
              
                _animationHandler.SetAnimationStateCallback(AnimationStates.Idle);
                audioManager.PlayServerAudioFile(AudioFileType.walking, 1);
            }
            
            if (targeter.GetTarget() == null) return;

            //Cool extension method invocation thing.
            if (transform.position.Distance(targeter.GetTarget().transform.position) < chaseRange && hasAuthority)
            {
                audioManager.PlayServerAudioFile(AudioFileType.walking, 0);
                CmdAttack();
            }
            else if (hasAuthority)
            {
                //To stop the command from being called every frame.
                if (Time.frameCount % 30 != 0) return;
                CmdMoveTowardsAttackTarget();
            }
        }
        [Command]
        public void CmdMove(Vector3 position)
        {
            targeter.ClearTarget();
            
            var validPosition = 
                NavMesh.SamplePosition(position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
            
            if (!validPosition) return;
            
            _agent.SetDestination(hit.position);
            _animationHandler.SetAnimationStateCallback(AnimationStates.Walking);
        }

        [Command]
        public void CmdMoveTowardsAttackTarget()
        {
            var target = targeter.GetTarget();

            _agent.SetDestination(target.transform.position);
            _animationHandler.SetAnimationStateCallback(AnimationStates.Walking);
        }
        
        [Command]
        public void CmdAttack()
        {
            _agent.ResetPath();
            _animationHandler.SetAnimationStateCallback(AnimationStates.Attacking);
        }
    }
}
