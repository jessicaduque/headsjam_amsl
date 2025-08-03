using System;
using UnityEngine;

public class ContinueTutorial2 : MonoBehaviour
{
    [SerializeField] private GameObject Dialogue3;
    [SerializeField] private GameObject Dialogue4;

    private void Start()
    {
        Dialogue3.SetActive(false);
        Dialogue4.SetActive(true);
    }
}
