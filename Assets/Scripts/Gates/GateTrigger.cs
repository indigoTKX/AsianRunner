using System;
using UnityEngine;
using Zenject;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private GateView _gate;
        
    // [Inject] private Player _player;
        
    private void OnTriggerEnter(Collider other)
    {
        var squadController = other.gameObject.GetComponent<SquadController>();
        
        if (squadController)
        {
            squadController.ChangeUnitCount(_gate.GetLevel());
        }
    }
}