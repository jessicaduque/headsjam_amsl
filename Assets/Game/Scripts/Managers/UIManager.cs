using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Singleton;

public class UIManager : Singleton<UIManager>
{
    // Input fields
    private Player_UI _playerUIActionsAsset;

    [Header("Panels for each game state")]
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;

    LevelManager _levelManager => LevelManager.I;
    private AudioManager _audioManager => AudioManager.I;
    protected override void Awake()
    {
        base.Awake();
        
        _playerUIActionsAsset = new Player_UI();
    }

    private void Start()
    {
        _levelManager.startLevelEvent += EnableInputs;
        _levelManager.pauseEvent += DisableInputs;
        _levelManager.blessingsEvent += () => _blessingPanel.SetActive(true);
        _levelManager.countdownEvent += () =>
        {
            Helpers.FadeOutPanel(_blessingPanel);
            Helpers.FadeInPanel(_countdownPanel);
            
        };
        _levelManager.startLevelEvent += () =>
        {
            _audioManager.FadeInMusic("mainmusic");
            Helpers.FadeCrossPanel(_countdownPanel, _hudPanel);
        };
        _levelManager.timeUpEvent += () =>
        {
            DisableInputs();
            _audioManager.PlaySfx("levelend");
        };

        _levelManager.BeginBlessings();
    }

    #region Input

    public void EnableInputs()
    {
        _playerUIActionsAsset.UI.PauseGame.started += DoPauseControl;

        _playerUIActionsAsset.UI.Enable();
    }

    public void DisableInputs()
    {
        _playerUIActionsAsset.UI.PauseGame.started -= DoPauseControl;

        _playerUIActionsAsset.UI.Disable();
    }

    #endregion

    #region Gameover Control

    public void ActiavateGameOverPanel()
    {
        Helpers.FadeInPanel(_gameOverPanel);
    }
    
    #endregion
    
    #region Pause Control

    private void DoPauseControl(InputAction.CallbackContext obj)
    {
        _levelManager.Pause();

        Helpers.FadeInPanel(_pausePanel);
    }

    #endregion
}
