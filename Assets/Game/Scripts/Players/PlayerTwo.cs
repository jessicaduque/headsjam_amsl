using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwo : PlayerBase
{
    [SerializeField] private float rayLength;
    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        OtherPlayerBase.DisableInputs();
        DisableInputs();
        
        AnimationBool("Singchirp", true);
        StartCoroutine(ChirpingCoroutine());
    }

    private IEnumerator ChirpingCoroutine()
    {
        yield return new WaitForSeconds(2);
        Vector2 direction = transform.right * Mathf.Sign(transform.localScale.x);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("InteractableObjects"));
        Debug.DrawRay(transform.position, direction * rayLength, Color.red, 1f);

        if (hit.collider != null)
        {
            GlassObject glass = hit.collider.GetComponent<GlassObject>();
            if (glass != null)
            {
                Debug.Log("Glass object hit! Breaking...");
                glass.BreakGlassObject();
            }
        }
        
        AnimationBool("Singchirp", false);
        OtherPlayerBase.EnableInputs();
        EnableInputs();
    }
}