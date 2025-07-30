using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwo : PlayerBase
{
    [SerializeField] private float rayLength;
    private Player2 _playerInputs;

    protected override void Awake()
    {
        base.Awake();
        
        _playerInputs = new Player2();
        Move = _playerInputs.Player.Move;
        Jump = _playerInputs.Player.Jump;
        Power = _playerInputs.Player.Power;
    }
    
    protected override void DoPowerControl(InputAction.CallbackContext context)
    {
        OtherPlayerBase.DisableInputs();
        DisableInputs();
        
        AnimationTrigger("singchirp");
    }

    public void ChirpingEndAnimatorCallback()
    {
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
        
        OtherPlayerBase.EnableInputs();
        EnableInputs();
    }
}