using UnityEngine;
using Random = UnityEngine.Random;

public class GateSpawner : PlayOnlySpawnerBase
    {
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [Space] 
        [SerializeField] private GateView _gatePrefab;

        protected override void Spawn()
        {
            var leftGate= DiContainer.InstantiatePrefabForComponent<GateView>(_gatePrefab, _leftSpawnPoint.position, _leftSpawnPoint.rotation, null);
            var rightGate = DiContainer.InstantiatePrefabForComponent<GateView>(_gatePrefab, _rightSpawnPoint.position, _rightSpawnPoint.rotation, null);

            // var leftGate = Instantiate(_gatePrefab, _leftSpawnPoint.position, _leftSpawnPoint.rotation);
            // var rightGate = Instantiate(_gatePrefab, _rightSpawnPoint.position, _rightSpawnPoint.rotation);

            // int buffLevel = Random.Range(_player.PlayerLevel + 1, _player.MaxLevel + 1);
            // int debuffLevel = Random.Range(0, _player.PlayerLevel + 1);
            var buffLevel = 1;
            var debuffLevel = -1;

            var randomNumber = Random.Range(0, 2);
            if (randomNumber == 1)
            {
                rightGate.SetLevel(buffLevel);
                leftGate.SetLevel(debuffLevel);
            }
            else
            {
                rightGate.SetLevel(debuffLevel);
                leftGate.SetLevel(buffLevel);
            }
        }
    }
