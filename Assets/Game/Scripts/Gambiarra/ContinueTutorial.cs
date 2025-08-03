using System;
using UnityEngine;

public class ContinueTutorial : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject Tutorial1;
    [SerializeField] private GameObject Dialogue1;
    [SerializeField] private GameObject Dialogue2;
    [SerializeField] private PlayerBase player1;
    [SerializeField] private PlayerBase player2;
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
                Tutorial1.SetActive(false);
                Dialogue1.SetActive(false);
                Dialogue2.SetActive(true);
                player1.DisableInputs();
                player2.DisableInputs();
                window.layer = LayerMask.NameToLayer("InteractableObjects");
                player1.transform.localScale = new Vector3(-1, 1, 1);
                player2.transform.localScale = new Vector3(-1, 1, 1);
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
