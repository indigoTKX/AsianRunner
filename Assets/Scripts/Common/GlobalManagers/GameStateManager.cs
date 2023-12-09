using System;
using UnityEngine;
using Zenject;

public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnStateChanged;
    public event Action OnReset;

    [SerializeField] private string _mainMenuThemeUid;
    [SerializeField] private string _runningThemeUid;
    [SerializeField] private string _startRunSoundUid;
    [SerializeField] private string _loseSoundUid;

    public GameState GetCurrentState()
    {
        return _currentState;
    }

    public void SetTutorialState()
    {
        _audioManager.StopAudio(_mainMenuThemeUid);
        _audioManager.PlayAudio(_startRunSoundUid);
        _audioManager.PlayAudio(_runningThemeUid);    
        SetState(GameState.TUTORIAL);
    }
    
    public void SetRunState(bool withAudio = true)
    {
        if (withAudio)
        {
            _audioManager.StopAudio(_mainMenuThemeUid);
            _audioManager.PlayAudio(_startRunSoundUid);
            _audioManager.PlayAudio(_runningThemeUid);    
        }
        SetState(GameState.RUN);
    }
    
    public void ResetGame()
    {
        OnReset?.Invoke();
        SetRunState();
    }

    [Inject] private AudioManager _audioManager;
    [Inject] private SquadController _playerController;
    
    private GameState _currentState = GameState.MAIN_MENU;

    private Damageable _playerDamageable;

    private void Awake()
    {
        _playerDamageable = _playerController.GetComponent<Damageable>();
        _playerDamageable.OnDie += SetGameOver;
    }
    
    private void OnDestroy()
    {
        _playerDamageable.OnDie -= SetGameOver;
    }

    private void Start()
    {
        SetState(GameState.MAIN_MENU);
        _audioManager.PlayAudio(_mainMenuThemeUid);
    }

    private void SetGameOver()
    {
        SetState(GameState.GAME_OVER);
        _audioManager.StopAudio(_runningThemeUid);
        _audioManager.PlayAudio(_loseSoundUid);
        _audioManager.PlayAudio(_mainMenuThemeUid);
    }
    
    private void SetState(GameState state)
    {
        _currentState = state;
        OnStateChanged?.Invoke(_currentState);
    }
}
    
public enum GameState
{
    MAIN_MENU,
    TUTORIAL,
    RUN,
    GAME_OVER
}


