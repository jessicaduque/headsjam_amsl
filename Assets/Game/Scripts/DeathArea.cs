using UnityEngine;

public class DeathArea : MonoBehaviour
{
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
            AudioManager.I.PlaySfx("touchoil");
            _poolManager.ReturnPool(other.gameObject);
        }
        else if (((1 << other.gameObject.layer) & interactableObjectsLayer) != 0)
        {
            AudioManager.I.PlaySfx("touchoil");
            Destroy(other.gameObject);
        }
    }
}
