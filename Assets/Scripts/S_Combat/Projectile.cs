using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public abstract class Projectile : NetworkBehaviour
{
    protected float projectileSpeed;
    protected int dmg;
    
    public abstract void Initialize(Transform forward, Targetable target, int dmg, float projectileSpeed);
}
