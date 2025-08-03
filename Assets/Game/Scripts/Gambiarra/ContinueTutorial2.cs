using System;
using UnityEngine;

public class ContinueTutorial2 : MonoBehaviour
{
    [SerializeField] private GameObject Dialogue2;
    [SerializeField] private GameObject Dialogue3;

    private void Start()
    {
        Dialogue2.SetActive(false);
        Dialogue3.SetActive(true);
    }
}
