using System;
using UnityEngine;

public class ConfigurarTutorial : MonoBehaviour
{
    [SerializeField] private GameObject rope;
    
    [SerializeField] private PlayerBase player1;
    [SerializeField] private PlayerBase player2;

    private void Start()
    {
        rope.SetActive(false);
        player1.DisableInputs();
        player2.DisableInputs();
    }
}
