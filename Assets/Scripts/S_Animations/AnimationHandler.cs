using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace S_Animations
{
    public enum AnimationStates
    {
        Idle,
        Walking,
        Attacking,
        Casting
    }
    
    public class AnimationHandler : NetworkBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator{ get => animator; }
        private static readonly int State = Animator.StringToHash("AnimationState");

        [Command]
        public void SetAnimationState(AnimationStates animationState)
        {
            animator.SetInteger(State, (int)animationState);
        }
        //Try implementing animation event handler here
    }
}