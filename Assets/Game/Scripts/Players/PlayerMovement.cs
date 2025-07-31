using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;

        [Header("Movement")]
        public float moveSpeed = 10f;
        private float _horizontalMovement;

        [Header("Jumping")]
        public float jumpPower = 10f;
        public float _jumpTime = 0.4f;
        private float _jumpCounter;
        public float _jumpMultiplier = 3f;
        private bool _isJumping;

        [Header("Ground Check")] 
        public Transform groundCheck;
        public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
        public LayerMask groundLayer;

        [Header("Gravity")]
        public float fallMultiplier = 2f;
        private Vector2 _vecGravity;

        private void Start()
        {
            _vecGravity = new Vector2(0, -Physics2D.gravity.y);
        }
        
        private void Update()
        {
            rb.linearVelocity = new Vector2(_horizontalMovement * moveSpeed, rb.linearVelocity.y);
            
            Gravity();
            HigherJumping();
        }
    
        public void Move(InputAction.CallbackContext context) 
        {
            _horizontalMovement = context.ReadValue<Vector2>().x;
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started && IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                _isJumping = true;
                _jumpCounter = 0f;
            }
            else if (context.canceled)
            {
                _isJumping = false;
                _jumpCounter = 0f;

                if (rb.linearVelocity.y > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                }
            }
        }

        private void Gravity()
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity -= _vecGravity * (fallMultiplier * Time.deltaTime);
            }
        }

        private void HigherJumping()
        {
            if (rb.linearVelocity.y > 0 && _isJumping)
            {
                _jumpCounter += Time.deltaTime;
                if (_jumpCounter > _jumpTime) _isJumping = false;

                var t = _jumpCounter / _jumpTime;
                var currentJumpF = _jumpMultiplier;
                
                if (t < 0.5f) currentJumpF = _jumpMultiplier * (1 - t);
                
                rb.linearVelocity += _vecGravity * (currentJumpF * Time.deltaTime);
            }
        }

        private bool IsGrounded()
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
