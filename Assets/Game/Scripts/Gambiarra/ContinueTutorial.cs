using DG.Tweening;
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
            other.GetComponent<PlayerBase>().FreezePlayer();
            other.transform.localScale = new Vector3(-1, 1, 1);
            amount++;
            
            if (amount == 2)
            {
                Tutorial1.SetActive(false);
                Dialogue1.SetActive(false);
                Dialogue2.SetActive(true);
                window.layer = LayerMask.NameToLayer("InteractableObjects");
                alreadyDone = true;
            }
            else if (amount == 1)
            {
                other.transform.DOMoveX(transform.position.x + 0.3f, 0.8f);
            }
        }
    }

}
