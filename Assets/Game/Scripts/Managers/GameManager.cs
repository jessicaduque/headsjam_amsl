using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Singleton;

public class GameManager : DontDestroySingleton<GameManager>
{
    private int currentLevel = 0;
    private int lastLevel = 4;
    
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
        _audioManager.FadeInMusic("mainmenu");
    }

    #region OnSceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckStartOfScene(scene.name);
    }

    private void CheckStartOfScene(string sceneName)
    {
        if (sceneName == "Level1")
        {
            
        }
        else if(sceneName != "MainMenu")
        {
            LevelManager.I.StartLevel();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion
    
    public void ChangeLevel(int nextLevel)
    {
        if (nextLevel > lastLevel)
        {
            Debug.Log("Need to implement what happens when end oof last level here!");
            // Not implemented yet
            return;
        }
        currentLevel = nextLevel;
    }
}
