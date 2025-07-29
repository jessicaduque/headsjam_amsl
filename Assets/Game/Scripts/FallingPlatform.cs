using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class FallingPlatform : MonoBehaviour
    {
        [SerializeField] private float fallWait = 2f;
        [SerializeField] private float respawnWait = 4f;
        [SerializeField] private float appearWait = 1;

        private Vector3 _initialPosition;
        private bool _isFalling;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _initialPosition = transform.position;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isFalling && other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FallCoroutine());
            }
        }

        private IEnumerator FallCoroutine()
        {
            _isFalling = true;
            yield return new WaitForSeconds(fallWait);
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _spriteRenderer.DOFade(0, fallWait).OnComplete(() =>
            {
                _collider.enabled = false;
                StartCoroutine(Respawn());
            });
        }

        private IEnumerator Respawn()
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            yield return new WaitForSeconds(respawnWait);
            transform.position = _initialPosition;
            _spriteRenderer.DOFade(1, appearWait).OnComplete(() =>
            {
                _collider.enabled = true;
            });
        }
    }
}
