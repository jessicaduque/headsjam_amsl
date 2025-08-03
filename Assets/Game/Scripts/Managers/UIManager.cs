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
        _levelManager.startLevelEvent += () =>
        {
            EnableInputs();
            Helpers.FadeInPanel(_hudPanel);
        };
        _levelManager.pauseEvent += DisableInputs;
        _levelManager.timeUpEvent += () =>
        {
            _levelManager.GameOver();
            DisableInputs();
        };
        _levelManager.gameOverEvent += () =>
        {
            ActivateGameOverPanel();
            DisableInputs();
        };
        _levelManager.levelCompleteEvent += DisableInputs;
        
        
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

    public void ActivateGameOverPanel()
    {
        Helpers.FadeInPanel(_gameOverPanel);
    }
    
    #endregion

    private void DialogueControl(bool activate)
    {
        if(activate) Helpers.FadeInPanel(_dialoguePanel);
        else Helpers.FadeOutPanel(_dialoguePanel);
    }
    
    #region Pause Control

    private void DoPauseControl(InputAction.CallbackContext obj)
    {
        _levelManager.Pause();

        Helpers.FadeInPanel(_pausePanel);
    }

    #endregion
}
