using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerUiController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TMP_Text _counterLabel;
    [SerializeField] private Slider _healthBar;

    [Inject] private GameStateManager _gameStateManager;
    
    private SquadController _playerSquadController;
    private Damageable _playerDamageable;

    private void Awake()
    {
        _playerSquadController = GetComponent<SquadController>();
        _playerSquadController.OnCountChanged += UpdateCounter;

        _playerDamageable = GetComponent<Damageable>();
        _playerDamageable.OnDamaged += UpdateHealthBar;

        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        _playerSquadController.OnCountChanged -= UpdateCounter;
        _playerDamageable.OnDamaged -= UpdateHealthBar;
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
    }

    private void Start()
    {
        UpdateCounter(_playerSquadController.GetSquadCount());

    }

    private void UpdateCounter(int squadCount)
    {
        _counterLabel.text = squadCount.ToString();
    }

    private void UpdateHealthBar(float damage)
    {
        _healthBar.value = _playerDamageable.CurrentHealth / _playerDamageable.GetMaxHealth();
        // var healthBarLocalScale = _healthBar.localScale;
        // healthBarLocalScale.x = _playerDamageable.CurrentHealth / _playerDamageable.GetMaxHealth();
        // _healthBar.localScale = healthBarLocalScale;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        if (state is GameState.RUN)
        {
            Show();
            UpdateCounter(_playerSquadController.GetSquadCount());
            UpdateHealthBar(_playerDamageable.CurrentHealth);
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _canvas.SetActive(true);
    }
    
    private void Hide()
    {
        _canvas.SetActive(false);

    }
}
