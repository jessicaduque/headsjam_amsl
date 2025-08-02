using System;
using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    [SerializeField] private LayerMask oilLayer;
    private float _halfHeight;

    private void Awake()
    {
        _halfHeight = GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouchingLayers(oilLayer))
        {
            AudioManager.I.PlaySfx("touchoil");
            Destroy(gameObject);
        }
    }

    public void Hold(Vector3 playerHoldPos)
    {
        transform.position = playerHoldPos + new Vector3(0, _halfHeight, 0);
    }
}
