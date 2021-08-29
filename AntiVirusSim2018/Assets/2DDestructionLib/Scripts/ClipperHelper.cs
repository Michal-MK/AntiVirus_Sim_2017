using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Delaunay;

public static class ClipperHelper {
	private const float MULTIPLIER = 1000;

	public static List<List<Vector2>> ClipTriangular(List<Vector2> boundary, Triangle piece) {
		List<List<IntPoint>> boundaryPoly = CreatePolygons(boundary);
		List<List<IntPoint>> subjPoly = CreatePolygons(piece);

		//clip triangular polygon against the boundary polygon
		List<List<IntPoint>> result = new List<List<IntPoint>>();
		Clipper c = new Clipper();
		c.AddPaths(subjPoly, PolyType.ptClip, true);
		c.AddPaths(boundaryPoly, PolyType.ptSubject, true);
		c.Execute(ClipType.ctIntersection, result, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

		List<List<Vector2>> clippedPolygons = new List<List<Vector2>>();

		foreach (List<IntPoint> poly in result) {
			clippedPolygons.Add(poly.Select(p => new Vector2(p.X, p.Y) / MULTIPLIER).ToList());
		}
		return clippedPolygons;
	}

	public static List<List<Vector2>> ClipVoronoi(List<Vector2> boundary, List<Vector2> region) {
		List<List<IntPoint>> boundaryPoly = CreatePolygons(boundary);
		List<List<IntPoint>> regionPoly = CreatePolygons(region);

		List<List<IntPoint>> result = new List<List<IntPoint>>();
		Clipper c = new Clipper();
		c.AddPaths(regionPoly, PolyType.ptClip, true);
		c.AddPaths(boundaryPoly, PolyType.ptSubject, true);
		c.Execute(ClipType.ctIntersection, result, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

		List<List<Vector2>> clippedPolygons = new List<List<Vector2>>();

		foreach (List<IntPoint> poly in result) {
			clippedPolygons.Add(poly.Select(p => new Vector2(p.X, p.Y) / MULTIPLIER).ToList());
		}
		return clippedPolygons;
	}

	private static List<List<IntPoint>> CreatePolygons(List<Vector2> source) {
		List<IntPoint> pol = source.Select(p => new IntPoint(p.x * MULTIPLIER, p.y * MULTIPLIER)).ToList();
		return new List<List<IntPoint>>(1) { pol };
	}

	private static List<List<IntPoint>> CreatePolygons(Triangle tri) {
		List<IntPoint> pol = new List<IntPoint>();
		for (int i = 0; i < 3; i++) {
			pol.Add(new IntPoint(tri.Sites[i].X * MULTIPLIER, tri.Sites[i].Y * MULTIPLIER));
		}
		return new List<List<IntPoint>>(1) { pol };
	}
}