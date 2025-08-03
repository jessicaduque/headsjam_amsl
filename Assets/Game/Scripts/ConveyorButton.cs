using UnityEngine;

public class ConveyorButton : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private bool startsPressed;
    [SerializeField] private Collider2D solidCollider;
    private int _amountObjectsPressing;
    private SpriteRenderer _spriteRenderer;

    public static event System. OnButtonPressed;
    public static event System.Action OnButtonUnpressed;

    private LevelManager _levelManager => LevelManager.I;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = startsPressed ? buttonSprites[1] : buttonSprites[0];
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_levelManager._levelState != LevelState.PLAYING) return;
        if (other.CompareTag("HeavyObject") || other.CompareTag("Player"))
        {
            _amountObjectsPressing++;
            _spriteRenderer.sprite = buttonSprites[1];
            Destroy(solidCollider);
            solidCollider = gameObject.AddComponent<PolygonCollider2D>();
            // Function to stop conveyor (including sound!)
            OnButtonPressed?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_levelManager._levelState != LevelState.PLAYING) return;
        if (other.CompareTag("HeavyObject")|| other.CompareTag("Player"))
        {
            _amountObjectsPressing--;
            if (_amountObjectsPressing == 0)
            {
                _spriteRenderer.sprite = buttonSprites[0];
                Destroy(solidCollider);
                solidCollider = gameObject.AddComponent<PolygonCollider2D>();
                // Function to start conveyor (including sound!)
                OnButtonUnpressed?.Invoke();
            }
        }
    }
}
