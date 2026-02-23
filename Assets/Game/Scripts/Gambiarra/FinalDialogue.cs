using System.Collections;
using UnityEngine;

public class FinalDialogue : MonoBehaviour
{
    [SerializeField] private GameObject happyPanel;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(10f);
        BlackScreenController.I.FadeBlackToControlPanel(happyPanel, true);
        yield return new WaitForSeconds(8);
        BlackScreenController.I.FadeOutScene("MainMenu");
        AudioManager.I.FadeInMusic("menumusic");
    }
}
