using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDamageable : Damageable
{
    [Inject] private GameStateManager _gameStateManager;

    private void Awake()
    {
        _gameStateManager.OnReset += base.Start;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnReset -= base.Start;

    }
}
