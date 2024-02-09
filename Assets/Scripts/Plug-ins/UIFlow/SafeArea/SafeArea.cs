using System.Collections.Generic;

using UnityEngine;

public static class SafeArea
{
    /// <summary>
    /// Rect transform's which need to update.
    /// </summary>
    private static List<SafeAreaBase> _notches = new List<SafeAreaBase>();

    /// <summary>
    /// Additional offsets to safe area such as ad banner or other layouts.
    /// </summary>
    private static Dictionary<string, RectOffset> _offsets = new Dictionary<string, RectOffset>();

    // Methods

    /// <summary>
    /// Subscribe notch behaviour to recieve offset updates.
    /// </summary>
    public static void Subscribe(SafeAreaBase safeAreaBase)
    {
        if (!_notches.Contains(safeAreaBase))
            _notches.Add(safeAreaBase);
    }

    public static void Unsubscribe(SafeAreaBase safeAreaBase)
    {
        _notches.Remove(safeAreaBase);
    }

    /// <summary>
    /// Add additional offset to safe area.
    /// </summary>
    /// <param name="uniqueId">Ex: ad_banner</param>
    /// <param name="offset">Ex: new RectOffset(left: 0, right: 0, top: 0, bottom: bannerHeight)</param>
    public static void AddOffset(string uniqueId, RectOffset offset)
    {
        if (!_offsets.ContainsKey(uniqueId))
            _offsets.Add(uniqueId, offset);

        Apply();
    }

    public static void RemoveOffset(string uniqueId)
    {
        _offsets.Remove(uniqueId);
        Apply();
    }

    public static RectOffset GetOffset()
    {
        RectOffset offset = new RectOffset();

        foreach (var item in _offsets)
        {
            offset.left += item.Value.left;
            offset.right += item.Value.right;
            offset.top += item.Value.top;
            offset.bottom += item.Value.bottom;
        }

        return offset;
    }

    private static void Apply()
    {
        for (int i = _notches.Count - 1; i >= 0; i--)
        {
            if (!_notches[i])
            {
                _notches.RemoveAt(i);
                continue;
            }

            _notches[i].Apply();
        }
    }
}