using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ManagerInstaller", menuName = "Installers/ManagerInstaller")]
public class ManagerInstaller : ScriptableObjectInstaller<ManagerInstaller>
{
    [SerializeField] private InputManager _inputManagerPrefab;
    // [SerializeField] private WorldMovementManager _worldMovementManager;
    // [SerializeField] private GameStateManager _gameStateManagerPrefab;
    [SerializeField] private AudioManager _audioManagerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<InputManager>().FromComponentInNewPrefab(_inputManagerPrefab).AsSingle();
        // Container.Bind<GameStateManager>().FromComponentInNewPrefab(_gameStateManagerPrefab).AsSingle();
        // Container.Bind<WorldMovementManager>().FromComponentInNewPrefab(_worldMovementManager).AsSingle();
        Container.Bind<AudioManager>().FromComponentInNewPrefab(_audioManagerPrefab).AsSingle();
    }
}