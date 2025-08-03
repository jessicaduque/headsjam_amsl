using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;

public class UIManager : Singleton<UIManager>
{
    // Input fields
    private Player_UI _playerUIActionsAsset;

    [Header("Panels for each game state")]
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    
    [SerializeField] private Button _pauseButton;
    LevelManager _levelManager => LevelManager.I;
    private AudioManager _audioManager => AudioManager.I;
    protected override void Awake()
    {
        base.Awake();
        
        _playerUIActionsAsset = new Player_UI();
    }

    private void Start()
    {
        _pauseButton.onClick.AddListener(_levelManager.Pause);
        _levelManager.startLevelEvent += () =>
        {
            _pauseButton.interactable = true;
            EnableInputs();
            Helpers.FadeInPanel(_hudPanel);
        };
        _levelManager.pauseEvent += () =>
        {
            _pauseButton.interactable = false;
            DisableInputs();
        };
        _levelManager.timeUpEvent += () =>
        {
            _levelManager.GameOver();
            DisableInputs();
        };
        _levelManager.gameOverEvent += () =>
        {
            StartCoroutine(ActivateGameOverPanel());
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

    public IEnumerator ActivateGameOverPanel()
    {
        yield return new WaitForSeconds(2f);
        Helpers.FadeInPanel(_gameOverPanel);
    }
    
    #endregion
    
    #region Pause Control

    private void DoPauseControl(InputAction.CallbackContext obj)
    {
        PauseGame();
    }

    public void PauseGame()
    {
        _levelManager.Pause();

        Helpers.FadeInPanel(_pausePanel);
    }

    #endregion
}
