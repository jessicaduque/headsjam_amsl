using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool normalStateIsOpen;

    private float _height;
    private Vector3 _openedPosition;
    private Vector3 _closedPosition;
    private readonly float _doorSpeed = 1f; 

    private void Awake()
    {
        _height = GetComponent<SpriteRenderer>().bounds.size.y;
        _closedPosition = transform.position;
        _openedPosition = _closedPosition + new Vector3(0, _height, 0);
    }

    private void Start()
    {
        if (normalStateIsOpen)
        {
            transform.position = _openedPosition;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Move(bool buttonPressed)
    {
        bool shouldBeOpen = normalStateIsOpen ? !buttonPressed : buttonPressed;
        Vector3 destination = shouldBeOpen ? _openedPosition : _closedPosition;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * _doorSpeed);
            yield return null;
        }

        transform.position = destination; 
    }

    public void TriggerDoor(bool buttonPressed)
    {
        StopAllCoroutines();
        StartCoroutine(Move(buttonPressed));
    }
}