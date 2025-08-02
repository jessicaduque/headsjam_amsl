using Game.Scripts.Players;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    // Other player
    [SerializeField] GameObject otherPlayer;
    protected Transform OtherPlayerTransform;
    protected PlayerBase OtherPlayerBase;
    
    // Animation
    private Animator _animator;
    
    // Collider
    private Rigidbody2D _rigidbody;

    // Movement
    protected PlayerMovement PlayerMovement;
    
    // Health
    private float _health = 1f;
    public bool _isDead { get; private set; }
    
    // Input
    private PlayerInput _playerInput;

    protected LevelManager _levelManager => LevelManager.I;
    
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }
    
    protected virtual void Start()
    {
        // Set correct animation layer
        AnimationRopeControl(_levelManager.IsLevelTutorial());
        
        OtherPlayerBase = otherPlayer.GetComponent<PlayerBase>();
        OtherPlayerTransform = otherPlayer.GetComponent<Transform>();
        
        Debug.Log("Remember that for now player inputs are being automatically turned on at the start!");
        EnableInputs();
        
        _levelManager.startLevelEvent += EnableInputs;
        
        _levelManager.timeUpEvent += DisableInputs;
        _levelManager.pauseEvent += DisableInputs;
        _levelManager.gameOverEvent += DisableInputs;
        _levelManager.levelCompleteEvent += EnableInputs;
    }
    
    #region Animation
    
    protected void AnimationBool(string boolName, bool enable)
    {
        _animator.SetBool(boolName, enable);
    }
    
    private void AnimationRopeControl(bool state)
    {
        _animator.SetLayerWeight(1, (state ? 1 : 0));
    }
    
    #endregion
    
    #region Input
    
    public void FreezePlayer()
    {
        DisableInputs();
        _rigidbody.linearVelocity = Vector2.zero;
    }
    
    public void EnableInputs()
    {
        _playerInput.enabled = true;
    }
    
    public void DisableInputs()
    {
        _playerInput.enabled = false;
    }
    
    #endregion
    
    #region Power
    public abstract void DoPowerControl(InputAction.CallbackContext context);
    
    #endregion

    #region Health
    
    public void ModifyHealth(int value)
    {
        if (_isDead)
        {
            return;
        }
    
        _health += value;
    
        if (_health <= 0)
        {
            _isDead = true;
            Death();
            _levelManager.GameOver();
        }
    }

    protected virtual void Death()
    {
        _animator.SetTrigger("Death");
    }
    
    #endregion
    
}
