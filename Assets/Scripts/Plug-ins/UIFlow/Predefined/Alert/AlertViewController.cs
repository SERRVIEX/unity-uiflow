using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using TMPro;
using DG.Tweening;

using UIFlow;
using UIFlow.Predefined.Alert;

public class AlertViewController : ViewController
{
    [SerializeField] private TextMeshProUGUI _message;

    [SerializeField, Header("Buttons")] private float _buttonHeight = 85f;
    [SerializeField] private RectTransform _buttonsContent;
    [SerializeField] private AlertButtonViewBase[] _buttonPrefabs;
    private Dictionary<AlertButtonBase, AlertButtonViewBase> _buttons = new Dictionary<AlertButtonBase, AlertButtonViewBase>();

    private Dictionary<Type, Type> _distributed = new Dictionary<Type, Type>();

    // Methods

    protected void Awake()
    {
        _distributed.Add(typeof(AlertLabelButtonData), typeof(AlertLabelButtonView));
        _distributed.Add(typeof(AlertIconButtonData), typeof(AlertIconButtonView));
        _distributed.Add(typeof(AlertDefaultButtonData), typeof(AlertDefaultButtonView));
        _distributed.Add(typeof(ConstraintedActionData), typeof(AlertConstraintedButtonView));
        _distributed.Add(typeof(AlertComplexButtonData), typeof(AlertComplexButtonView));
    }

    public static AlertViewController Present()
    {
        AlertViewController alertViewController = Storyboard.Present<AlertViewController>();
        return alertViewController;
    }

    public void UpdateContent(string text, bool alignHorizontal, params AlertButtonBase[] actions)
    {
        _message.text = text;

        DestroyImmediate(_buttonsContent.GetComponent<LayoutGroup>());

        if (alignHorizontal)
        {
            ContentSizeFitter contentSizeFitter = _buttonsContent.GetComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit =  ContentSizeFitter.FitMode.Unconstrained;

            HorizontalLayoutGroup layoutGroup = _buttonsContent.gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 10;

            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;

            layoutGroup.childScaleWidth = true;
            layoutGroup.childScaleHeight = true;

            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;

            _buttonsContent.SetHeight(_buttonHeight);
        }
        else
        {
            ContentSizeFitter contentSizeFitter = _buttonsContent.GetComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layoutGroup = _buttonsContent.gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 10;
            
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;

            layoutGroup.childScaleWidth = true;
            layoutGroup.childScaleHeight = false;

            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = false;
        }

        if (actions.Length > 0)
            for (int i = 0; i < actions.Length; i++)
                CreateButton(actions[i]);
        else
            CreateButton(
                new AlertLabelButtonData()
                    .SetLabel("OK")
                    .SetOnClick(() => Dismiss()));

        LayoutRebuilder.ForceRebuildLayoutImmediate(_message.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonsContent);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
    }

    private void CreateButton(AlertButtonBase data)
    {
        AlertButtonViewBase view = null;

        Type viewType = _distributed[data.GetType()];

        for (int i = 0; i < _buttonPrefabs.Length; i++)
        {
            if (_buttonPrefabs[i].GetType() == viewType)
            {
                view = Instantiate(_buttonPrefabs[i], _buttonsContent);
                view.RectTransform.SetHeight(_buttonHeight);
                _buttons.Add(data, view);
            }
        }

        Assert.IsFalse(view == null);

        view.SetData(data);
    }

    public AlertButtonViewBase GetButtonPrefab(AlertButtonBase data)
    {
        if(_buttons.TryGetValue(data, out AlertButtonViewBase view))
            return view;

        return null;
    }

    public override void OnPresentTransition()
    {
        Content.DOPunchScale(Vector3.one * .1f, Transition.Appear);
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, Transition.Appear);
    }

    public override void OnDismissTransition()
    {
        Content.DOAnchorPosY(-RectTransform.rect.height / 2f, Transition.Disappear);
        CanvasGroup.DOFade(0, Transition.Disappear);
    }
}
