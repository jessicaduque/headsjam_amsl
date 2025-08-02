using System;
using UnityEngine;
using DG.Tweening;

public class OilDispenser : MonoBehaviour
{
    [SerializeField] bool startsDispensing = false;
    private float _timeTillDispenseOil = 1.2f;
    private float _timeOilDispensing = 3f;

    private void Start()
    {
        // spriterenderer.bounds.size.y 
        transform.DOMoveY();
        throw new NotImplementedException();
    }
}
