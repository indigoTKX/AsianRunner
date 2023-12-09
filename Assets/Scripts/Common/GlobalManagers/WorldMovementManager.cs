using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldMovementManager : MonoBehaviour
{
    [SerializeField] private float _maxForwardSpeed = 30f;

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    [Inject] private GameStateManager _gameStateManager;
    
    private float _currentSpeed;

    private void Start()
    {
        _gameStateManager.OnStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(_gameStateManager.GetCurrentState());
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleGameStateChanged;
    }

    private void StartMoving()
    {
        _currentSpeed = _maxForwardSpeed;
    }

    private void StopMoving()
    {
        _currentSpeed = 0;
    }
    
    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.RUN)
        {
            StartMoving();
        }
        else
        {
            StopMoving();
        }
    }
}
