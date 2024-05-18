using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using UIFlow;
using UIFlow.Predefined.KebabMenu;

public class KebabMenuViewController : ViewController
{
    [SerializeField] private RectTransform _menu;
    [SerializeField] private ItemView _itemViewPrefab;

    // Methods

    public static void Present(GameObject kebabSource, params KebabItemData[] data)
    {
        Storyboard.Present<KebabMenuViewController>().PresentImpl(kebabSource, data);
    }

    private void PresentImpl(GameObject kebabSource, params KebabItemData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            ItemView view = Instantiate(_itemViewPrefab, _menu);
            view.Initialize(data[i]);
            data[i].Callback.AddListener(() =>
            {
                Dismiss();
            });
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_menu);
        AlignAround(kebabSource);

        Vector3 pos = _menu.position;
        _menu.position = kebabSource.transform.position;
        _menu.DOMove(pos, Transition.Appear);
    }

    private void AlignAround(GameObject kebabSource)
    {
        RectTransform kebabRect = kebabSource.GetComponent<RectTransform>();
        _menu.pivot = Vector2.one / 2f;
        _menu.anchorMin = Vector2.one / 2f;
        _menu.anchorMax = Vector2.one / 2f;
        _menu.position = kebabRect.position;
        _menu.anchoredPosition -= new Vector2(_menu.rect.width, kebabRect.rect.height + _menu.rect.height) / 2f;

        Canvas rootCanvas = GetComponentInParent<Canvas>();

        Vector3[] corners = new Vector3[4];
        _menu.GetWorldCorners(corners);

        RectTransform rootCanvasRectTransform = rootCanvas.transform as RectTransform;
        Rect rootCanvasRect = rootCanvasRectTransform.rect;

        if(FlipLayoutOn(0, corners, rootCanvasRectTransform, rootCanvasRect))
            _menu.anchoredPosition += new Vector2(_menu.rect.width, 0) / 2f + new Vector2(kebabRect.rect.width, 0);
        
        if(FlipLayoutOn(1, corners, rootCanvasRectTransform, rootCanvasRect))
            _menu.anchoredPosition += new Vector2(0, _menu.rect.height) / 2f + new Vector2(0, kebabRect.rect.height);
    }

    private bool FlipLayoutOn(int axis, Vector3[] corners, RectTransform rootCanvasRectTransform, Rect rootCanvasRect)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
            if ((corner[axis] < rootCanvasRect.min[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.min[axis])) ||
                (corner[axis] > rootCanvasRect.max[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.max[axis])))
            {
                return true;
            }
        }
        return false;
    }

    public override void OnPresentTransition()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, Transition.Appear);
    }

    public override void OnDismissTransition()
    {
        CanvasGroup.DOFade(0, Transition.Disappear);
        _menu.DOScale(0, Transition.Disappear);
    }
}
