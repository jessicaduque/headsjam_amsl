using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    [Header("Play")]
    [SerializeField] Button b_play;

    [Header("Settings")]
    [SerializeField] Button b_settings;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] Button b_leaveSettings;

    [Header("Credits")]
    [SerializeField] Button b_credits;
    [SerializeField] GameObject _creditsPanel;
    [SerializeField] Button b_leaveCredits;
    
    [Header("Cutscene")]
    [SerializeField] Button b_skip;
    [SerializeField] Button b_watchCutscene;
    [SerializeField] GameObject _cutscenePanel;
    [SerializeField] Animator _cutsceneAnimator;

    BlackScreenController _blackScreenController => BlackScreenController.I;
    AudioManager _audioManager => AudioManager.I;

    private void OnEnable()
    {
        StartCoroutine(WaitForPanelReady());
        ButtonThresholdSetup();
        AddButtonListeners();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #region Play

    private void PlayButtonControl()
    {
        _blackScreenController.FadeOutScene("Tutorial");
        _audioManager.FadeOutMusic("menumusic");
    }

    #endregion

    #region Settings

    private void SettingsButtonControl(bool state)
    {
        _blackScreenController.FadeBlackToControlPanel(_settingsPanel, state);
    }

    #endregion

    #region Credits

    private void CreditsButtonControl(bool state)
    {
        _blackScreenController.FadeBlackToControlPanel(_creditsPanel, state);
    }

    #endregion

    #region Quit

    private void QuitGame()
    {
        Application.Quit();
    }
    
    #endregion
    
    #region Cutscene

    public IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1);
        _cutsceneAnimator.SetTrigger("Start");
    }


    public void EndCutscene()
    {
        _cutsceneAnimator.StopPlayback();
        _blackScreenController.FadeBlackToControlPanel(_cutscenePanel, false);
        _audioManager.FadeInMusic("menumusic");
    }

    public void DisableCutscene()
    {
        _cutscenePanel.SetActive(false);
        _audioManager.FadeInMusic("menumusic");
    }

    #endregion

    #region Buttons
    private void ButtonThresholdSetup()
    {
        float threshold = 0.4f;

        b_play.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_settings.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_credits.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_leaveSettings.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_leaveCredits.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_skip.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
        b_watchCutscene.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
    }

    private void AddButtonListeners()
    {
        b_play.onClick.AddListener(PlayButtonControl);
        b_settings.onClick.AddListener(() => SettingsButtonControl(true));
        b_credits.onClick.AddListener(() => CreditsButtonControl(true));
        b_leaveSettings.onClick.AddListener(() => SettingsButtonControl(false));
        b_leaveCredits.onClick.AddListener(() => CreditsButtonControl(false));
        b_skip.onClick.AddListener(EndCutscene); 
        b_watchCutscene.onClick.AddListener(() => {
            _audioManager.FadeOutMusic("menumusic");
            _blackScreenController.FadeBlackToControlPanel(_cutscenePanel, true); StartCoroutine(StartCutscene()); 
        });
    }

    private void ButtonEnableControl(bool state)
    {
        b_play.enabled = state;
        b_settings.enabled = state;
        b_credits.enabled = state;
    }

    #endregion
    private IEnumerator WaitForPanelReady()
    {
        ButtonEnableControl(false);

        yield return new WaitForSeconds(Helpers.blackFadeTime);

        ButtonEnableControl(true);
    }
}
