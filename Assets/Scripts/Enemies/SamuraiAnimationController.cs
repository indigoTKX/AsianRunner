using System;
using UnityEngine;
using Zenject;

public class SamuraiAnimationController : MonoBehaviour
{
    public event Action OnAttacked; 
    public event Action OnDied;
    
    [SerializeField] private GameObject _dieSfxPrefab;
    [SerializeField] private float _dieSfxLifeTime = 2f;
    [Space]
    [SerializeField] private GameObject _hitSfxPrefab;
    [SerializeField] private float _hitSfxLifeTime = 1f;
    [SerializeField] private string _hitSoundUid;
    
    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(ATTACK_ANIMATION_TRIGGER_UID);
    }
    
    //called via animation event
    public void Attack()
    {
        OnAttacked?.Invoke();
    }
    
    //called via animation event
    public void Die()
    {
        var dieSfx = _diContainer.InstantiatePrefab(_dieSfxPrefab, transform.position, Quaternion.identity, null);
        Destroy(dieSfx, _dieSfxLifeTime);
        OnDied?.Invoke();
    }
    
    private const string DEATH_ANIMATION_TRIGGER_UID = "Die";
    private const string ATTACK_ANIMATION_TRIGGER_UID = "Attack";

    [Inject] private AudioManager _audioManager;
    
    private Animator _animator;
    private Damageable _damageable;
    
    private DiContainer _diContainer;
    
    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _damageable.OnDamaged += HandleOnHit;
        _damageable.OnDie += PlayDeathAnimation;
    }

    private void OnDestroy()
    {
        _damageable.OnDamaged -= HandleOnHit;
        _damageable.OnDie -= PlayDeathAnimation;
    }

    private void PlayDeathAnimation()
    {
        _animator.SetTrigger(DEATH_ANIMATION_TRIGGER_UID);
    }

    private void HandleOnHit(float dmg)
    {
        _audioManager.PlayAudio(_hitSoundUid);
            
        // var hitSfx = _diContainer.InstantiatePrefab(_hitSfxPrefab, transform.position, Quaternion.identity, null);
        // Destroy(hitSfx, _hitSfxLifeTime);
    }
}
