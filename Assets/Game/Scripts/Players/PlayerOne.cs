using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOne : PlayerBase
{
    [Header("For rope mechanics")]
    [SerializeField] private SpringJoint2D springJoint;
    [SerializeField] private int maxRopeLength = 4;
    
    private Player1 _playerInputs;
    private GameObject _carriedObject;
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
    }

    public override void DoPowerControl(InputAction.CallbackContext context)
    {
        if (!PlayerMovement.IsGrounded()) return;
        
        Debug.Log("Player 1's power not implemented yet!");
        // Não implementado ainda
    }
}
