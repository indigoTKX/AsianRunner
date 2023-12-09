using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GateView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _redMaterial;
        
    [SerializeField] private TextMeshProUGUI _gateText;

    public int GetLevel()
    {
        return _level;
    }
        
    // [Inject] private Player _player;

    private int _level;
        
    public void SetLevel(int newLevel)
    {
        _level = newLevel;
        if (_level <= 0)
        {
            _meshRenderer.material = _redMaterial;
            _gateText.text = $"{_level}";
        }
        else
        {
            _gateText.text = $"+{_level}";
        }
    }
}