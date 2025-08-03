using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Singleton;

public class GameManager : DontDestroySingleton<GameManager>
{
    [SerializeField] private Scene[] _scenes;
    [SerializeField] private GameObject askForFullscreen;
    private int _currentScene;
    private AudioManager _audioManager => AudioManager.I;
    
    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        CheckStartOfScene("MainMenu");
        _audioManager.FadeInMusic("menumusic");
    }

    private void Update()
    {
        if (IsInFullscreen())
        {
            askForFullscreen.SetActive(false);
        }
        else
        {
            askForFullscreen.SetActive(true);
        }
    }

    #region OnSceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckStartOfScene(scene.name);
    }

    private void CheckStartOfScene(string sceneName)
    {
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3")
        {
            StartCoroutine(AAA());
        }
    }

    private IEnumerator AAA()
    {
        yield return new WaitForSeconds(0.2f);
        LevelManager.I.StartLevel();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion
    
    public void CompleteLevel()
    {
        _currentScene += 1;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(_scenes[_currentScene].name);
    }

    #region CheckFullscreen

    [DllImport("__Internal")]
    private static extern bool IsFullscreen();

    public bool IsInFullscreen()
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        return IsFullscreen();
    #else
        return true;
    #endif
    }

    #endregion
}
