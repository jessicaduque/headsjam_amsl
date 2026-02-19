using Game.Scripts.Players;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }
    

    protected virtual void Start()
    {
        OtherPlayerBase = otherPlayer.GetComponent<PlayerBase>();
        OtherPlayerTransform = otherPlayer.GetComponent<Transform>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            AnimationRopeControl(true);
            return;
        }
     
        AnimationRopeControl(false);
        LevelManager.I.timeUpEvent += DisableInputs;
        LevelManager.I.pauseEvent += DisableInputs;
        LevelManager.I.gameOverEvent += DisableInputs;
        LevelManager.I.levelCompleteEvent += DisableInputs;
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
        PlayerMovement.enabled = true;
    }
    
    public void DisableInputs()
    {
        PlayerMovement.enabled = false;
        _playerInput.enabled = false;
        _animator.SetBool("Walking", false);
        _animator.SetInteger("JumpVel", -1);
        _animator.SetBool("IsJumping", false);
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
            LevelManager.I.GameOver();
        }
    }

    protected virtual void Death()
    {
        _animator.SetTrigger("Death");
    }
    
    #endregion
    
    #region GAMBIARRA

    public void LigarCanto()
    {
        _rigidbody.bodyType = RigidbodyType2D.Static;
    }
    
    #endregion
    
}
