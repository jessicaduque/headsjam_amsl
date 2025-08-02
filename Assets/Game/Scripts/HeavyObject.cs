using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    [SerializeField] private LayerMask oilLayer;
    private Rigidbody2D _rigidbody;
    private float _halfHeight;
    
    AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _halfHeight = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        _rigidbody = GetComponent<Rigidbody2D>();
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
        transform.position = playerHoldPos + new Vector3(0, _halfHeight, 0);
        gameObject.layer = LayerMask.NameToLayer("Player");
        _audioManager.PlaySfx("holdheavyobject");
    }

    public void Drop()
    {
        _rigidbody.gravityScale = 0;
        gameObject.layer = LayerMask.NameToLayer("InteractableObjects");
        _audioManager.PlaySfx("holdheavyobject");
    }
}
