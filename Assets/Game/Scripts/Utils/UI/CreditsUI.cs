using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;

    private void OnEnable()
    {
        _scrollbar.value = 1;
    }
}