using UnityEngine;

public class HeavyObject : MonoBehaviour
{
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
