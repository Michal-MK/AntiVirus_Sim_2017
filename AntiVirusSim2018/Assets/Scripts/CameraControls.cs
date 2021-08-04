using UnityEngine;

public class CameraControls : MonoBehaviour {
	[SerializeField]
	private Zoom zoom = null;
	/// <summary>
	/// Zooming behaviour of this <see cref="Camera"/>
	/// </summary>
	public Zoom Zoom => zoom;

	[SerializeField]
	private CameraMovement camMovement;
	/// <summary>
	/// Movement behaviour of this <see cref="Camera"/>
	/// </summary>
	public CameraMovement CamMovement => camMovement;
}
