using System;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _damage = 25f;

    [Inject] private GameStateManager _gameStateManager;
    [Inject] private SquadController _playerController;

    private Transform _playerTransform;
    private Damageable _playerDamageable;
    
    private Transform _transform;
    private Damageable _damageable;
    private SamuraiAnimationController _animationController; 

    private bool _canMove = true;
    private bool _canAttack = true;
    
    private void OnEnable()
    {
        _transform = transform;

        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
        HandleOnGameStateChanged(_gameStateManager.GetCurrentState());
        
        _playerTransform = _playerController.transform;
        _playerDamageable = _playerController.GetComponent<Damageable>();

        _damageable = GetComponent<Damageable>();
        _damageable.OnDie += HandleOnDie;

        _animationController = GetComponent<SamuraiAnimationController>();
        _animationController.OnAttacked += TryDealDamage;
        _animationController.OnDied += Destroy;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
        _damageable.OnDie -= HandleOnDie;
        _animationController.OnAttacked -= TryDealDamage;
        _animationController.OnDied -= Destroy;
    }

    private void FixedUpdate()
    {
        var position = _transform.position;
        var moveDirection = _playerTransform.position - position;

        var distanceToPlayer = moveDirection.magnitude;
        if (distanceToPlayer <= _attackRange && _canAttack)
        {
            _animationController.PlayAttackAnimation();
        }
        
        var desiredPosition = position + moveDirection.normalized * (_moveSpeed * Time.fixedDeltaTime);

        if (!_canMove) return;
        _transform.position = desiredPosition;
        _transform.LookAt(_playerTransform);
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        _canMove = state is GameState.RUN;
    }

    private void HandleOnDie()
    {
        _canMove = false;
        _canAttack = false;
    }

    private void TryDealDamage()
    {
        var vectorToPlayer = _playerTransform.position - _transform.position;
        var distanceToPlayer = vectorToPlayer.magnitude;
        if (distanceToPlayer > _attackRange || !_canAttack) return;
        
        _playerDamageable.DealDamage(_damage);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
