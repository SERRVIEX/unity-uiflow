using System.Collections;

using UnityEngine;

using TMPro;

using UIFlow;

public class ToastViewController : ViewController
{
    private static ToastViewController _instance;

    private string _id;

    [SerializeField] private TextMeshProUGUI _message;

    // Methods

    public static ToastViewController Present(string id, string message, float duration = 2)
    {
        // Avoid spamming toasts with the same id.
        if (_instance != null)
            if(id == _instance._id)
                return _instance;

        _instance = Storyboard.Present(Storyboard.GetCachedViewController<ToastViewController>());
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
        yield return new WaitForSeconds(duration);
        Dismiss();
    }
}
