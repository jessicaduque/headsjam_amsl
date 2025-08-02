using System;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public bool isOn = true;
    public bool clockwise = true;
    public float speed = 500;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isOn)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Players") ||
                collision.gameObject.layer == LayerMask.NameToLayer("InteractableObjects"))
            {
                var rb = collision.gameObject.GetComponent<Rigidbody2D>();
                
                if (clockwise)
                {
                    rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
                }
            }
        }
    }

    public void ToggleOnOff()
    {
        isOn = !isOn;
    }
    
    public void ToggleDirection()
    {
        clockwise = !clockwise;
    }
}
