using System.Collections;
using UnityEngine;
using Zenject;

public abstract class SpawnerWithPeriodBase : MonoBehaviour
{
    [SerializeField] protected float SpawnDelay = 1f;
        
    protected DiContainer DiContainer;

    private bool _isSpawning = false;
    private IEnumerator _spawnCoroutine;
    
    [Inject]
    private void Construct(DiContainer diContainer)
    {
        DiContainer = diContainer;
    }
    
    protected void StartSpawning()
    {
        if (_isSpawning) return;
        _isSpawning = true;
        _spawnCoroutine = ConstantDelayedSpawn();
        StartCoroutine(_spawnCoroutine);
    }

    protected void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
        _isSpawning = false;
    }

    protected abstract void Spawn();

    private IEnumerator ConstantDelayedSpawn()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(SpawnDelay);    
        }
    }
}

