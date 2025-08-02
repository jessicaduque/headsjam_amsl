using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    [SerializeField] private LayerMask oilLayer;
    private Transform _playerTransformHold;
    private float _halfHeight;
    private bool _isBeingHeld;
    
    AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _halfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void Update()
    {
        if (_isBeingHeld)
        {
            transform.position = _playerTransformHold.position + new Vector3(0, _halfHeight, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.IsTouchingLayers(oilLayer))
        {
            _audioManager.PlaySfx("touchoil");
            Destroy(gameObject);
        }
    }

    public void Hold(Transform playerTransform)
    {
        _playerTransformHold = playerTransform;
        _isBeingHeld = true;
        gameObject.layer = LayerMask.NameToLayer("Players");
        _audioManager.PlaySfx("holdheavyobject");
    }

    public void Drop()
    {
        _isBeingHeld = false;
        transform.SetParent(null);
        gameObject.layer = LayerMask.NameToLayer("InteractableObjects");
        _audioManager.PlaySfx("holdheavyobject");
    }
}
