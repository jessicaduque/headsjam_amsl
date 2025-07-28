using UnityEngine;
using DG.Tweening;
using TMPro;

public class HowToPlayUI : MonoBehaviour
{
    [SerializeField] CanvasGroup _firstPageCG;
    [SerializeField] CanvasGroup _secondPageCG;

    [SerializeField] TextMeshProUGUI _pageTitle;
    [SerializeField] TextMeshProUGUI _pageNumber;

    private void OnEnable()
    {
        _firstPageCG.alpha = 1;
        _secondPageCG.alpha = 0;
        _pageTitle.text = "Como Jogar";
        _pageNumber.text = "1/2";
    }

    public void ChangePage(bool goToFirst)
    {
        _firstPageCG.DOFade((goToFirst ? 1 : 0), 0.3f);
        _secondPageCG.DOFade((goToFirst ? 0 : 1), 0.3f);

        if (goToFirst)
        {
            _pageTitle.text = "Como Jogar";
            _pageNumber.text = "1/2";
        }
        else
        {
            _pageTitle.text = "Como Vencer";
            _pageNumber.text = "2/2";
        }

    }
}