using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;

public class SquadController : MonoBehaviour
{
    public event Action<int> OnCountChanged;
    
    [SerializeField] private Transform _unitPrefab;
    [SerializeField] private Transform _initialUnit;

    [SerializeField] private float radius = 1f;
    [SerializeField] private float _regroupAnimationTime = 1f;

    [SerializeField] private GameObject _spawnSfxPrefab;
    [SerializeField] private float _spawnSfxLifetime = 2f;
    [SerializeField] private string _spawnSfxSoundUid;

    public Transform GetAliveMember()
    {
        return _squadMembers.IsEmpty() ? null : _squadMembers.First();
    }
    
    public void ChangeUnitCount(int countDiff)
    {
        StopRegroupAnimation();
        
        var targetCount = _squadMembers.Count + countDiff;
        if (targetCount <= 0)
        {
            targetCount = 1;
        }

        while (_squadMembers.Count != targetCount)
        {
            if (countDiff > 0)
            {
                var unit = _diContainer.InstantiatePrefab(_unitPrefab, transform);
                _squadMembers.Add(unit.transform);
            }
            else
            {
                var unit = _squadMembers.First();
                _squadMembers.Remove(unit);
                Destroy(unit.gameObject);
            }
        }

        var sfx = _diContainer.InstantiatePrefab(_spawnSfxPrefab, transform.position, transform.rotation, null);
        Destroy(sfx, _spawnSfxLifetime);
        _audioManager.PlayAudio(_spawnSfxSoundUid);
        
        OnCountChanged?.Invoke(targetCount);

        AdjustUnitsPositions();
    }

    public int GetSquadCount()
    {
        return _squadMembers.Count;
    }

    [Inject] private GameStateManager _gameStateManager;
    [Inject] private AudioManager _audioManager;
    
    private List<Transform> _squadMembers = new List<Transform>();
    private DiContainer _diContainer;
    
    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Awake()
    {
        _squadMembers.Add(_initialUnit);

        _gameStateManager.OnReset += ResetSquad;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnReset -= ResetSquad;
        StopRegroupAnimation();
    }

    private void StopRegroupAnimation()
    {
        foreach (var unit in _squadMembers)
        {
            unit.DOKill();
        }
    }
    
    private void AdjustUnitsPositions(bool withAnimation = true)
    {
        var spawnPoint = Vector3.zero + radius * transform.forward;
        if (_squadMembers.Count == 2)
        {
            spawnPoint = Vector3.zero + radius * transform.right;
        }
        else if (_squadMembers.Count == 1)
        {
            spawnPoint = Vector3.zero;
        }
        
        var rotationStep = 360 / _squadMembers.Count;
        foreach (var unit in _squadMembers)
        {
            if (withAnimation)
            {
                unit.DOLocalMove(spawnPoint, _regroupAnimationTime);
            }
            else
            {
                unit.transform.localPosition = spawnPoint;
            }
            spawnPoint = Quaternion.Euler(0, rotationStep, 0) * spawnPoint;
        }
    }

    private void ResetSquad()
    {
        while (_squadMembers.Count != 1)
        {
            var unit = _squadMembers.First();
            _squadMembers.Remove(unit);
            Destroy(unit.gameObject);
        }

        AdjustUnitsPositions(false);
    }
}
