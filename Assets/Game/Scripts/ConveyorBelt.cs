using System.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private GameObject[] friedChickens;
    
        public bool isOn = true;
        public bool clockwise = true;
        public bool hasInfiniteChicken;
        public float speed = 500;

        private void Start()
        {
            DeathArea.OnDeathArea += ResetChickens;
            
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

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableObjects"))
            {
                // ResetChickens(collision.gameObject);
                // StartCoroutine(InfiniteChicken());
            }
        }

        private IEnumerator InfiniteChicken()
        {
            foreach (var friedChicken in friedChickens)
            {
                friedChicken.SetActive(true);
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void ResetChickens(GameObject gameObject)
        {
            gameObject.transform.position = new Vector3(16, 0);
            gameObject.SetActive(false);
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
