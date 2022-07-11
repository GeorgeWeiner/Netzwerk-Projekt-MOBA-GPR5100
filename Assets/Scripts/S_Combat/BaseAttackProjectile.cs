using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public class BaseAttackProjectile : Projectile
{
     Targetable target;

    void Update()
    {
        MoveTowardsTarget();
    }
    public override void Initialize(Transform forward, Targetable target, int dmg, float projectileSpeed)
    {
        this.dmg = dmg;
        this.target = target;
        this.projectileSpeed = projectileSpeed;
        transform.forward = forward.forward;
    }
    void MoveTowardsTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.GetAimAtPoint().position, Time.deltaTime * projectileSpeed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Targetable>(out var target))
        { 
            if (target == this.target)
            {
                target.GetComponent<IDamageable>().TakeDmg(dmg);
                Destroy(gameObject);
            }
        }
    }
}
