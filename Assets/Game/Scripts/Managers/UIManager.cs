using System;
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
    private AudioManager _audioManager => AudioManager.I;
    protected override void Awake()
    {
        base.Awake();
        
        _playerUIActionsAsset = new Player_UI();
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartMethod()
    {
        _pauseButton.interactable = true;
        EnableInputs();
        Helpers.FadeInPanel(_hudPanel);
    }

    private void Start()
    {
        _pauseButton.onClick.AddListener(PauseGame);
        
        LevelManager.I.timeUpEvent += () =>
        {
            LevelManager.I.GameOver();
        };
        LevelManager.I.gameOverEvent += () =>
        {
            DisableInputs();
            StartCoroutine(ActivateGameOverPanel());
        };
        LevelManager.I.levelCompleteEvent += DisableInputs;
        
        
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

    private void PauseGame()
    {
        _pauseButton.interactable = false;
        DisableInputs();
        Helpers.FadeInPanel(_pausePanel);
        LevelManager.I.Pause();
    }

    #endregion
    
    #region HUD Panel

    public void DisableHUD()
    {
        Helpers.FadeOutPanel(_hudPanel);
    }
    
    public void DisablePause()
    {
        Helpers.FadeOutPanel(_pausePanel);
    }
    
    #endregion
}
