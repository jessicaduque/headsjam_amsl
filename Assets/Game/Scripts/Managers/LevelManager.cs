using System;
using UnityEngine;
using Utils.Singleton;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] int thisLevel;
    
    public event Action startLevelEvent;
    public event Action timeUpEvent;
    public event Action pauseEvent;
    public event Action gameOverEvent;
    public event Action levelCompleteEvent;
    public LevelState _levelState { get; private set; }
    
    private GameManager _gameManager => GameManager.I;

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

}