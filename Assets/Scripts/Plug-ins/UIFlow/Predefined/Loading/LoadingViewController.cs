using UnityEngine;

using TMPro;

using UIFlow;

public class LoadingViewController : ViewController
{
    private static LoadingViewController _instance;

    [SerializeField] private TextMeshProUGUI _message;

    // Methods

    public static LoadingViewController Present(string text = "")
    {
        if (_instance == null)
            _instance = Storyboard.Present(Storyboard.GetCachedViewController<LoadingViewController>());

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

    public static void Release()
    {
        if (_instance == null)
            return;

        _instance.Dismiss();

        _instance = null;
    }
}
