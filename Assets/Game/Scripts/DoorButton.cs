using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Door[] doors;
    private int _amountObjectsPressing = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HeavyObject") || other.CompareTag("Player"))
        {
            _amountObjectsPressing++;
            foreach (Door door in doors)
            {
                door.TriggerDoor(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HeavyObject")|| other.CompareTag("Player"))
        {
            _amountObjectsPressing--;
            if (_amountObjectsPressing == 0)
            {
                foreach (Door door in doors)
                {
                    door.TriggerDoor(false);
                }   
            }
        }
    }
}
