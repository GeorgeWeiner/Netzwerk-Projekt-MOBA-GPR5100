﻿using System;
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
        private static readonly int State = Animator.StringToHash("AnimationState");

        public void SetAnimationStateCallback(AnimationStates animationState)
        {
            if (!hasAuthority) return;
            SetAnimationState(animationState);
        }
        
        [Command]
        public void SetAnimationState(AnimationStates animationState)
        {
            animator.SetInteger(State, (int)animationState);
        }
    }
}