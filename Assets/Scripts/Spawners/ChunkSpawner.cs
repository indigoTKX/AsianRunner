using System;
using UnityEngine;
using Zenject;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private DestroyableEnvironment _chunkPrefab;
    [SerializeField] private float _chunkSize = 10f;
    [SerializeField] private int _chunksCount = 10;

    [Inject] private GameStateManager _gameStateManager;
    
    private DiContainer _diContainer;
    private DestroyableEnvironment _lastChunk;


    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Awake()
    {
        _gameStateManager.OnReset += Start;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnReset -= Start;
    }

    private void Start()
    {
        for (int i = 0; i < _chunksCount; i++)
        {
            SpawnChunk(transform.position + Vector3.forward * _chunkSize * i);
        }
    }

    private void SpawnChunk(Vector3 spawnPoint)
    {
        var lastChunkGameObject = _diContainer.InstantiatePrefab(_chunkPrefab, spawnPoint, Quaternion.identity, null);
        // var lastChunkGameObject = Instantiate(_chunkPrefab, spawnPoint, Quaternion.identity, null);
        _lastChunk = lastChunkGameObject.GetComponent<DestroyableEnvironment>();
            
        _lastChunk.Initialize(
            chunk =>
            {
                var newSpawnPoint = _lastChunk.transform.position + Vector3.forward * _chunkSize;
                RespawnChunk(newSpawnPoint, chunk);
            });
    }

    private void RespawnChunk(Vector3 spawnPoint, DestroyableEnvironment chunk)
    {
        _lastChunk = chunk;
        chunk.transform.position = spawnPoint;
    }
}