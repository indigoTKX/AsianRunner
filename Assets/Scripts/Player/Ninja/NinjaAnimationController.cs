using System;
using UnityEngine;
using Zenject;

public class NinjaAnimationController : MonoBehaviour
{
    public event Action OnShurikenThrown;

    [SerializeField] private string _throwShurikenAudioUid;
    
    //called via animation event
    public void ThrowShuriken()
    {
        _audioManager.PlayAudio(_throwShurikenAudioUid);
        OnShurikenThrown?.Invoke();
    }

    public void Revive()
    {
        _animator.SetTrigger(REVIVE_ANIMATION_TRIGGER_UID);
    }
    
    public void SerRunning(bool isRunning)
    {
        _animator.SetBool(RUN_ANIMATION_PARAMETER_UID, isRunning);
        _animator.SetBool(THROW_ANIMATION_PARAMETER_UID, isRunning);
    }

    private const string DEATH_ANIMATION_TRIGGER_UID = "Die";
    private const string REVIVE_ANIMATION_TRIGGER_UID = "Revive";
    private const string RUN_ANIMATION_PARAMETER_UID = "IsRunning";
    private const string THROW_ANIMATION_PARAMETER_UID = "IsThrowing";
    
    [Inject] private GameStateManager _gameStateManager;
    [Inject] private AudioManager _audioManager;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
        HandleOnGameStateChanged(_gameStateManager.GetCurrentState());
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        if (state is GameState.GAME_OVER)
        {
            _animator.SetTrigger(DEATH_ANIMATION_TRIGGER_UID);
        }
    }
}
