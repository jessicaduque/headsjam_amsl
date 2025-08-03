using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private bool isOn = true;
        [SerializeField] private bool clockwise = true;
        [SerializeField] private bool hasInfiniteChicken;
        [SerializeField] private float speed = 500;
        [SerializeField] private string tagPool;
        
        [Header("Positioning")]
        public Transform startPosition;

        [Header("Tilemap")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile[] normalConveyorTile;
        [SerializeField] private AnimatedTile[] animatedConveyorTile;
        
        private static PoolManager _poolManager => PoolManager.I;

        private void Start()
        {
            //ConveyorButton.OnButtonPressed += StopConveyor;
            ConveyorButton.OnButtonUnpressed += StartConveyor;
            
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

        private void StartConveyor()
        {
            isOn = true;
            // Start sound
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            if (hasInfiniteChicken) StartCoroutine(InfiniteChicken());
        }

        private IEnumerator StopConveyor()
        {
            Debug.Log("Stopping Conveyor");
            if (hasInfiniteChicken) StopCoroutine(InfiniteChicken());
            yield return new WaitForSeconds(2f);
            
            isOn = false;
            // Stop sound
        }
    }
}
