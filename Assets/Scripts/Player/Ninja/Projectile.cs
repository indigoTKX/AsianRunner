using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float Damage = 50f;
    [SerializeField] public float Speed = 10f;
    [Space]
    [SerializeField] private float _maxLifeTime = 5f;

    [SerializeField] private string _poolUid;

    [Inject] private ObjectPooler _objectPooler;
    private Transform _transform;
    private IEnumerator _returnCoroutine;

    // private void Awake()
    // {
    //     _transform = transform;
    //     // Destroy(gameObject, _maxLifeTime);
    //     _returnCoroutine = ReturnToPoolWithDelay(_maxLifeTime);
    //     StartCoroutine(_returnCoroutine);
    // }

    private void OnEnable()
    {
        //Debug.Log("instantiated shuriken");
        
        _transform = transform;
        // Destroy(gameObject, _maxLifeTime);
        _returnCoroutine = ReturnToPoolWithDelay(_maxLifeTime);
        StartCoroutine(_returnCoroutine);
    }

    private void OnDisable()
    {
        // Debug.Log("destroyed shuriken");
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        damageable?.DealDamage(Damage);
        
        // Destroy(gameObject);
        StopCoroutine(_returnCoroutine);
        _objectPooler.ReturnToPool(gameObject, _poolUid);
    }

    private void FixedUpdate()
    {
        _transform.position += _transform.forward * (Speed * Time.fixedDeltaTime);
    }

    private IEnumerator ReturnToPoolWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _objectPooler.ReturnToPool(gameObject, _poolUid);
    }
}   