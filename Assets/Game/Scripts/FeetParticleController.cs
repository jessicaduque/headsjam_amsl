using System;
using UnityEngine;

public class FeetParticleController : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D playerRb;

    private ParticleSystem _movementParticles;
    private float _counter;

    private void Start()
    {
        _movementParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        _counter += Time.deltaTime;

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