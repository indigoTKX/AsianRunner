using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuScreenController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private string _tutorialSaveUid;
    [Space] 
    [SerializeField] private float _fadeOutAnimationTime = 1f;
    
    [Inject] private GameStateManager _gameStateManager;

    private CanvasGroup _canvasGroup;
    private Tweener _fadeOutTweener;
    private bool _isTutorialShown = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _isTutorialShown = PlayerPrefs.GetInt(_tutorialSaveUid) != 0;
        
        _playButton.onClick.AddListener(FirePlay);
        _exitButton.onClick.AddListener(FireExit);
        
        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        if (_fadeOutTweener != null)
        {
            _fadeOutTweener.onComplete -= Hide;
        }
        
        _playButton.onClick.RemoveListener(FirePlay);
        _exitButton.onClick.RemoveListener(FireExit);

        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
    }

    private void FirePlay()
    {
        if (_isTutorialShown)
        {
            _gameStateManager.SetRunState();
        }
        else
        {
            PlayerPrefs.SetInt(_tutorialSaveUid, 1);
            _gameStateManager.SetTutorialState();
        }
    }

    private void FireExit()
    {
        Application.Quit();
    }

    private void HandleOnGameStateChanged(GameState newState)
    {
        if (newState == GameState.MAIN_MENU) return;
        _fadeOutTweener = _canvasGroup.DOFade(0f, _fadeOutAnimationTime);
        _fadeOutTweener.onComplete += Hide;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        if (_fadeOutTweener != null)
        {
            _fadeOutTweener.onComplete -= Hide;
        }
    }
}
