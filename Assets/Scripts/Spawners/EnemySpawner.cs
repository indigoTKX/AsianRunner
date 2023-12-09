using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : PlayOnlySpawnerBase
{
    [SerializeField] private Transform _baseSpawnPoint;
    [SerializeField] private float _maxSpawnRadius = 4f;
    [SerializeField] private int _maxEnemiesPerSpawn = 3;
    [SerializeField] private List<GameObject> _enemyPrefabs;
      
    protected override void Spawn()
    {
        var enemiesCount = Random.Range(1, _maxEnemiesPerSpawn + 1);

        for (var i = 0; i < enemiesCount; i++)
        {
            var randomOffset2d = Random.insideUnitCircle * _maxSpawnRadius;
            var randomOffset = new Vector3(randomOffset2d.x, 0f, randomOffset2d.y);
            var randomSpawnPoint = _baseSpawnPoint.position + randomOffset;

            var randomEnemyIndex = Random.Range(0, _enemyPrefabs.Count);
            DiContainer.InstantiatePrefab(_enemyPrefabs[randomEnemyIndex], randomSpawnPoint, _baseSpawnPoint.rotation, null);
        }
    }
}