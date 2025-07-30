using System;
using UnityEngine;

public class OilDispenser : MonoBehaviour
{
    [SerializeField] bool startsDispensing = false;
    private float _timeTillDispenseOil = 1.2f;
    private float _timeOilDispensing = 3f;

    private void Start()
    {
        throw new NotImplementedException();
    }
}
