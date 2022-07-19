using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public class BaseAttackProjectile : Projectile
{
    public Targetable target;

    void Update()
    {
        MoveTowardsTarget();
    }
    /// <summary>
    /// Initializes the projectile for example if you spawn it so that you can get the component of it
    /// </summary>
    /// <param name="forward"></param>
    /// <param name="target"></param>
    /// <param name="dmg"></param>
    /// <param name="projectileSpeed"></param>
    public override void Initialize(Transform forward, Targetable target, int dmg, float projectileSpeed)
    {
        this.dmg = dmg;
        this.target = target;
        this.projectileSpeed = projectileSpeed;
        transform.forward = forward.forward;
    }
    /// <summary>
    /// Moves the projectile towards the target
    /// </summary>
    void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.GetAimAtPoint().position, Time.deltaTime * 5);
        }
    }
    /// <summary>
    /// Deals dgm to the target youve selected
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Targetable>(out var target))
        { 
            if (target == this.target)
            {
                target.GetComponent<IDamageable>().TakeDmg(dmg);
                Debug.Log(target.GetComponent<Health>().CurrentValue);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
