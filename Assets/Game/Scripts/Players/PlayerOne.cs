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
        
        _objectCollider = Physics2D.OverlapCircle(interactionPoint.position, interactionPointRadius, interactableObjectsMask, 0);
    }

    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!PlayerMovement.IsGrounded()) return;
        
        if (!_isCarryingObject) // Code to hold an object if player isn't already carrying one
        {
            if (_objectCollider)
            {
                Debug.Log("here 4");
                HeavyObject heavyObjectScript = _objectCollider.gameObject.GetComponent<HeavyObject>();
                if (!heavyObjectScript) return;
                Debug.Log("here 5");
                _carriedObject = heavyObjectScript;
                GameObject heavyObject = heavyObjectScript.gameObject;
                heavyObjectScript.Hold(playerHoldPosition.position);
                heavyObject.transform.SetParent(transform);
                AnimationBool("IsHolding", true);
                _isCarryingObject = true;
            }
        }
        else // Code to release an object if player is already carrying one
        {
            Debug.Log("here 3");
            AnimationBool("IsHolding", false);
            _carriedObject.Drop();
            _carriedObject = null;
            _isCarryingObject = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
