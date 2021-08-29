using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class Explodable : MonoBehaviour {

	public bool runtimeFragment = true;
	public int extraPoints;
	public int subShatterSteps;

	public string fragmentLayer = "Default";
	public string sortingLayerName = "Default";
	public int orderInLayer;

	public enum ShatterType {
		Triangle,
		Voronoi
	}

	public ShatterType shatterType;
	public List<GameObject> fragments = new List<GameObject>();
	private readonly List<List<Vector2>> polygons = new List<List<Vector2>>();

	/// <summary>
	/// Creates fragments if necessary
	/// </summary>
	public void Explode() {
		if (fragments.Count == 0 && runtimeFragment) {
			GenerateFragments();
		}

		foreach (GameObject frag in fragments) {
			frag.transform.parent = null;
			frag.SetActive(true);
		}
	}

	/// <summary>
	/// Creates fragments and then disables them
	/// </summary>
	public void FragmentInEditor() {
		if (fragments.Count > 0) {
			DeleteFragments();
		}
		GenerateFragments();
		SetPolygonsForDrawing();
		foreach (GameObject frag in fragments) {
			frag.transform.parent = transform;
			frag.SetActive(false);
		}
	}

	public void DeleteFragments() {
		foreach (GameObject frag in fragments) {
			if (Application.isEditor) {
				DestroyImmediate(frag);
			}
			else {
				Destroy(frag);
			}
		}
		fragments.Clear();
		polygons.Clear();
	}

	/// <summary>
	/// Turns GameObject into multiple fragments
	/// </summary>
	private void GenerateFragments() {
		fragments = shatterType switch {
			ShatterType.Triangle => SpriteExploder.GenerateTriangularPieces(gameObject, extraPoints, subShatterSteps),
			ShatterType.Voronoi  => SpriteExploder.GenerateVoronoiPieces(gameObject, extraPoints, subShatterSteps),
			_                    => throw new ArgumentException(nameof(shatterType) + " is Invalid!")
		};

		//sets additional aspects of the fragments
		foreach (GameObject p in fragments.Where(s => s != null)) {
			p.layer = LayerMask.NameToLayer(fragmentLayer);
			p.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
			p.GetComponent<Renderer>().sortingOrder = orderInLayer;
		}
	}

	private void SetPolygonsForDrawing() {
		polygons.Clear();

		foreach (GameObject frag in fragments) {
			List<Vector2> polygon = new List<Vector2>();
			foreach (Vector2 point in frag.GetComponent<PolygonCollider2D>().points) {
				Vector2 offset = RotateAroundPivot(frag.transform.position, transform.position, Quaternion.Inverse(transform.rotation)) - (Vector2)transform.position;
				offset.x /= transform.localScale.x;
				offset.y /= transform.localScale.y;
				polygon.Add(point + offset);
			}
			polygons.Add(polygon);
		}
	}

	private Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, Quaternion angle) {
		Vector2 dir = point - pivot;
		dir = angle * dir;
		point = dir + pivot;
		return point;
	}

	private void OnDrawGizmosSelected() {
		if (!Application.isEditor) return;
		
		if (polygons.Count == 0 && fragments.Count != 0) {
			SetPolygonsForDrawing();
		}

		Gizmos.color = Color.blue;
		Gizmos.matrix = transform.localToWorldMatrix;
		Vector2 offset = (Vector2)transform.position * 0;
		foreach (List<Vector2> polygon in polygons) {
			for (int i = 0; i < polygon.Count; i++) {
				if (i + 1 == polygon.Count) {
					Gizmos.DrawLine(polygon[i] + offset, polygon[0] + offset);
				}
				else {
					Gizmos.DrawLine(polygon[i] + offset, polygon[i + 1] + offset);
				}
			}
		}
	}
}