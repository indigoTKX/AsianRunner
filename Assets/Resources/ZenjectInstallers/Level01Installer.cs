using UnityEngine;
using Zenject;

public class Level01Installer : MonoInstaller
{
    [SerializeField] private SquadController _playerSquadController;
    [SerializeField] private GateSpawner _gateSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private WorldMovementManager _worldMovementManager;
    [SerializeField] private ObjectPooler _objectPooler;
    
    public override void InstallBindings()
    {
         Container.Bind<SquadController>().FromInstance(_playerSquadController).AsSingle();
         Container.Bind<GateSpawner>().FromInstance(_gateSpawner).AsSingle();
         Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
         Container.Bind<GameStateManager>().FromInstance(_gameStateManager).AsSingle();
         Container.Bind<WorldMovementManager>().FromInstance(_worldMovementManager).AsSingle();
         Container.Bind<ObjectPooler>().FromInstance(_objectPooler).AsSingle();
    }
}
