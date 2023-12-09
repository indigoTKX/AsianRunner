using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private float _destroyPosition = -12f;

    private void Update()
    {
        if (transform.position.z > _destroyPosition) return;
        DestroyBehindPlayer();
    }

    protected virtual void DestroyBehindPlayer()
    {
        Destroy(gameObject);
    }
}