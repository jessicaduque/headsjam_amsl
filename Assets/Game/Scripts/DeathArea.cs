using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private string deathAreaSfxName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBase>().ModifyHealth(-1);
            AudioManager.I.PlaySfx(deathAreaSfxName);
        }
    }
}
