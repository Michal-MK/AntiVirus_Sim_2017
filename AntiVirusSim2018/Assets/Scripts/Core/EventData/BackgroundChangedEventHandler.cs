using UnityEngine;

/// <summary>
/// Delegate for handling <see cref="Player"/>'s transition from background to background
/// </summary>
public delegate void BackgroundChangedEventHandler(Player sender, RectTransform current, RectTransform previous);
