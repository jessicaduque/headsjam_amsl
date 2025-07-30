using UnityEngine;

public class EndLevelObject : MonoBehaviour
{
    [SerializeField] private string nameScene;
    
    private BlackScreenController BlackScreenController => BlackScreenController.I;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            BlackScreenController.FadeOutScene(nameScene);
        }
    }
}
