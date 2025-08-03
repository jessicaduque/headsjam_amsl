using System;
using System.Collections;
using UnityEngine;
using Utils.Singleton;

public class LevelManager : Singleton<LevelManager>
{
    private int tutorialLevel = 0;
    [SerializeField] int thisLevel;
    [SerializeField] private float thisLevelTimeSeconds;
    public event Action startLevelEvent;
    public event Action timeUpEvent;
    public event Action pauseEvent;
    public event Action gameOverEvent;
    public event Action levelCompleteEvent;
    public LevelState _levelState { get; private set; }
    
    private GameManager _gameManager => GameManager.I;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        ///TEMPORARY.
        StartLevel();
    }

    #region StartLevel Level
    public void StartLevel()
    {
        Time.timeScale = 1;
        _levelState = LevelState.PLAYING;
        startLevelEvent?.Invoke();
    }
    #endregion

    #region Pause
    public void Pause()
    {
        _levelState = LevelState.PAUSED;
        Time.timeScale = 0;
        pauseEvent?.Invoke();
    }

    #endregion

    #region Time Up
    public void TimeUp()
    {
        _levelState = LevelState.END;
        timeUpEvent?.Invoke();
    }

    #endregion
    
    #region Game Over
    public void GameOver()
    {
        _levelState = LevelState.END;
        gameOverEvent?.Invoke();
    }

    #endregion
    
    #region Level Complete
    public void LevelComplete()
    {
        _levelState = LevelState.END;
        _gameManager.ChangeLevel(thisLevel + 1);
        levelCompleteEvent?.Invoke();
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