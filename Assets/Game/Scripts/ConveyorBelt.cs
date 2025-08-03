using System.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("Configuration")]
        public bool isOn = true;
        public bool clockwise = true;
        public bool hasInfiniteChicken;
        public float speed = 500;
        public string tagPool;
        
        [Header("Positioning")]
        public Transform startPosition;

        private static PoolManager _poolManager => PoolManager.I;

        private void Start()
        {
            if (hasInfiniteChicken)
            {
                StartCoroutine(InfiniteChicken());
            }
        }
    
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (isOn)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            
                if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
                {
                    if (clockwise)
                    {
                        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
                    }
                }
                else if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableObjects"))
                {
                    if (clockwise)
                    {
                        rb.AddForce(Vector2.right * speed / 20, ForceMode2D.Force);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * speed / 20, ForceMode2D.Force);
                    }
                }
            }
        }

        private IEnumerator InfiniteChicken()
        {
            while (true)
            {
                _poolManager.GetObject(tagPool, startPosition.position, new Quaternion());
                
                yield return new WaitForSeconds(1.25f);
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
}
