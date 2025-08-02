using DG.Tweening;
using UnityEngine;

public class OilManager : MonoBehaviour
{
    [Header("Oil Dispenser")]
    [SerializeField] private GameObject[] dispensedObjects;
    [SerializeField] private GameObject[] oilBubbles;
    private const float TimeTillDispenseOil = 1.2f;
    private const float TimeOilDispensing = 3f;
    private const float DurationTime = 0.8f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        for (var i = 0;  i < dispensedObjects.Length; i++)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .OnStart(() => ActivateBubble(i))
                .SetDelay(i * 0.8f + Random.Range(0, 0.5f))
                .Append(dispensedObjects[i].transform.DOMoveY(0f, DurationTime)
                    .SetDelay(0.7f)
                    .SetEase(Ease.Linear))
                    .OnStart(() => DeactivateBubble(i))
                .Append(
                    dispensedObjects[i].transform.DOMoveY(-18f, DurationTime).SetDelay(TimeOilDispensing)
                        .SetEase(Ease.Linear))
                .SetDelay(TimeTillDispenseOil);
            mySequence.SetLoops(-1, LoopType.Restart);
        }
    }

    private void ActivateBubble(int pos)
    {
        Debug.Log("oi");
        oilBubbles[pos].SetActive(true);
    }
    
    private void DeactivateBubble(int pos)
    {
        Debug.Log("ola");
        oilBubbles[pos].SetActive(false);
    }
}
