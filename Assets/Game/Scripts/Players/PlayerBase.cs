using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    // Collider
    private Rigidbody2D _rigidbody;
    
    // Movement
    private readonly float _speed = 50f;
    private Rigidbody2D _rigidbody2D;
    
    // Jumping
    private bool _isJumping;
    private float _jumpCounter;
    private readonly float _jumpTime = 0.3f;
    private readonly float _jumpForce = 0.5f;
    [HideInInspector] public bool isGrounded = true;
    
    // Health
    private float _health = 10f;
    public bool _isDead { get; private set; }
    
    // Animation
    private Animator _animator;
    
    // Input
    protected InputAction Move, Jump, Power;

    //private LevelController _levelController => LevelController.I;
    
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        Debug.Log("Remember that for now player inputs are being automatically turned on at the start!");
        EnableInputs();
        _rigidbody = GetComponent<Rigidbody2D>();
        // _levelController.beginLevelEvent += EnableInputs;
        //
        // _levelController.timeUpEvent += DisableInputs;
        // _levelController.pauseEvent += DisableInputs;
    }

    private void FixedUpdate()
    {
        Movement();
    }
    
    #region Movement
    
    private void Movement()
    {
        float speedX = Move.ReadValue<Vector2>().x;
        MovementAnimationControl(speedX);
        BodyRotate(speedX);

        // Apply force for movement
        Vector2 force = new Vector2(speedX * _speed, 0f);
        _rigidbody2D.AddForce(force, ForceMode2D.Force);
        
        float maxSpeed = 5f;
        if (_rigidbody2D.linearVelocity.magnitude > maxSpeed)
        {
            _rigidbody2D.linearVelocity = _rigidbody2D.linearVelocity.normalized * maxSpeed;
        }
    }
    
    private void BodyRotate(float speedX)
    {
        if (speedX > 0f)
            transform.localScale = new Vector3(1, 1, 1);
        else if(speedX < 0f)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    
    #endregion
    
    #region Animation
    
    private void MovementAnimationControl(float speedX)
    {
        //_animator.SetBool("Walking", speedX != 0);
    }
    
    private void AnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
    
    #endregion
    
    #region Input
    
    public void EnableInputs()
    {
        Power.started += DoPowerControl;
        Jump.started += DoJumpStarted;
        Jump.performed += DoJumpPerformed;
    
        Move.Enable();
        Power.Enable();
        Jump.Enable();
    }
    
    public void DisableInputs()
    {
        Power.started -= DoPowerControl;
        Jump.started -= DoJumpStarted;
        Jump.performed -= DoJumpPerformed;
    
        Move.Disable();
        Power.Disable();
        Jump.Disable();
    }
    
    #endregion
    
    #region Jump Control

    private void DoJumpStarted(InputAction.CallbackContext context)
    {
        // Pulo
        if (isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _speed);
        }

        // Isso não foi implementado ainda (isso vai pro Update)
        // if (_rigidbody.linearVelocity.y > 0f && _isJumping)
        // {
        //     _rigidbody.linearVelocity += ;
        // }
    }

    private void DoJumpPerformed(InputAction.CallbackContext context)
    {
        _isJumping = false;
        _jumpCounter = 0f;

        if (_rigidbody.linearVelocity.y > 0f)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.6f);
        }
    }
    
    #endregion
    
    #region Power
    protected abstract void DoPowerControl(InputAction.CallbackContext context);
    
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
            Debug.Log("Something to happen when player dies not implemented yet!");
            // Nada implementado ainda
        }
    }
    
    #endregion
}
