using UnityEngine;

using TMPro;

using DG.Tweening;

using UIFlow;

public class LoadingViewController : ViewController
{
    private static LoadingViewController _instance;

    [SerializeField] private TextMeshProUGUI _message;

    // Methods

    public static LoadingViewController Present(string text = "")
    {
        if (_instance == null)
            _instance = Storyboard.Present<LoadingViewController>();

        _instance.Set(text);

        return _instance;
    }

    public static void SetMessage(string text)
    {
        if (_instance == null)
            return;

        _instance.Set(text);
    }

    public void Set(string text) => _message.text = text;

    public override void OnPresentTransition()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, Transition.Appear);
    }

    public override void OnDismissTransition()
    {
        CanvasGroup.DOFade(0, Transition.Disappear);
    }


    public static void Release()
    {
        if (_instance == null)
            return;

        _instance.Dismiss();

        _instance = null;
    }
}
