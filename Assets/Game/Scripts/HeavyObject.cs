using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    [SerializeField] private LayerMask oilLayer;
    private Rigidbody2D _rigidbody;
    private float _halfHeight;
    private Collider2D _collider;
    
    AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _halfHeight = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        _rigidbody = GetComponent<Rigidbody2D>();
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
        _rigidbody.gravityScale = 0;
        transform.localPosition = playerHoldPos + new Vector3(0, _halfHeight, 0);
        gameObject.layer = LayerMask.NameToLayer("Players");
        _audioManager.PlaySfx("holdheavyobject");
        _collider.enabled = false;
    }

    public void Drop()
    {
        _rigidbody.gravityScale = 1;
        gameObject.layer = LayerMask.NameToLayer("InteractableObjects");
        _audioManager.PlaySfx("holdheavyobject");
        _collider.enabled = true;
    }
}
