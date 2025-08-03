using UnityEngine;

public class ConveyorButton : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private bool startsPressed;
    [SerializeField] private bool possiblePress = true;
    [SerializeField] private Collider2D solidCollider;
    private int _amountObjectsPressing;
    private SpriteRenderer _spriteRenderer;

    public static event System.Action OnButtonPressed;
    public static event System.Action OnButtonUnpressed;

    private LevelManager _levelManager => LevelManager.I;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = startsPressed ? buttonSprites[1] : buttonSprites[0];
    }

    private void Start()
    {
        GlassBlockingObject.OnBreak += CanBePressed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_levelManager._levelState != LevelState.PLAYING) return;
        if (!possiblePress) return;
        if (other.CompareTag("HeavyObject") || other.CompareTag("Player") || other.CompareTag("Dummy"))
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
        if (!possiblePress) return;
        if (other.CompareTag("HeavyObject")|| other.CompareTag("Player") || other.CompareTag("Dummy"))
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

    private void CanBePressed()
    {
        possiblePress = true;
    }
}
