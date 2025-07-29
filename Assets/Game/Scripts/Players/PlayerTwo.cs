using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwo : PlayerBase
{
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
        Debug.Log("Player 2's power not implemented yet!");
        // Não implementado ainda
    }
}