using System;
using UnityEngine;

public class DestroyableEnvironment : DestroyableObject
{
    private Action<DestroyableEnvironment> _respawnAction = null;

    public void Initialize(Action<DestroyableEnvironment> onDestroyCb)
    {
        _respawnAction = onDestroyCb;
    }

    protected override void DestroyBehindPlayer()
    {
        if (_respawnAction == null)
        {
            Destroy(gameObject);
        }
        else
        {
            _respawnAction(this);
        }
    }
}