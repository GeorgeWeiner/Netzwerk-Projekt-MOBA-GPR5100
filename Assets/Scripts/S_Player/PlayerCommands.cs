using System;
using System.Collections;
using Mirror;
using S_Animations;
using S_Combat;
using UnityEngine;
using UnityEngine.AI;

namespace S_Player
{
    public class PlayerCommands : NetworkBehaviour
    {
        public Targeter targeter;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private NetworkAudioManager audioManager;
        
        public NavMeshAgent agent;
        private AnimationHandler _animationHandler;
        private float turnRate;

        public event Action targetReached;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _animationHandler = GetComponent<AnimationHandler>();
            turnRate = agent.angularSpeed;

        }

        private void Update()
        {
            if (agent.velocity.magnitude < 0.1 && hasAuthority && _animationHandler != null)
            {
                _animationHandler.SetAnimationStateCallback(AnimationStates.Idle);
            }
            
            //if (targeter.GetTarget() == null) return;
            //
            //Cool extension method invocation thing.
            //if (transform.position.Distance(targeter.GetTarget().transform.position) < chaseRange && hasAuthority)
            //{
            //    audioManager.PlayServerAudioFile(AudioFileType.walking, 0);
            //    CmdAttack();
            //}
            //else if (hasAuthority)
            //{
            //    //To stop the command from being called every frame.
            //    if (Time.frameCount % 2 != 0) return;
            //    CmdMoveTowardsAttackTarget();
            //}
        }
        [Command]
        public void CmdMove(Vector3 position)
        {
            targeter.ClearTarget();
            
            var validPosition = 
                NavMesh.SamplePosition(position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
            
            if (!validPosition) return;
            
            agent.SetDestination(hit.position);
            _animationHandler.SetAnimationStateCallback(AnimationStates.Walking);
        }

        [Command]
        public void CmdMoveTowardsAttackTarget()
        {
            var target = targeter.GetTarget();

            agent.SetDestination(target.transform.position);
            _animationHandler.SetAnimationStateCallback(AnimationStates.Walking);
        }
        
        [Command]
        private void CmdAttack()
        {
            agent.ResetPath();
            _animationHandler.SetAnimationStateCallback(AnimationStates.Attacking);
        }

        [Command]
        public void CmdStandStill(float duration)
        {
            StartCoroutine(StationaryCoroutine(duration, Time.time));
        }

        private IEnumerator StationaryCoroutine(float duration, float time)
        {
            agent.ResetPath();
            
            while (Time.time < time + duration)
            {
                agent.velocity = Vector3.MoveTowards(agent.velocity, Vector3.zero, .1f);
                agent.angularSpeed = 10f;
                yield return null;
            }

            agent.angularSpeed = turnRate;
        }
    }
}
