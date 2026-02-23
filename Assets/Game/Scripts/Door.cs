using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool normalStateIsOpen;

    private Collider2D _collider;
    private float _height;
    private Vector3 _openedPosition;
    private Vector3 _closedPosition;
    private readonly float _doorSpeed = 5f; 

    private void Awake()
    {
        _height = GetComponent<SpriteRenderer>().bounds.size.y;
        _collider = GetComponent<Collider2D>();
        _closedPosition = transform.position;
        _openedPosition = _closedPosition - new Vector3(0, _height, 0);
    }

    private void Start()
    {
        _collider.enabled = transform.position != _openedPosition;
    }

    public void SetInitialPosition(bool isPressed)
    {
        if(!isPressed && normalStateIsOpen) transform.position = _openedPosition;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Move(bool buttonPressed)
    {
        bool shouldBeOpen = normalStateIsOpen ? !buttonPressed : buttonPressed;
        Vector3 destination = shouldBeOpen ? _openedPosition : _closedPosition;

        while (Vector3.Distance(transform.position, destination) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * _doorSpeed);
            yield return null;
        }

        transform.position = destination; 
        _collider.enabled = transform.position != _openedPosition;
    }

    public void TriggerDoor(bool buttonPressed)
    {
        if (LevelManager.I._levelState == LevelState.END) return;
        StopAllCoroutines();
        StartCoroutine(Move(buttonPressed));
    }
}