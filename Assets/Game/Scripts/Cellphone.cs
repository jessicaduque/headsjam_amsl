using UnityEngine;

public class Cellphone : MonoBehaviour
{
    [SerializeField] PlayerBase player1;
    [SerializeField] PlayerBase player2;
    
    [SerializeField] GameObject dialogueManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player1.DisableInputs();
            player2.DisableInputs();
            dialogueManager.SetActive(true);
        }
    }
}
