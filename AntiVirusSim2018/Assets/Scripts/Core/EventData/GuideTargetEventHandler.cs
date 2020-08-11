using UnityEngine;

/// <summary>
/// Dynamic guiding towards an object's <see cref="Transform"/>
/// </summary>
public delegate void GuideTargetDynamicEventHandler(Transform target);

/// <summary>
/// Static guiding towards a set <see cref="Vector3"/>
/// </summary>
public delegate void GuideTargetStaticEventHandler(Vector3 target);
