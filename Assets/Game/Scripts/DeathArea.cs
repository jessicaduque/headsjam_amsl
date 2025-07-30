using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private string deathAreaSfxName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.I.PlaySfx(deathAreaSfxName);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBase>().ModifyHealth(-1);
        }
        else
        {
            Destroy(other.gameObject, 0.6f);
        }
    }
}
