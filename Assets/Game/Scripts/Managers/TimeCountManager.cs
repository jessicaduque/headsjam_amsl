using TMPro;
using UnityEngine;
using System.Collections;
using Utils.Singleton;

public class TimeCountManager : Singleton<TimeCountManager>
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI t_time;

    [Header("Timer Settings")]
    private float _startTimeSeconds;
    private float _currentTime = 121;

    private bool _timeOver = false;
    private bool _timerEnding = false;
    
    Coroutine _timerCoroutine;

    private AudioManager _audioManager => AudioManager.I;
    
    private void Start()
    {
        _startTimeSeconds = LevelManager.I.GetLevelTime() + 1;
        _currentTime = _startTimeSeconds == 0 ? _startTimeSeconds : _currentTime;
        SetTimeText();
        
        LevelManager.I.pauseEvent += TimerEnd;
        LevelManager.I.gameOverEvent += TimerEnd;
        LevelManager.I.levelCompleteEvent += TimerEnd;
    }

    private void OnDisable()
    {
        StopCoroutine(_timerCoroutine);
    }

    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(StartTimerCoroutine());
    }

    public IEnumerator StartTimerCoroutine()
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
                LevelManager.I.TimeUp();
            }
            else if (_currentTime > 6)
            {
                _audioManager.StopSfx2();
                _timerEnding = false;
            }

            SetTimeText();
            yield return null;
        }
        
        TimerEnd();
    }

    public void TimerEnd()
    {
        _audioManager.StopSfx2();
        _timerEnding = false;
        StopCoroutine(_timerCoroutine);
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
