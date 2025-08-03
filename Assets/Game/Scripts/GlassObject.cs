using DG.Tweening;
using UnityEngine;

public class GlassObject : MonoBehaviour
{
    private string _sfxName;
    protected Collider2D _collider2D;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;

    protected CameraShake _cameraShake => CameraShake.I;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void BreakGlassObject()
    {
        _collider2D.enabled = false;
        AudioManager.I.PlaySfx(_sfxName);
        _animator.SetTrigger("glassbreak");
        _cameraShake.DoCameraShake();
    }
    
    public virtual void DestroyObject() // Called by event on animator
    {
        _spriteRenderer.DOFade(0, 0.5f).OnComplete(() => Destroy(this));
    }
}
