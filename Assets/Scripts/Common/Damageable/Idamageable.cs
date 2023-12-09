using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public event Action<float> OnDamaged;
        
    public bool IsAlive { get; }
        
    public void DealDamage(float damage);
}