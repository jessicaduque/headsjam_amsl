using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    // Other player
    [SerializeField] private Transform otherPlayerTransform;
    private PlayerBase _otherPlayerBase;
    
    // Collider
    private Rigidbody2D _rigidbody;
    
    // Movement
    private readonly float _speed = 50f;
    public bool IsMovingFromInput { get; private set; }
    
    // Jumping
    private bool _isJumping;
    private float _jumpCounter;
    private readonly float _jumpTime = 0.3f;
    private readonly float _jumpForce = 0.5f;
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
    
    private void Start()
    {
        _otherPlayerBase = otherPlayerTransform.GetComponent<PlayerBase>();
        
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

        if (!isGrounded && _otherPlayerBase.isGrounded)
        {
            SwingMovement(x);
        }
        else
        {
            Movement(x);
        }
    }
    
    
    
    #region Movement
    
    private void Movement(float speedX)
    {
        MovementAnimationControl(speedX);
        BodyRotate(speedX);

        Vector2 force = new Vector2(speedX * _speed, 0f);
        _rigidbody.AddForce(force, ForceMode2D.Force);
        
        float maxSpeed = 5f;
        if (_rigidbody.linearVelocity.magnitude > maxSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
    
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
        Vector2 ropeDirection = (otherPlayerTransform.position - transform.position).normalized;
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
            Movement(speed);
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
            Movement(speed);
            yield return null;
        }
        _rigidbody.linearVelocity = Vector2.zero;
        _spriteRenderer.DOFade(0, fadeDuration).OnComplete(endObject.PlayerEntered);
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
