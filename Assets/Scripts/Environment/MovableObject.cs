using System;
using UnityEngine;
using Zenject;

public class MovableObject : MonoBehaviour
{

    [Inject] private WorldMovementManager _movementManager;
    [Inject] private GameStateManager _gameStateManager;

    private float _currentForwardSpeed = 0f;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();

        _gameStateManager.OnReset += Destroy;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnReset -= Destroy;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var desiredPosition = _transform.position;
        _currentForwardSpeed = _movementManager.GetCurrentSpeed();
        desiredPosition.z -= _currentForwardSpeed * Time.fixedDeltaTime;
        _transform.position = desiredPosition;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}