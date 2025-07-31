using Game.Scripts.Players;
using UnityEngine;

public class FeetParticleController : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] PlayerMovement playerMovement;
    
    private ParticleSystem _movementParticles;
    private float _counter;

    private void Start()
    {
        _movementParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        if (!playerMovement.IsMovingFromInput || !playerMovement.IsGrounded())
        {
            _movementParticles.Stop();
            return;
        };
        
        if(Mathf.Abs(playerRb.linearVelocity.x) > occurAfterVelocity)
        {
            if(_counter > dustFormationPeriod)
            {
                _movementParticles.Play();
                _counter = 0;
            }
        }
        else
        {
            _movementParticles.Stop();
        }
    }
}