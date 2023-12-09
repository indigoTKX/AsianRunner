using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _moveRange = 4f;
    [SerializeField] private float _moveSpeed = 5f;

    [Inject] private InputManager _inputManager;
    [Inject] private GameStateManager _gameStateManager;
    
    private Vector3 _initialPosition;
    private Transform _transform;

    private float _minPosition;
    private float _maxPosition;

    private bool _canMove = true;
    
    private void Start()
    {
        _transform = transform;
        _initialPosition = _transform.position;
        
        _minPosition = _initialPosition.x - _moveRange;
        _maxPosition = _initialPosition.x + _moveRange;
        
        _inputManager.OnMoved += HandleMovement;

        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
        HandleOnGameStateChanged(_gameStateManager.GetCurrentState());
    }

    private void OnDestroy()
    {
        _inputManager.OnMoved -= HandleMovement;
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
    }

    private void HandleMovement(Vector2 moveVector)
    {
        if (!_canMove) return;
        
        // Debug.Log(moveVector);
        var desiredPosition = _transform.position + _transform.right * moveVector.x * _moveSpeed * Time.deltaTime;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, _minPosition, _maxPosition);
        
        _transform.position = desiredPosition;
    }

    private void HandleOnGameStateChanged(GameState gameState)
    {
        _canMove = gameState is GameState.RUN;
    }
}
