using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private string deathAreaSfxName;
    [SerializeField] private LayerMask interactableObjectsLayer;
    private static PoolManager _poolManager => PoolManager.I;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBase>().ModifyHealth(-1);
        }
        else if (other.CompareTag("Dummy"))
        {
            _poolManager.ReturnPool(other.gameObject);
        }
        else if (((1 << other.gameObject.layer) & interactableObjectsLayer) != 0)
        {
            Destroy(other.gameObject);
        }
        else return;
        AudioManager.I.PlaySfx(deathAreaSfxName);
    }
}
