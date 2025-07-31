using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    // Other player
    [SerializeField] GameObject otherPlayer;
    protected Transform OtherPlayerTransform;
    protected PlayerBase OtherPlayerBase;
    
    // Collider
    private Rigidbody2D _rigidbody;
    
    // Movement
    private readonly float _speed = 50f;
    public bool IsMovingFromInput { get; private set; }
    
    // Jumping
    private bool _isJumping;
    private float _jumpCounter;
    private readonly float _jumpTime = 5f;
    private readonly float _jumpForce = 4f;
    private readonly float _fallForce = 3f;
    private Vector2 _gravity;
    public bool isGrounded { get; private set; } = true;
    
    // Health
    private float _health = 1f;
    public bool _isDead { get; private set; }
    
    // Animation
    private Animator _animator;
    
    // Sprite
    private SpriteRenderer _spriteRenderer;
    
    // Input
    protected InputAction Move, Jump, Power;

    private LevelManager _levelManager => LevelManager.I;
    
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void Start()
    {
        _gravity = new(0f, -Physics2D.gravity.y);
        
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

    protected virtual void Update()
    {
        float x = Move.ReadValue<Vector2>().x;
        IsMovingFromInput = x != 0;

        // Movement(x);
        // if (!isGrounded && OtherPlayerBase.isGrounded)
        // {
        //     SwingMovement(x);
        // }
        // else
        // {
        //     Movement(x);
        // }
        
        if (_rigidbody.linearVelocity.y < 0f)
        {
            _rigidbody.linearVelocity -= _gravity * (_fallForce * Time.deltaTime);
        }
        
        if (_rigidbody.linearVelocity.y > 0f && _isJumping)
        {
            _jumpCounter += Time.deltaTime;
            if (_jumpCounter > _jumpTime) _isJumping = false;

            var t = _jumpCounter / _jumpTime;
            var currentJumpF = _jumpForce;
            
            if (t < 0.5f) currentJumpF = _jumpForce * (1 - t);
            
            _rigidbody.linearVelocity += _gravity * (currentJumpF * Time.deltaTime);
        }
    }
    
    #region Movement
    
    // private void Movement(float speedX)
    // {
    //     MovementAnimationControl(speedX);
    //     BodyRotate(speedX);
    //
    //     Vector2 force = new Vector2(speedX * _speed, 0f);
    //     _rigidbody.AddForce(force, ForceMode2D.Force);
    //     
    //     float maxSpeed = 5f;
    //     if (_rigidbody.linearVelocity.magnitude > maxSpeed)
    //     {
    //         _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * maxSpeed;
    //     }
    // }
    
    private void BodyRotate(float speedX)
    {
        if (speedX > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (speedX < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);   
        }
    }
    
    private void SwingMovement(float speedX)
    {
        Vector2 ropeDirection = (OtherPlayerTransform.position - transform.position).normalized;
        Vector2 perpendicular = Vector2.Perpendicular(ropeDirection); 
        float input = speedX;

        if (Vector2.Dot(perpendicular, Vector2.up) < 0)
            perpendicular = -perpendicular;

        _rigidbody.AddForce(perpendicular * (input * 10f), ForceMode2D.Force); 
    }
    
    public IEnumerator GoTo(Vector3 position)
    {
        float speed = 1;
        if(position.x < transform.position.x) speed = -1f;
        
        while (!Mathf.Approximately(transform.position.x, position.x))
        {
            // Movement(speed);
            yield return null;
        }
        _rigidbody.linearVelocity = Vector2.zero;
    }
    
    public IEnumerator GoToEndLevelObject(Vector3 position, EndLevelObject endObject, float fadeDuration)
    {
        float speed = 1;
        if(position.x < transform.position.x) speed = -1f;

        while (Mathf.Abs(transform.position.x - position.x) > 0.2f) 
        {
            // Movement(speed);
            yield return null;
        }
        FreezePlayer();
        _spriteRenderer.DOFade(0, fadeDuration).OnComplete(endObject.PlayerEntered);
    }

    public void FreezePlayer()
    {
        DisableInputs();
        _rigidbody.linearVelocity = Vector2.zero;
    }
    
    #endregion
    
    #region Animation
    
    private void MovementAnimationControl(float speedX)
    {
        //_animator.SetBool("Walking", speedX != 0);
    }
    
    protected void AnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
    
    protected void AnimationBool(string boolName, bool enable)
    {
        _animator.SetBool(boolName, enable);
    }
    
    #endregion
    
    #region Input
    
    public void EnableInputs()
    {
        Power.started += DoPowerControl;
        // Jump.started += DoJumpStarted;
        // Jump.canceled += DoJumpPerformed;
        // Jump.performed += DoJumpPerformed;
    
        Move.Enable();
        Power.Enable();
        Jump.Enable();
    }
    
    public void DisableInputs()
    {
        Power.started -= DoPowerControl;
        // Jump.started -= DoJumpStarted;
        // Jump.canceled -= DoJumpCanceled;
        // Jump.performed -= DoJumpPerformed;
    
        Move.Disable();
        Power.Disable();
        Jump.Disable();
    }
    
    #endregion
    
    #region Jump Control

    private void DoJumpStarted(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _speed);
            _isJumping = true;
            _jumpCounter = 0f;
        }
    }

    private void DoJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            _isJumping = false;
            
            if (_rigidbody.linearVelocity.y > 0f)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.8f);
            }
        }
    }

    private void DoJumpCanceled(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            _isJumping = true;
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 1.25f);
        }
        
        if (context.interaction is HoldInteraction)
        {
            Debug.Log("Hold Canceled");
            _isJumping = false;
            
            if (_rigidbody.linearVelocity.y > 0f)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.8f);
            }
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
            _levelManager.GameOver();
        }
    }
    
    #endregion
    
    #region SET

    public void SetIsGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
    
    #endregion
}
