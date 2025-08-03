using System;
using UnityEngine;

public class ContinueTutorial : MonoBehaviour
{
    [SerializeField] private GameObject Dialogue1;
    [SerializeField] private GameObject Dialogue2;
    private bool alreadyDone = false;
    private int amount = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyDone) return;
        if (other.CompareTag("Player"))
        {
            amount++;
            if (amount == 2)
            {
                Dialogue1.SetActive(false);
                Dialogue2.SetActive(true);
                alreadyDone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (alreadyDone) return;
        if (other.CompareTag("Player"))
        {
            amount--;
        }
    }
}
