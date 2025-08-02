using DG.Tweening;
using UnityEngine;

public class OilManager : MonoBehaviour
{
    [Header("Oil Dispenser")]
    [SerializeField] private OilDispenser[] oilDispensers;
    private const float TimeTillDispenseOil = 1.2f;
    private const float TimeOilDispensing = 3f;
    private const float DurationTime = 0.8f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (OilDispenser oilDispenser in oilDispensers)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(oilDispenser.dispensedObject.transform.DOMoveY(0f, DurationTime).SetDelay(Random.Range(0.8f, 1.5f)).SetEase(Ease.Linear))
                .Append(
                    oilDispenser.dispensedObject.transform.DOMoveY(-18f, DurationTime).SetDelay(TimeOilDispensing).SetEase(Ease.Linear))
                .SetDelay(TimeTillDispenseOil);
            mySequence.SetLoops(-1, LoopType.Restart);
        }
    }
}
