using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : DontDestroySingleton<BlackScreenController>
{
    [SerializeField] private GameObject blackScreenPanel;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    private float BlackFadeTime => Helpers.blackFadeTime;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        FadeOutSceneStart();
    }

    #region OnSceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeOutSceneStart();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region Fade in and out

    private void FadeInBlack() // Black screen is turned on to fade in
    {
        blackScreenPanel.SetActive(true);
        blackScreenCanvasGroup.DOFade(1, BlackFadeTime).SetUpdate(true);
    }

    private void FadeOutBlack() // Black screen fades out then turns off
    {
        blackScreenCanvasGroup.DOFade(0, BlackFadeTime).SetUpdate(true).OnComplete(() => blackScreenPanel.SetActive(false));
    }

    #endregion

    #region Fades with scenes
    private void FadeOutSceneStart() // When it's the first frame of scene, ideal to apply to start as pitch black then fade out
    {
        blackScreenPanel.SetActive(true);
        blackScreenCanvasGroup.alpha = 1f;
        blackScreenCanvasGroup.DOFade(0, BlackFadeTime).SetUpdate(true).OnComplete(() => blackScreenPanel.SetActive(false));
    }

    public void FadeOutScene(string nextScene) // When it is needed to go to next scene, triggers a black fade in before activating the change of scene
    {
        blackScreenPanel.SetActive(true);
        blackScreenCanvasGroup.DOFade(1, BlackFadeTime).OnComplete(() => {
            SceneManager.LoadScene(nextScene);
            }).SetUpdate(true);
    }

    public void RestartGame() // Restarts the scene with a smooth black fade transition
    {
        blackScreenPanel.SetActive(true);
        blackScreenCanvasGroup.DOFade(1, BlackFadeTime).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name)).SetUpdate(true);
    }

    #endregion

    #region Fades with panels
    public void FadeBlackToControlPanel(GameObject panel, bool estado)
    {
        blackScreenPanel.SetActive(true);
        blackScreenCanvasGroup.DOFade(1, BlackFadeTime).SetUpdate(true).onComplete = () => {
            panel.SetActive(estado);
            FadeOutBlack();
        };
    }
    #endregion

}