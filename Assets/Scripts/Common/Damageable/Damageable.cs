using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;
        
    public event Action OnDie;
    public event Action<float> OnDamaged;
        
    public float CurrentHealth { get; private set; }

    public bool IsAlive { get; private set; } = true;

    public void DealDamage(float damage)
    {
        if(!IsAlive) return;
            
        CurrentHealth -= damage;
        OnDamaged?.Invoke(damage);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    
    protected void Start()
    {
        CurrentHealth = _maxHealth;
        IsAlive = CurrentHealth > 0;
    }

    private void Die()
    {
        OnDie?.Invoke();
        IsAlive = false;
    }
}