using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOne : PlayerBase
{
    private Player1 _playerInputs;

    protected override void Awake()
    {
        base.Awake();
        
        _playerInputs = new Player1();
        Move = _playerInputs.Player.Move;
        Jump = _playerInputs.Player.Jump;
        Power = _playerInputs.Player.Power;
    }
    
    protected override void DoPowerControl(InputAction.CallbackContext context)
    {
        Debug.Log("Player 1's power not implemented yet!");
        // Não implementado ainda
    }
}
