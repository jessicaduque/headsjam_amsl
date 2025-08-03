using System.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class ConveyorBelt : MonoBehaviour
    {
        public bool isOn = true;
        public bool clockwise = true;
        public bool hasInfiniteChicken;
        public float speed = 500;
        public Vector3 dummySpawn;

        private static PoolManager PoolManager => PoolManager.I;

        private void Start()
        {
            if (hasInfiniteChicken)
            {
                DeathArea.OnDeathArea += ResetChickens;
            
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
                        rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
                    }
                    else
                    {
                        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
                    }
                }
                else if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableObjects"))
                {
                    if (clockwise)
                    {
                        rb.AddForce(Vector2.left * speed / 20, ForceMode2D.Force);
                    }
                    else
                    {
                        rb.AddForce(Vector2.right * speed / 20, ForceMode2D.Force);
                    }
                }
            }
        }

        private IEnumerator InfiniteChicken()
        {
            while (true)
            {
                PoolManager.GetObject("Dummy", dummySpawn, new Quaternion());
                
                yield return new WaitForSeconds(1.25f);
            }
        }

        private void ResetChickens(GameObject gameObject)
        {
            PoolManager.ReturnPool(gameObject);
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
