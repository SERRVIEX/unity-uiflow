using System.Collections;

using UnityEngine;

using TMPro;
using DG.Tweening;

using UIFlow;

public class CommandViewController : ViewController
{
    private static CommandViewController _instance;

    [SerializeField] private TextMeshProUGUI _message;

    // Methods

    public static CommandViewController Present(string message)
    {
        _instance = Storyboard.Present<CommandViewController>(false);
        _instance.Set(message);
        return _instance;
    }

    private void Set(string text)
    {
        _message.text = text;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        for(float t = 0; t < 1; t += Time.deltaTime)
        {
            transform.localPosition += new Vector3(0, 1f, 1);
            yield return null;
        }

        Dismiss();
    }

    public override void OnPresentTransition()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, Transition.Appear);
    }

    public override void OnDismissTransition()
    {
        CanvasGroup.DOFade(0, Transition.Disappear);
    }
}
