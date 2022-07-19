using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Animations;
using S_Combat;
using UnityEngine;
using UnityEngine.AI;

public abstract class Attack : NetworkBehaviour
{
    [SerializeField] float attackRange = 5f;
    [SerializeField] protected int dmg = 4;
    [SerializeField] protected float attackDelay = 0.4f;
    [SerializeField] protected float timeBeetweenAttacks = 1f;
    protected Coroutine attackRoutine;
    protected Targeter targeter;
    NavMeshAgent agent;
    protected bool canAttack = true;
    protected AnimationHandler animationHandler;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targeter = GetComponent<Targeter>();
        animationHandler = GetComponent<AnimationHandler>();
    }
    void Update()
    {
        CheckIfIsInAttackRange();
    }
    void CheckIfIsInAttackRange()
    {
        if (targeter.GetTarget() == null || !hasAuthority) return;
        {
            if (canAttack && transform.position.GetSqredDistance(targeter.GetTarget().transform.position) < attackRange)
            {
                agent.ResetPath();
                LookAtTarget();
                attackRoutine = StartCoroutine(AttackTarget(targeter.GetTarget().GetComponent<IDamageable>()));
            }
        }

    }

    void LookAtTarget()
    {
        Vector3 direction = targeter._target.transform.position - transform.position;
        direction.Normalize();
        transform.forward = direction;
    }
    protected abstract IEnumerator AttackTarget(IDamageable target);
    void OnDrawGizmosSelected()
    {
       gameObject.DrawCircle(attackRange,0.1f);
    }
}
