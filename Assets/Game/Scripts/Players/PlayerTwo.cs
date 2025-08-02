using System.Collections;
using Game.Scripts.Players;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwo : PlayerBase
{
    [SerializeField] private float rayLength;
    [SerializeField] private ParticleSystem singingParticlesRight;
    [SerializeField] private ParticleSystem singingParticlesLeft;
    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        if (!PlayerMovement.IsGrounded()) return;

        if (Mathf.Approximately(PlayerMovement.transform.localScale.x, -1))
        {
            singingParticlesLeft.Play();
        }
        else
        {
            singingParticlesRight.Play();
        }
        
        OtherPlayerBase.DisableInputs();
        DisableInputs();
        
        AnimationBool("Singchirp", true);
        StartCoroutine(ChirpingCoroutine());
    }

    private IEnumerator ChirpingCoroutine()
    {
        yield return new WaitForSeconds(2);
        singingParticlesRight.Stop();
        yield return new WaitForSeconds(1.5f);
        Vector2 direction = transform.right * Mathf.Sign(transform.localScale.x);

        Vector2 normalizedDirection = direction.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, normalizedDirection, rayLength, LayerMask.GetMask("InteractableObjects"));
        Debug.DrawRay(transform.position, normalizedDirection * rayLength, Color.red, 3f);

        if (hit.collider != null)
        {
            GlassObject glass = hit.collider.GetComponent<GlassObject>();
            if (glass != null)
            {
                Debug.Log("Glass object hit! Breaking...");
                glass.BreakGlassObject();
            }
        }
        singingParticlesRight.gameObject.SetActive(false);
        singingParticlesLeft.gameObject.SetActive(false);
        singingParticlesRight.Stop();
        singingParticlesLeft.Stop();
        singingParticlesRight.gameObject.SetActive(true);
        singingParticlesLeft.gameObject.SetActive(true);
        AnimationBool("Singchirp", false);
        OtherPlayerBase.EnableInputs();
        EnableInputs();
    }
}