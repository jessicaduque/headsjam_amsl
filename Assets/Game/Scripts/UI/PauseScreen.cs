using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    private float _animationTime = 0.5f;

    [SerializeField] Button[] _buttons;

    BlackScreenController _blackScreenController => BlackScreenController.I;
    private LevelManager _levelManager => LevelManager.I;
    private void Start()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForPanelStart());
    }

    private IEnumerator WaitForPanelStart()
    {
        ResetButtons();

        yield return new WaitForSecondsRealtime(Helpers.panelFadeTime / 2);

        ButtonsAnimation();
        
        yield return new WaitForSecondsRealtime(_animationTime);

        ButtonsActivationControl(true);
    }

   
    #region
    private void SetupButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            string name = _buttons[i].GetComponent<ButtonExtra>().nameTag;
            switch (name)
            {
                case "resume":
                    _buttons[i].onClick.AddListener(() => {
                        ButtonsActivationControl(false);
                        Helpers.FadeOutPanel(gameObject);
                        _levelManager.StartLevel();
                    });
                    break;
                case "restart":
                    _buttons[i].onClick.AddListener(() => _blackScreenController.RestartGame());
                    break;
                case "exit":
                    _buttons[i].onClick.AddListener(() => _blackScreenController.FadeOutScene("MainMenu"));
                    break;
            }
        }
    }

    private void ResetButtons()
    {
        ButtonsActivationControl(false);

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].gameObject.transform.localScale = Vector3.zero;
        }
    }

    private void ButtonsActivationControl(bool state)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].enabled = state;
        }
    }

    private void ButtonsAnimation()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].GetComponent<RectTransform>().DOScale(1, _animationTime).SetEase(Ease.OutBounce).SetDelay(i * 0.1f).SetUpdate(true);
        }
    }

    #endregion Buttons

}
