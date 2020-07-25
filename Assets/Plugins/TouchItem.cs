using UnityEngine;
using System.Collections;

/// <summary>
/// Structure used by TouchManager
/// </summary>
public struct TouchItem
{
    public Vector2 position;
	public Vector2 deltaPosition;
	public TouchPhaseEnum phase;
}
