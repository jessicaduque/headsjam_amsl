using UnityEngine;

public class Feet : MonoBehaviour
{
    [SerializeField] private PlayerBase _player;
    
    private void IsGrounded(bool isGrounded)
    {
        Debug.Log("isGrounded: " + isGrounded);
        _player.SetIsGrounded(isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGrounded(false);
        }
    }
}
