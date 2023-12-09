using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothSpeed = 10f;
    [SerializeField] private Vector3 _desiredOffset;
    [SerializeField] private Quaternion _desiredRotation;
    [SerializeField] private float _rotationTime = 1f;

    [Inject] private GameStateManager _gameStateManager;

    private Transform _transform;
    private bool _rotated = false;

    private void Start()
    {
        _transform = transform;

        _gameStateManager.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        if (_gameStateManager.GetCurrentState() == GameState.MAIN_MENU) return;
        FollowPlayer();
    }

    private void HandleGameStateChanged(GameState gameState)
    {
        if (_rotated) return;

        if (gameState is GameState.TUTORIAL or GameState.RUN)
        {
            RotateOnStart();
        }
    }
    
    private void RotateOnStart()
    {
        _transform.DORotateQuaternion(_desiredRotation, _rotationTime);
        _rotated = true;
    }

    private void FollowPlayer()
    {
        var desiredPosition = _player.position + _desiredOffset;
        var smoothedPosition =
            Vector3.Lerp(_transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        _transform.position = smoothedPosition;
    }
}