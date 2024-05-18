using System.Collections;

using UnityEngine;

using TMPro;
using DG.Tweening;

using UIFlow;

public class ToastViewController : ViewController
{
    private static ToastViewController _instance;

    private string _id;

    [SerializeField] private RectTransform _background;
    [SerializeField] private TextMeshProUGUI _message;

    // Methods

    public static ToastViewController Present(string id, string message, float duration = 2)
    {
        // Avoid spamming toasts with the same id.
        if (_instance != null)
            if(id == _instance._id)
                return _instance;

        _instance = Storyboard.Present<ToastViewController>(false);
        _instance._id = id;
        _instance.Set(message, duration);
        return _instance;
    }

    private void Set(string text, float duration)
    {
        _message.text = text;
        StartCoroutine(Countdown(duration));
    }

    private IEnumerator Countdown(float duration)
    {
       // _background.DOAnchorPosY(0, 5);
        yield return new WaitForSeconds(duration);
        Dismiss();
    }

    public override void OnPresentTransition()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, Transition.Appear);
        Vector2 anchoredPosition = _background.anchoredPosition;
        anchoredPosition.y = -200;
        _background.anchoredPosition = anchoredPosition;
        _background.DOAnchorPosY(-175, Transition.Appear);
        _background.DOPunchScale(Vector3.one * .1f, Transition.Appear);
        _background.DOPunchRotation(Vector3.forward * 5, Transition.Appear, 30, 1);
    }

    public override void OnDismissTransition()
    {
        CanvasGroup.DOFade(0, Transition.Disappear);
        _background.DOAnchorPosY(0, Transition.Disappear);
        _background.DOPunchRotation(Vector3.forward * 5, Transition.Disappear, 30, 1);
    }
}
