using System.Collections;
using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    [SerializeField] private LayerMask oilLayer;
    private float _halfHeight;
    private Collider2D _collider;
    
    AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _halfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouchingLayers(oilLayer))
        {
            _audioManager.PlaySfx("touchoil");
            Destroy(gameObject);
        }
    }

    public void Hold(Vector3 playerHoldPos)
    {
        transform.position = playerHoldPos + new Vector3(0, _halfHeight, 0);
        gameObject.layer = LayerMask.NameToLayer("Players");
        _audioManager.PlaySfx("holdheavyobject");
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractableObjects");
        _audioManager.PlaySfx("holdheavyobject");
    }
}
