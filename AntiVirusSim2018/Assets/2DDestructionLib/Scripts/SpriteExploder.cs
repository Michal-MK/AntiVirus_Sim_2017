using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public static class SpriteExploder {
	public static List<GameObject> GenerateTriangularPieces(GameObject source, int extraPoints = 0, int subShatterSteps = 0, Material mat = null) {
		List<GameObject> pieces = new List<GameObject>();

		if (mat == null) {
			mat = CreateFragmentMaterial(source);
		}

		//get transform information
		Vector3 origScale = source.transform.localScale;
		source.transform.localScale = Vector3.one;
		Quaternion origRotation = source.transform.localRotation;
		source.transform.localRotation = Quaternion.identity;

		//get rigidbody information
		Vector2 origVelocity = source.GetComponent<Rigidbody2D>().velocity;

		//get collider information
		PolygonCollider2D sourcePolyCollider = source.GetComponent<PolygonCollider2D>();
		BoxCollider2D sourceBoxCollider = source.GetComponent<BoxCollider2D>();
		List<Vector2> points = new List<Vector2>();
		List<Vector2> borderPoints = new List<Vector2>();

		//add points from the present collider
		if (sourcePolyCollider != null) {
			points = GetPoints(sourcePolyCollider);
			borderPoints = GetPoints(sourcePolyCollider);
		}
		else if (sourceBoxCollider != null) {
			points = GetPoints(sourceBoxCollider);
			borderPoints = GetPoints(sourceBoxCollider);
		}

		//create a bounding rectangle based on the polygon points
		Rect rect = GetRect(source);

		//if the target polygon is a triangle, generate a point in the middle to allow for fracture
		if (points.Count == 3) {
			points.Add((points[0] + points[1] + points[2]) / 3);
		}

		for (int i = 0; i < extraPoints; i++) {
			points.Add(new Vector2(Random.Range(rect.width / -2, rect.width / 2), Random.Range(rect.height / -2 + rect.center.y, rect.height / 2 + rect.center.y)));
		}


		Voronoi voronoi = new Delaunay.Voronoi(points, rect);

		List<List<Vector2>> clippedTriangles = new List<List<Vector2>>();
		foreach (Triangle tri in voronoi.Triangles()) {
			clippedTriangles = ClipperHelper.ClipTriangular(borderPoints, tri);
			foreach (List<Vector2> triangle in clippedTriangles) {
				pieces.Add(GenerateTriangularPiece(source, triangle, origVelocity, origScale, origRotation, mat));
			}
		}
		List<GameObject> morePieces = new List<GameObject>();
		if (subShatterSteps > 0) {
			subShatterSteps--;
			foreach (GameObject piece in pieces) {
				morePieces.AddRange(SpriteExploder.GenerateTriangularPieces(piece, extraPoints, subShatterSteps, mat));
				GameObject.DestroyImmediate(piece);
			}
		}
		else {
			morePieces = pieces;
		}

		//reset transform information
		source.transform.localScale = origScale;
		source.transform.localRotation = origRotation;

		Resources.UnloadUnusedAssets();

		return morePieces;
	}

	private static GameObject GenerateTriangularPiece(GameObject source, List<Vector2> tri, Vector2 origVelocity, Vector3 origScale, Quaternion origRotation, Material mat) {
		//Create Game Object and set transform settings properly
		GameObject piece = new GameObject(source.name + " piece");
		piece.transform.position = source.transform.position;
		piece.transform.rotation = source.transform.rotation;
		piece.transform.localScale = source.transform.localScale;

		//Create and Add Mesh Components
		MeshFilter meshFilter = (MeshFilter)piece.AddComponent(typeof(MeshFilter));
		piece.AddComponent(typeof(MeshRenderer));

		Mesh uMesh = piece.GetComponent<MeshFilter>().sharedMesh;
		if (uMesh == null) {
			meshFilter.mesh = new Mesh();
			uMesh = meshFilter.sharedMesh;
		}
		Vector3[] vertices = new Vector3[3];
		int[] triangles = new int[3];

		vertices[0] = new Vector3(tri[0].x, tri[0].y, 0);
		vertices[1] = new Vector3(tri[1].x, tri[1].y, 0);
		vertices[2] = new Vector3(tri[2].x, tri[2].y, 0);

		triangles[0] = 2;
		triangles[1] = 1;
		triangles[2] = 0;

		uMesh.vertices = vertices;
		uMesh.triangles = triangles;
		if (source.GetComponent<SpriteRenderer>() != null) {
			uMesh.uv = CalcUV(vertices, source.GetComponent<SpriteRenderer>(), source.transform);
		}
		else {
			uMesh.uv = CalcUV(vertices, source.GetComponent<MeshRenderer>(), source.transform);
		}

		//set transform properties before fixing the pivot for easier rotation
		piece.transform.localScale = origScale;
		piece.transform.localRotation = origRotation;

		Vector3 diff = CalcPivotCenterDiff(piece);
		CenterMeshPivot(piece, diff);
		uMesh.RecalculateBounds();
		uMesh.RecalculateNormals();

		//setFragmentMaterial(piece, source);
		piece.GetComponent<MeshRenderer>().sharedMaterial = mat;

		//assign mesh
		meshFilter.mesh = uMesh;

		//Create and Add Polygon Collider
		PolygonCollider2D collider = piece.AddComponent<PolygonCollider2D>();
		collider.SetPath(0, new Vector2[] { uMesh.vertices[0], uMesh.vertices[1], uMesh.vertices[2] });

		//Create and Add Rigidbody
		Rigidbody2D rigidbody = piece.AddComponent<Rigidbody2D>();
		rigidbody.velocity = origVelocity;


		return piece;
	}

	public static List<GameObject> GenerateVoronoiPieces(GameObject source, int extraPoints = 0, int subshatterSteps = 0, Material mat = null) {
		List<GameObject> pieces = new List<GameObject>();

		if (mat == null) {
			mat = CreateFragmentMaterial(source);
		}

		//get transform information
		Vector3 origScale = source.transform.localScale;
		source.transform.localScale = Vector3.one;
		Quaternion origRotation = source.transform.localRotation;
		source.transform.localRotation = Quaternion.identity;

		//get rigidbody information
		Vector2 origVelocity = source.GetComponent<Rigidbody2D>().velocity;

		//get collider information
		PolygonCollider2D sourcePolyCollider = source.GetComponent<PolygonCollider2D>();
		BoxCollider2D sourceBoxCollider = source.GetComponent<BoxCollider2D>();
		List<Vector2> points = new List<Vector2>();
		List<Vector2> borderPoints = new List<Vector2>();
		if (sourcePolyCollider != null) {
			points = GetPoints(sourcePolyCollider);
			borderPoints = GetPoints(sourcePolyCollider);
		}
		else if (sourceBoxCollider != null) {
			points = GetPoints(sourceBoxCollider);
			borderPoints = GetPoints(sourceBoxCollider);
		}

		Rect rect = GetRect(source);

		for (int i = 0; i < extraPoints; i++) {
			points.Add(new Vector2(Random.Range(
									   rect.width / -2 + rect.center.x, rect.width / 2 + rect.center.x),
								   Random.Range(rect.height / -2 + rect.center.y, rect.height / 2 + rect.center.y)
					   ));
		}


		Voronoi voronoi = new Voronoi(points, rect);
		List<List<Vector2>> clippedRegions = new List<List<Vector2>>();
		foreach (List<Vector2> region in voronoi.Regions()) {
			clippedRegions = ClipperHelper.ClipVoronoi(borderPoints, region);
			foreach (List<Vector2> clippedRegion in clippedRegions) {
				pieces.Add(GenerateVoronoiPiece(source, clippedRegion, origVelocity, origScale, origRotation, mat));
			}
		}

		List<GameObject> morePieces = new List<GameObject>();
		if (subshatterSteps > 0) {
			subshatterSteps--;
			foreach (GameObject piece in pieces) {
				morePieces.AddRange(SpriteExploder.GenerateVoronoiPieces(piece, extraPoints, subshatterSteps));
				GameObject.DestroyImmediate(piece);
			}
		}
		else {
			morePieces = pieces;
		}

		//reset transform information
		source.transform.localScale = origScale;
		source.transform.localRotation = origRotation;

		Resources.UnloadUnusedAssets();

		return morePieces;
	}

	private static GameObject GenerateVoronoiPiece(GameObject source, List<Vector2> region, Vector2 origVelocity, Vector3 origScale, Quaternion origRotation, Material mat) {
		//Create Game Object and set transform settings properly
		GameObject piece = new GameObject(source.name + " piece");
		piece.transform.position = source.transform.position;
		piece.transform.rotation = source.transform.rotation;
		piece.transform.localScale = source.transform.localScale;

		//Create and Add Mesh Components
		MeshFilter meshFilter = (MeshFilter)piece.AddComponent(typeof(MeshFilter));
		piece.AddComponent(typeof(MeshRenderer));

		Mesh uMesh = piece.GetComponent<MeshFilter>().sharedMesh;
		if (uMesh == null) {
			meshFilter.mesh = new Mesh();
			uMesh = meshFilter.sharedMesh;
		}

		Voronoi voronoi = new Voronoi(region, GetRect(region));

		Vector3[] vertices = CalcVerts(voronoi);
		int[] triangles = CalcTriangles(voronoi);

		uMesh.vertices = vertices;
		uMesh.triangles = triangles;
		if (source.GetComponent<SpriteRenderer>() != null) {
			uMesh.uv = CalcUV(vertices, source.GetComponent<SpriteRenderer>(), source.transform);
		}
		else {
			uMesh.uv = CalcUV(vertices, source.GetComponent<MeshRenderer>(), source.transform);
		}

		//set transform properties before fixing the pivot for easier rotation
		piece.transform.localScale = origScale;
		piece.transform.localRotation = origRotation;

		Vector3 diff = CalcPivotCenterDiff(piece);
		CenterMeshPivot(piece, diff);
		uMesh.RecalculateBounds();
		uMesh.RecalculateNormals();

		//setFragmentMaterial(piece, source);
		piece.GetComponent<MeshRenderer>().sharedMaterial = mat;

		//assign mesh
		meshFilter.mesh = uMesh;

		//Create and Add Polygon Collider
		PolygonCollider2D collider = piece.AddComponent<PolygonCollider2D>();
		collider.SetPath(0, CalcPolyColliderPoints(region, diff));

		//Create and Add Rigidbody
		Rigidbody2D rigidbody = piece.AddComponent<Rigidbody2D>();
		rigidbody.velocity = origVelocity;


		return piece;
	}

	/// <summary>
	/// generates a list of points from a box collider
	/// </summary>
	/// <param name="collider">source box collider</param>
	/// <returns>list of points</returns>
	private static List<Vector2> GetPoints(BoxCollider2D collider) {
		List<Vector2> points = new List<Vector2>();

		Vector2 center = collider.offset;
		Vector2 size = collider.size;

		//bottom left
		points.Add(new Vector2((center.x - size.x / 2), (center.y - size.y / 2)));

		//top left
		points.Add(new Vector2((center.x - size.x / 2), (center.y + size.y / 2)));

		//top right
		points.Add(new Vector2((center.x + size.x / 2), (center.y + size.y / 2)));

		//bottom right
		points.Add(new Vector2((center.x + size.x / 2), (center.y - size.y / 2)));

		return points;
	}

	/// <summary>
	/// generates a list of points from a polygon collider
	/// </summary>
	/// <param name="collider">source polygon collider</param>
	/// <returns>list of points</returns>
	private static List<Vector2> GetPoints(PolygonCollider2D collider) {
		List<Vector2> points = new List<Vector2>();

		foreach (Vector2 point in collider.GetPath(0)) {
			points.Add(point);
		}

		return points;
	}

	/// <summary>
	/// generates a rectangle based on the rendering bounds of the object
	/// </summary>
	/// <param name="source">gameobject to get the rectangle from</param>
	/// <returns>a Rectangle representing the rendering bounds of the object</returns>
	private static Rect GetRect(GameObject source) {
		Bounds bounds = source.GetComponent<Renderer>().bounds;
		Rect rect = new Rect(bounds.extents.x * -1, bounds.extents.y * -1, bounds.size.x, bounds.size.y);
		rect.center = new Vector2(rect.center.x + bounds.center.x - source.transform.position.x, rect.center.y + bounds.center.y - source.transform.position.y);
		return rect;
	}

	private static Rect GetRect(List<Vector2> region) {
		Vector2 center = new Vector2();
		float minX = region[0].x;
		float maxX = minX;
		float minY = region[0].y;
		float maxY = minY;
		foreach (Vector2 v in region) {
			center += v;
			if (v.x < minX) {
				minX = v.x;
			}
			if (v.x > maxX) {
				maxX = v.x;
			}
			if (v.y < minY) {
				minY = v.y;
			}
			if (v.y > maxY) {
				maxY = v.y;
			}
		}
		center /= region.Count;
		Vector2 size = new Vector2(maxX - minX, maxY - minY);
		return new Rect(center, size);
	}

	/// <summary>
	/// calculates the UV coordinates for the given vertices based on the provided Sprite
	/// </summary>
	/// <param name="vertices">vertices to generate the UV coordinates for</param>
	/// <param name="sRend">Sprite Renderer of original object</param>
	/// <param name="sTransform">Transform of the original object</param>
	/// <returns>array of UV coordinates for the mesh</returns>
	private static Vector2[] CalcUV(Vector3[] vertices, SpriteRenderer sRend, Transform sTransform) {
		float texHeight = (sRend.bounds.extents.y * 2) / sTransform.localScale.y;
		float texWidth = (sRend.bounds.extents.x * 2) / sTransform.localScale.x;
		Vector3 botLeft = sTransform.InverseTransformPoint(new Vector3(sRend.bounds.center.x - sRend.bounds.extents.x, sRend.bounds.center.y - sRend.bounds.extents.y, 0));
		Vector2[] uv = new Vector2[vertices.Length];

		Vector2[] sourceUV = sRend.sprite.uv;
		Vector2 uvMin;
		Vector2 uvMax;
		GetUVRange(out uvMin, out uvMax, sourceUV);

		for (int i = 0; i < vertices.Length; i++) {

			float x = (vertices[i].x - botLeft.x) / texWidth;
			x = ScaleRange(x, 0, 1, uvMin.x, uvMax.x);
			float y = (vertices[i].y - botLeft.y) / texHeight;
			y = ScaleRange(y, 0, 1, uvMin.y, uvMax.y);

			uv[i] = new Vector2(x, y);
		}
		return uv;
	}

	private static Vector2[] CalcUV(Vector3[] vertices, MeshRenderer mRend, Transform sTransform) {
		float texHeight = (mRend.bounds.extents.y * 2) / sTransform.localScale.y;
		float texWidth = (mRend.bounds.extents.x * 2) / sTransform.localScale.x;
		Vector3 botLeft = sTransform.InverseTransformPoint(new Vector3(mRend.bounds.center.x - mRend.bounds.extents.x, mRend.bounds.center.y - mRend.bounds.extents.y, 0));
		Vector2[] uv = new Vector2[vertices.Length];

		Vector2[] sourceUV = sTransform.GetComponent<MeshFilter>().sharedMesh.uv;
		Vector2 uvMin;
		Vector2 uvMax;
		GetUVRange(out uvMin, out uvMax, sourceUV);

		for (int i = 0; i < vertices.Length; i++) {
			float x = (vertices[i].x - botLeft.x) / texWidth;
			x = ScaleRange(x, 0, 1, uvMin.x, uvMax.x);
			float y = (vertices[i].y - botLeft.y) / texHeight;
			y = ScaleRange(y, 0, 1, uvMin.y, uvMax.y);

			uv[i] = new Vector2(x, y);
		}
		return uv;
	}

	private static void GetUVRange(out Vector2 min, out Vector2 max, Vector2[] uv) {
		min = uv[0];
		max = uv[0];

		foreach (Vector2 p in uv) {
			if (p.x < min.x) {
				min.x = p.x;
			}
			if (p.x > max.x) {
				max.x = p.x;
			}
			if (p.y < min.y) {
				min.y = p.y;
			}
			if (p.y > max.y) {
				max.y = p.y;
			}
		}
	}

	private static float ScaleRange(float target, float oldMin, float oldMax, float newMin, float newMax) {
		return (target / ((oldMax - oldMin) / (newMax - newMin))) + newMin;
	}

	private static Vector3[] CalcVerts(Voronoi region) {
		List<Site> sites = region.Sites().sites;
		Vector3[] vertices = new Vector3[sites.Count];
		int idx = 0;
		foreach (Site s in sites) {
			vertices[idx++] = new Vector3(s.X, s.Y, 0);
		}
		return vertices;
	}

	private static int[] CalcTriangles(Voronoi region) {
		//calculate unity triangles
		int[] triangles = new int[region.Triangles().Count * 3];

		List<Site> sites = region.Sites().sites;
		int idx = 0;
		foreach (Triangle t in region.Triangles()) {
			triangles[idx++] = sites.IndexOf(t.Sites[0]);
			triangles[idx++] = sites.IndexOf(t.Sites[1]);
			triangles[idx++] = sites.IndexOf(t.Sites[2]);
		}
		return triangles;
	}

	private static Vector2[] CalcPolyColliderPoints(List<Vector2> points, Vector2 offset) {
		Vector2[] result = new Vector2[points.Count];
		for (int i = 0; i < points.Count; i++) {
			result[i] = points[i] + offset;
		}
		return result;
	}

	/// <summary>
	/// calculates the distance between the targets pivot and it's actual center
	/// </summary>
	/// <param name="target">target gameobject to do the calculation on</param>
	/// <returns>distance between center and pivot</returns>
	private static Vector3 CalcPivotCenterDiff(GameObject target) {
		Mesh uMesh = target.GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices = uMesh.vertices;

		Vector3 sum = new Vector3();

		for (int i = 0; i < vertices.Length; i++) {
			sum += vertices[i];
		}
		Vector3 triCenter = sum / vertices.Length;
		Vector3 pivot = target.transform.InverseTransformPoint(target.transform.position);
		return pivot - triCenter;
	}

	/// <summary>
	/// Sets the pivot of the target object to it's center
	/// </summary>
	/// <param name="target">Target Gameobject</param>
	/// <param name="diff">the distance from pivot to center</param>
	private static void CenterMeshPivot(GameObject target, Vector3 diff) {
		//initialize mesh and vertices variables from source
		Mesh uMesh = target.GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices = uMesh.vertices;

		//calculate adjusted vertices
		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] += diff;
		}

		//set adjusted vertices
		uMesh.vertices = vertices;

		//calculate and assign adjusted trasnsform position
		Vector3 pivot = target.transform.InverseTransformPoint(target.transform.position);
		target.transform.localPosition = target.transform.TransformPoint(pivot - diff);

	}

	private static Material CreateFragmentMaterial(GameObject source) {
		SpriteRenderer sRend = source.GetComponent<SpriteRenderer>();
		if (sRend != null) {
			Material mat = new Material(sRend.sharedMaterial);
			mat.SetTexture("_MainTex", sRend.sprite.texture);
			mat.color = sRend.color;
			return mat;
		}
		return source.GetComponent<MeshRenderer>().sharedMaterial;
	}
}