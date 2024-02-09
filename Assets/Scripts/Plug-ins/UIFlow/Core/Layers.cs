namespace UIFlow.Core
{
    using System;

    using UnityEngine;

    [Serializable]
    public struct Layers
    {
        [SerializeField, Immutable] public Canvas Under;
        [SerializeField, Immutable] public Canvas Base;
        [SerializeField, Immutable] public Canvas Extra;
        [SerializeField, Immutable] public Canvas Context;
        [SerializeField, Immutable] public Canvas Alert;
        [SerializeField, Immutable] public Canvas Over;

        // Methods

        public Canvas GetCanvas(Layer layer)
        {
            return layer switch
            {
                Layer.Under => Under,
                Layer.Base => Base,
                Layer.Extra => Extra,
                Layer.Context => Context,
                Layer.Alert => Alert,
                Layer.Over => Over,
                _ => Under,
            };
        }

        public void UpdateProperties(HideFlags hideFlags)
        {
            if (Under != null)
            {
                Under.overrideSorting = true;
                Under.sortingOrder = 0;
                Under.gameObject.hideFlags = hideFlags;
            }

            if (Base != null)
            {
                Base.overrideSorting = true;
                Base.sortingOrder = 20;
                Base.gameObject.hideFlags = hideFlags;
            }

            if (Extra != null)
            {
                Extra.overrideSorting = true;
                Extra.sortingOrder = 40;
                Extra.gameObject.hideFlags = hideFlags;
            }

            if (Context != null)
            {
                Context.overrideSorting = true;
                Context.sortingOrder = 60;
                Context.gameObject.hideFlags = hideFlags;
            }

            if (Alert != null)
            {
                Alert.overrideSorting = true;
                Alert.sortingOrder = 80;
                Alert.gameObject.hideFlags = hideFlags;
            }

            if (Over != null)
            {
                Over.overrideSorting = true;
                Over.sortingOrder = 100;
                Over.gameObject.hideFlags = hideFlags;
            }
        }
    }
}