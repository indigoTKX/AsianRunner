using System;
using UnityEngine;
using Zenject;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private string _footstepsAudioUid;
    [Space]
    [SerializeField] private GameObject _hurtSfx;
    [SerializeField] private float _hurtSfxLifetime = 2f;
    [SerializeField] private string _hurtSoundUid;

    [Inject] private AudioManager _audioManager;
    [Inject] private GameStateManager _gameStateManager;

    private SquadController _playerController;
    private Damageable _damageable;
    private NinjaAnimationController _initialNinja;

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.OnDamaged += HandleOnDamaged;

        _playerController = GetComponent<SquadController>();
        _initialNinja = _playerController.GetAliveMember().GetComponent<NinjaAnimationController>();
        _initialNinja.SerRunning(false);
        
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
        _gameStateManager.OnReset += ReviveNinja;
    }

    private void OnDestroy()
    {
        _damageable.OnDamaged -= HandleOnDamaged;
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
        _gameStateManager.OnReset -= ReviveNinja;
    }

    private void HandleOnDamaged(float damage)
    {
        _audioManager.PlayAudio(_hurtSoundUid);
        var sfx = Instantiate(_hurtSfx, transform);
        Destroy(sfx, _hurtSfxLifetime);
    }
    
    private void HandleOnGameStateChanged(GameState state)
    {
        if (state is GameState.RUN)
        {
            _audioManager.PlayAudio(_footstepsAudioUid);
            _initialNinja = _playerController.GetAliveMember().GetComponent<NinjaAnimationController>();
            _initialNinja.SerRunning(true);
        }
        else
        {
            _audioManager.StopAudio(_footstepsAudioUid);
        }
    }

    private void ReviveNinja()
    {
        _initialNinja = _playerController.GetAliveMember().GetComponent<NinjaAnimationController>();
        _initialNinja.Revive();
    } 
}
