using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOne : PlayerBase
{
    [Header("For rope mechanics")]
    [SerializeField] private SpringJoint2D springJoint;
    [SerializeField] private int maxRopeLength = 4;
    
    private Player1 _playerInputs;
    
    [Header("Interaction with heavy objects")] 
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 2f;
    [SerializeField] private LayerMask interactableObjectsMask; 
    [SerializeField] private Transform playerHoldPosition;
    private Collider2D _objectCollider;
    // Carrying object control
    private bool _isCarryingObject;
    private bool _canCarryObject = true;
    private HeavyObject _carriedObject;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, OtherPlayerTransform.position);

        _objectCollider = Physics2D.OverlapCircle(interactionPoint.position, interactionPointRadius, interactableObjectsMask, 0);
        
        if (!springJoint) return;
        if (playerDistance < maxRopeLength)
        {
            springJoint.distance = playerDistance;
        }
        else
        {
            springJoint.distance = maxRopeLength;
        }
    }

    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        if (!_canCarryObject) return;
        if (!context.started) return;
        if (!PlayerMovement.IsGrounded()) return;
        
        if (!_isCarryingObject) // Code to hold an object if player isn't already carrying one
        {
            if (_objectCollider)
            {
                HeavyObject heavyObjectScript = _objectCollider.gameObject.GetComponent<HeavyObject>();
                if (!heavyObjectScript) return;
                _carriedObject = heavyObjectScript;
                GameObject heavyObject = heavyObjectScript.gameObject;
                heavyObjectScript.Hold(playerHoldPosition);
                heavyObject.transform.SetParent(playerHoldPosition);
                AnimationBool("IsHolding", true);
                _isCarryingObject = true;
            }
        }
        else // Code to release an object if player is already carrying one
        {
            _canCarryObject = false;
            AnimationBool("IsHolding", false);
            _carriedObject.Drop();
            _carriedObject = null;
            StartCoroutine(PowerCooldownCoroutine());
            _isCarryingObject = false;
        }
    }

    private IEnumerator PowerCooldownCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        _canCarryObject = true;
    } 
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
