using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

namespace Game.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        [Header("Movement")]
        public float moveSpeed = 10f;
        private float _horizontalMovement;
        public bool IsMovingFromInput { get; private set; }
        private PlayerBase _playerBase;
        
        // Sprite
        private SpriteRenderer _spriteRenderer;
        
        // Animation
        private Animator _animator;
        
        [FormerlySerializedAs("_jumpMultiplier")]
        [Header("Jumping")]
        [SerializeField] float jumpMultiplier = 3f;
        private readonly float _jumpPower = 8f;
        private readonly float _jumpTime = 0.4f;
        private float _jumpCounter;
        private bool _isJumping;

        [Header("Ground Check")] 
        public Transform groundCheck;
        public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
        public LayerMask groundLayer;
        [SerializeField] private PlayerMovement otherPlayerMovement;
        private Transform OtherPlayerTransform;
        
        [Header("Gravity")]
        [SerializeField] float fallMultiplier = 3f;
        private Vector2 _gravity;

        private void Awake()
        {
            _gravity = new Vector2(0, -Physics2D.gravity.y);
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerBase = GetComponent<PlayerBase>();
        }

        private void Start()
        {
            OtherPlayerTransform = otherPlayerMovement.GetComponent<Transform>();
        }

        private void Update()
        {
            IsMovingFromInput = _horizontalMovement != 0;
            
            DoPlayerMovement(_horizontalMovement);
            // if (!IsGrounded() && otherPlayerMovement.IsGrounded() && OtherPlayerTransform.position.y - 2 > transform.position.y)
            // {
            //     SwingMovement(_horizontalMovement);
            // }
            // else
            // {
            //     DoPlayerMovement(_horizontalMovement);
            // } 
            
            Gravity();
            BodyRotate(_horizontalMovement);
            MovementAnimationControl(_horizontalMovement);
        }

        public void Move(InputAction.CallbackContext context) 
        {
            _horizontalMovement = context.ReadValue<Vector2>().x;
        }

        private void DoPlayerMovement(float speedX)
        {
            _rigidbody.linearVelocity = new Vector2(speedX * moveSpeed, _rigidbody.linearVelocity.y);
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

        private void MovementAnimationControl(float speedX)
        {
            _animator.SetBool("Walking", speedX != 0);
        }
        
        #region Jump
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started && IsGrounded())
            {
                _animator.SetBool("IsJumping", true);
                _animator.SetInteger("JumpVel", 1);
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpPower);
                _isJumping = true;
                _jumpCounter = 0f;
            }
            else if (context.canceled)
            {
                _isJumping = false;
                _jumpCounter = 0f;

                if (_rigidbody.linearVelocity.y > 0)
                {
                    _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.5f);
                }
            }
        }

        private void Gravity()
        {
            if (_rigidbody.linearVelocity.y < 0f && !IsGrounded())
            {
                _rigidbody.linearVelocity -= _gravity * (fallMultiplier * Time.deltaTime);
                _animator.SetInteger("JumpVel", -1);
            }
            
            
            if (_rigidbody.linearVelocity.y > 0f && _isJumping)
            {
                _jumpCounter += Time.deltaTime;
                if (_jumpCounter > _jumpTime) _isJumping = false;

                var t = _jumpCounter / _jumpTime;
                var currentJumpF = jumpMultiplier;
                
                if (t < 0.5f) currentJumpF = jumpMultiplier * (1 - t);
                
                _rigidbody.linearVelocity += _gravity * (currentJumpF * Time.deltaTime);
            }
            else if (IsGrounded())
            {
                _animator.SetBool("IsJumping", false);
            }
        }

        #endregion
        
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
                DoPlayerMovement(speed);
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
                DoPlayerMovement(speed);
                BodyRotate(_horizontalMovement);
                MovementAnimationControl(_horizontalMovement);
                yield return null;
            }
            _playerBase.FreezePlayer();
            _spriteRenderer.DOFade(0, fadeDuration).OnComplete(endObject.PlayerEntered);
        }
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
