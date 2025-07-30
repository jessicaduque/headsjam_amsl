using System;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private Door[] doors;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HeavyObject"))
        {
            foreach (Door door in doors)
            {
                door.TriggerDoor(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HeavyObject"))
        {
            foreach (Door door in doors)
            {
                door.TriggerDoor(false);
            }
        }
    }
}
