using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class GameOver : Singleton<GameOver>
{
    CanvasGroup _canvasGroup;

    [SerializeField]  Button b_replay;
    [SerializeField]  Button b_exit;

    private BlackScreenController _blackScreenController => BlackScreenController.I;

    protected override void Awake()
    {
        base.Awake();
        
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(WaitForPanelReady());
    }
    
    private IEnumerator WaitForPanelReady()
    {
        b_replay.enabled = false;
        b_exit.enabled = false;
        while (_canvasGroup.alpha != 1)
        {
            yield return null;
        }
        Time.timeScale = 0;
        b_replay.onClick.AddListener(() => {
            AudioManager.I.FadeOutMusic("mainmusic");
            _blackScreenController.RestartGame();
        });

        b_exit.onClick.AddListener(() => {
            AudioManager.I.PlayCrossFade("menumusic");
            _blackScreenController.FadeOutScene("MainMenu");
        });
        b_replay.enabled = true;
        b_exit.enabled = true;
    }
}