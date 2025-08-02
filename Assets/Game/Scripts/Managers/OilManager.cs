using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OilManager : MonoBehaviour
{
    [Header("Oil Dispenser")]
    [SerializeField] private GameObject[] dispensedObjects;
    [SerializeField] private GameObject[] oilBubbles;
    private Vector3[] _dispensedOriginalPositions;

    private const float TimeTillDispenseOil = 1.2f;
    private const float TimeOilDispensing = 3f;
    private const float DurationTime = 0.8f;
    
    private void Start()
    {
        _dispensedOriginalPositions = new Vector3[dispensedObjects.Length];
        
        for (var i = 0;  i < dispensedObjects.Length; i++)
        {
            _dispensedOriginalPositions[i] = dispensedObjects[i].transform.position;

            StartCoroutine(Sequence(i));
        }
    }

    private IEnumerator Sequence(int pos)
    {
        yield return new WaitForSeconds(pos * 0.8f + Random.Range(0, 0.5f));
        SetBubble(pos, true);
        
        yield return new WaitForSeconds(0.7f);
        SetBubble(pos, false);
        MoveDispensedObject(pos, 0);
        
        yield return new WaitForSeconds(TimeOilDispensing);
        MoveDispensedObject(pos, -18f);
        
        yield return new WaitForSeconds(TimeTillDispenseOil);
        Restart(pos);
    }

    private void Restart(int pos)
    {
        dispensedObjects[pos].transform.position = _dispensedOriginalPositions[pos];
        StartCoroutine(Sequence(pos));
    }
    
    private void MoveDispensedObject(int pos, float yValue)
    {
        dispensedObjects[pos].transform.DOMoveY(yValue, DurationTime).SetEase(Ease.Linear);
    }

    private void SetBubble(int pos, bool active)
    {
        oilBubbles[pos].SetActive(active);
    }
}
