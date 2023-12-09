using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 1.5f;

    [Inject] private InputManager _inputManager;
    [Inject] private GameStateManager _gameStateManager;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _gameStateManager.OnStateChanged += HandleOnStateChanged;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleOnStateChanged;
        _inputManager.OnAnyTap -= ResetGame;
    }

    private void HandleOnStateChanged(GameState newState)
    {
        if (newState != GameState.GAME_OVER)
        {
            gameObject.SetActive(false);
            _inputManager.OnAnyTap -= ResetGame;
            return;
        }

        // Debug.Log("GAME OVER!");
        _rectTransform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        _inputManager.OnAnyTap += ResetGame;
        _rectTransform.DOScale(Vector3.one, _animationDuration);
    }

    private void ResetGame()
    {
        _gameStateManager.ResetGame();
    }
}
