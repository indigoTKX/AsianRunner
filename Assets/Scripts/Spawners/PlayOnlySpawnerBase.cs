using System;
using Zenject;

public abstract class PlayOnlySpawnerBase : SpawnerWithPeriodBase
{
    [Inject] private GameStateManager _gameStateManager;

    private void Awake()
    {
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
        HandleOnGameStateChanged(_gameStateManager.GetCurrentState());
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.RUN:
                StartSpawning();
                break;
            default:
                StopSpawning();
                break;
        }
    }
}
