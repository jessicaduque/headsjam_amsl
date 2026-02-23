using System;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Utils.Singleton.Singleton<LevelManager>
{
    private int tutorialLevel = 0;
    [SerializeField] int thisLevel;
    [SerializeField] private float thisLevelTimeSeconds;
    [SerializeField] private PlayerBase[] players;
    public event Action timeUpEvent;
    public event Action pauseEvent;
    public event Action gameOverEvent;
    public event Action levelCompleteEvent;
    public LevelState _levelState { get; private set; }

    private void Start()
    {
        StartLevel();
    }

    #region StartLevel Level
    public void StartLevel()
    {
        Time.timeScale = 1;
        if (_levelState == LevelState.PLAYING) return;
        _levelState = LevelState.PLAYING;
        StartCoroutine(TimeCountManager.I.StartTimer());
        UIManager.I.StartMethod();
        foreach (var player in players)
        {
            player.EnableInputs();
        }
    }
    #endregion

    #region Pause
    public void Pause()
    {
        if (_levelState == LevelState.PAUSED) return;
        _levelState = LevelState.PAUSED;
        Time.timeScale = 0;
        pauseEvent?.Invoke();
    }

    #endregion

    #region Time Up
    public void TimeUp()
    {
        if (_levelState == LevelState.END) return;
        Debug.Log("Time Up Started!");
        _levelState = LevelState.END;
        timeUpEvent?.Invoke();
        Debug.Log("Time Up Ended!");
    }

    #endregion
    
    #region Game Over
    public void GameOver()
    {
        if (_levelState == LevelState.END) return;
        Debug.Log("Game Over Started!");
        _levelState = LevelState.END;
        gameOverEvent?.Invoke();
        Debug.Log("Game Over Ended");
    }

    #endregion
    
    #region Level Complete
    public void LevelComplete()
    {
        if (_levelState == LevelState.END) return;
        Debug.Log("Level Complete Started!");
        _levelState = LevelState.END;
        levelCompleteEvent?.Invoke();
        Debug.Log("Level Complete Ended!");
    }

    #endregion

    #region GET

    public float GetLevelTime()
    {
        return thisLevelTimeSeconds;
    }

    public bool IsLevelTutorial()
    {
        return thisLevel == tutorialLevel;
    }
    
    #endregion
}