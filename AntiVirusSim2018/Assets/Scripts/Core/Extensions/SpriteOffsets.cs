using UnityEngine;

public static class SpriteOffsets {
	/// <summary>
	/// Return a position in world space based on sprites position and percentual offset from top left
	/// </summary>
	/// <param name="render">The renderer that contains the sprite to measure</param>
	/// <param name="percentageX">Value between 0-100</param>
	/// <param name="percentageY">Value between 0-100</param>
	/// <returns></returns>
	public static Vector3 GetPoint(SpriteRenderer render, int percentageX, int percentageY) {
		Bounds b = render.sprite.bounds;
		Vector3 topLeft = new Vector3(render.transform.position.x - (b.extents.x * render.transform.localScale.x), render.transform.position.y + (b.extents.y * render.transform.localScale.y));
		return new Vector3(
			ValueMapping.MapFloat(percentageX, 0, 100, topLeft.x, topLeft.x + (b.size.x * render.transform.localScale.x)),
			ValueMapping.MapFloat(percentageY, 0, 100, topLeft.y, topLeft.y - (b.size.y * render.transform.localScale.y)));
	}
}

