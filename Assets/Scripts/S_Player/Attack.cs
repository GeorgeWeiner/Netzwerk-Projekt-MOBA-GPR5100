using System.Collections;
using Mirror;
using S_Animations;
using S_Combat;
using S_Extensions;
using UnityEngine;
using UnityEngine.AI;

public abstract class Attack : NetworkBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] protected int dmg = 4;
    [SerializeField] protected float attackDelay = 0.4f;
    [SerializeField] protected float timeBeetweenAttacks = 1f;
    protected Coroutine attackRoutine;
    protected Targeter targeter;
    private NavMeshAgent agent;
    protected bool canAttack = true;
    protected AnimationHandler animationHandler;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targeter = GetComponent<Targeter>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Update()
    {
        CheckIfIsInAttackRange();
    }

    private void CheckIfIsInAttackRange()
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

    private void LookAtTarget()
    {
        Vector3 direction = targeter._target.transform.position - transform.position;
        direction.Normalize();
        transform.forward = direction;
    }
    protected abstract IEnumerator AttackTarget(IDamageable target);

    private void OnDrawGizmosSelected()
    {
       gameObject.DrawCircle(attackRange,0.1f);
    }
}
