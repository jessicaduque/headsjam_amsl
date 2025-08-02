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
    private bool _interactorEnabled;
    private Collider2D _objectCollider;
    // Carrying object control
    private bool _isCarryingObject;
    private HeavyObject _carriedObject;
    
    private void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, OtherPlayerTransform.position);
        
        if (playerDistance < maxRopeLength)
        {
            springJoint.distance = playerDistance;
        }
        else
        {
            springJoint.distance = maxRopeLength;
        }
        
        _objectCollider = _interactorEnabled ? Physics2D.OverlapCircle(interactionPoint.position, interactionPointRadius, interactableObjectsMask, 0) : null;
    }

    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        if (!PlayerMovement.IsGrounded()) return;

        if (!_isCarryingObject) // Code to hold an object if player isn't already carrying one
        {
            if (_objectCollider)
            {
                HeavyObject heavyObjectScript = _objectCollider.gameObject.GetComponent<HeavyObject>();
                if (!heavyObjectScript) return;
                _carriedObject = heavyObjectScript;
                GameObject heavyObject = heavyObjectScript.gameObject;
                heavyObjectScript.Hold(playerHoldPosition.position);
                heavyObject.transform.SetParent(transform);
                AnimationBool("IsHolding", true);
            }
        }
        else // Code to release an object if player is already carrying one
        {
            AnimationBool("IsHolding", true);
            _carriedObject.Drop();
            _carriedObject = null;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
