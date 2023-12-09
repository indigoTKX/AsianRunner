using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TutorialScreenController : MonoBehaviour
{
    [SerializeField] private float _fadeAnimationDuration = 1f;
    [Space]
    [SerializeField] private TMP_Text _tutorialSpeechLabel;
    [SerializeField] private List<string> _tutorialPhrases;
    [Space]
    [SerializeField] private Button _nextButton;

    [Inject] private GameStateManager _gameStateManager;
    private int _currentPhraseIndex = 0;

    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        _tutorialSpeechLabel.text = _tutorialPhrases[_currentPhraseIndex];
        
        _nextButton.onClick.AddListener(SetNextTutorialPhase);
        
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        _nextButton.onClick.RemoveListener(SetNextTutorialPhase);
        
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
    }

    private void SetNextTutorialPhase()
    {
        _currentPhraseIndex++;
        if (_currentPhraseIndex >= _tutorialPhrases.Count)
        {
            _gameStateManager.SetRunState(false);
        }
        else
        {
            _tutorialSpeechLabel.text = _tutorialPhrases[_currentPhraseIndex];
        }
    }
    
    private void HandleOnGameStateChanged(GameState newState)
    {
        if (newState != GameState.TUTORIAL)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, _fadeAnimationDuration);
    }
}
