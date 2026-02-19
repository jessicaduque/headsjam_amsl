using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Singleton;

public class GameManager : DontDestroySingleton<GameManager>
{
    [SerializeField] private Scene[] _scenes;
    [SerializeField] private GameObject askForFullscreen;
    private AudioManager _audioManager => AudioManager.I;
    
    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
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
        StopAllCoroutines();
    }

    #endregion

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
