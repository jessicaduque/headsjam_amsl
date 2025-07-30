using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerOne : PlayerBase
{
    [Header("For rope mechanics")]
    [SerializeField] private SpringJoint2D springJoint;
    [SerializeField] private Transform playerTwoTransform;
    [SerializeField] private Rigidbody2D playerTwoRigidbody;
    [SerializeField] private int maxRopeLength = 4;
    private Player1 _playerInputs;
    private GameObject _carriedObject;
    
    protected override void Awake()
    {
        base.Awake();
        
        _playerInputs = new Player1();
        Move = _playerInputs.Player.Move;
        Jump = _playerInputs.Player.Jump;
        Power = _playerInputs.Player.Power;
    }

    private void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, playerTwoTransform.position);
        
        if (playerDistance < maxRopeLength)
        {
            springJoint.distance = playerDistance;
        }
        else
        {
            springJoint.distance = maxRopeLength;
        }
    }

    protected override void DoPowerControl(InputAction.CallbackContext context)
    {
        Debug.Log("Player 1's power not implemented yet!");
        // Não implementado ainda
    }
}
