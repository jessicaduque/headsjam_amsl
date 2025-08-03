using System.Collections;
using UnityEngine;

public class Window : GlassObject
{
    [SerializeField] private Sprite firstChange;
    [SerializeField] private SpriteRenderer _spriteRenedrer;
    [SerializeField] private GameObject tutorial2;
    [SerializeField] private GameObject Dialogue2;
    [SerializeField] private GameObject Dialogue3;
    [SerializeField] private PlayerTwo playerTwo;
    [SerializeField] private PlayerOne playerOne;
    
    public override void BreakGlassObject()
    {
        AudioManager.I.PlaySfx("glassbreak");
        _cameraShake.DoCameraShake();
        _spriteRenedrer.sprite = firstChange;
        tutorial2.SetActive(false);
        Dialogue2.SetActive(false);
        StartCoroutine(aa());
        _collider2D.enabled = false;
    }

    private IEnumerator aa()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("AQUI: 2");
        playerTwo.StopSinging();
        playerTwo.DisableInputs();
        playerOne.DisableInputs();
        Dialogue3.SetActive(true);
    }
}
