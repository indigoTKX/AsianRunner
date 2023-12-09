using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NinjaWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private string _shurikenPoolUid;
    [SerializeField] private Transform _handTransform;

    [Inject] private ObjectPooler _objectPooler;
    private NinjaAnimationController _animationController;
    
    private void OnEnable()
    {
        _animationController = GetComponent<NinjaAnimationController>();
        _animationController.OnShurikenThrown += SpawnProjectile;
    }

    private void OnDisable()
    {
        _animationController.OnShurikenThrown -= SpawnProjectile;
    }

    private void SpawnProjectile()
    {
        // Instantiate(_projectilePrefab, _handTransform.position, transform.rotation);
        var shuriken = _objectPooler.GetPooledObject(_shurikenPoolUid);
        shuriken.transform.position = _handTransform.position;
        shuriken.transform.rotation = transform.rotation;
    }
}
