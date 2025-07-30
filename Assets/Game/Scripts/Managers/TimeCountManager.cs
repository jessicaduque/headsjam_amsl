using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using Utils.Singleton;

public class TimeCountManager : Singleton<TimeCountManager>
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI t_time;

    [Header("Timer Settings")]
    [SerializeField] private float _startTimeSeconds;
    private float _currentTime;

    private bool _timeOver = false;
    private bool _timerEnding = false;

    private LevelManager _levelManager => LevelManager.I;
    private AudioManager _audioManager => AudioManager.I;

    private void Start()
    {
        _currentTime = _levelManager.GetLevelTime() + 1;
        SetTimeText();
        
        _levelManager.startLevelEvent += delegate { StartCoroutine(StartTimer()); };
        
        _levelManager.pauseEvent += TimerEnd;
        _levelManager.gameOverEvent += TimerEnd;
        _levelManager.levelCompleteEvent += TimerEnd;
    }

    private IEnumerator StartTimer()
    {
        while (!_timeOver)
        {
            _currentTime -= Time.deltaTime;

            if (!_timerEnding && _currentTime <= 6)
            {
                _audioManager.PlaySfx2("clocktick");
                _timerEnding = true;
            }
            else if (_currentTime <= 0)
            {
                _audioManager.StopSfx2();
                _timeOver = true;
            }
            else if (_currentTime > 6)
            {
                _audioManager.StopSfx2();
                _timerEnding = false;
            }

            SetTimeText();
            yield return null;
        }
        
        _levelManager.TimeUp();
        TimerEnd();
    }

    public void TimerEnd()
    {
        _audioManager.StopSfx2();
        _timerEnding = false;
        StopAllCoroutines();
    }

    #region Set

    private void SetTimeText()
    {
        int minutes = (int)_currentTime / 60;
        int seconds = (int)_currentTime - minutes * 60;
        t_time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion
}
