using UnityEngine;
using System;
using System.Collections.Generic;
using Delaunay.Geo;

namespace Delaunay {

	public sealed class Site : ICoord, IComparable {
		private static readonly Stack<Site> POOL = new Stack<Site>();

		public static Site Create(Vector2 p, uint index) {
			if (POOL.Count > 0) {
				return POOL.Pop().Init(p, index);
			}
			return new Site(p, index);
		}

		internal static void SortSites(List<Site> sites) {
			sites.Sort(); // XXX: Check if this works
		}

		/**
		 * sort sites on y, then x, coord
		 * also change each site's _siteIndex to match its new position in the list
		 * so the _siteIndex can be used to identify the site for nearest-neighbor queries
		 * 
		 * haha "also" - means more than one responsibility...
		 * 
		 */
		public int CompareTo(System.Object obj) // XXX: Really, really worried about this because it depends on how sorting works in AS3 impl - Julian
		{
			Site s2 = (Site)obj;

			int returnValue = Voronoi.CompareByYThenX(this, s2);

			// swap _siteIndex values if necessary to match new ordering:
			uint tempIndex;
			if (returnValue == -1) {
				if (siteIndex > s2.siteIndex) {
					tempIndex = siteIndex;
					siteIndex = s2.siteIndex;
					s2.siteIndex = tempIndex;
				}
			}
			else if (returnValue == 1) {
				if (s2.siteIndex > siteIndex) {
					tempIndex = s2.siteIndex;
					s2.siteIndex = siteIndex;
					siteIndex = tempIndex;
				}

			}
			return returnValue;
		}

		private const float EPSILON = 0.005f;

		private static bool CloseEnough(Vector2 p0, Vector2 p1) {
			return Vector2.Distance(p0, p1) < EPSILON;
		}

		private Vector2 coord;

		public Vector2 Coord => coord;

		private uint siteIndex;

		private List<Edge> edges;

		private List<Side> edgeOrientations;

		// ordered list of points that define the region clipped to bounds:
		private List<Vector2> region;

		private Site(Vector2 p, uint index) {
			Init(p, index);
		}

		private Site Init(Vector2 p, uint index) {
			coord = p;
			siteIndex = index;
			edges = new List<Edge>();
			region = null;
			return this;
		}

		public override string ToString() {
			return "Site " + siteIndex + ": " + Coord;
		}

		public void Dispose() {
			Clear();
			POOL.Push(this);
		}

		private void Clear() {
			if (edges != null) {
				edges.Clear();
				edges = null;
			}
			if (edgeOrientations != null) {
				edgeOrientations.Clear();
				edgeOrientations = null;
			}
			if (region != null) {
				region.Clear();
				region = null;
			}
		}

		public void AddEdge(Edge edge) {
			edges.Add(edge);
		}

		internal List<Vector2> Region(Rect clippingBounds) {
			if (edges == null || edges.Count == 0) {
				return new List<Vector2>();
			}
			if (edgeOrientations == null) {
				ReorderEdges();
				region = ClipToBounds(clippingBounds);
				if ((new Polygon(region)).Winding() == Winding.Clockwise) {
					region.Reverse();
				}
			}
			return region;
		}

		private void ReorderEdges() {
			EdgeReorderer reorderer = new EdgeReorderer(edges, VertexOrSite.VERTEX);
			edges = reorderer.Edges;
			edgeOrientations = reorderer.EdgeOrientations;
			reorderer.Dispose();
		}

		private List<Vector2> ClipToBounds(Rect bounds) {
			List<Vector2> points = new List<Vector2>();
			int n = edges.Count;
			int i = 0;
			Edge edge;
			while (i < n && (edges[i].Visible == false)) {
				++i;
			}

			if (i == n) {
				// no edges visible
				return new List<Vector2>();
			}
			edge = edges[i];
			Side orientation = edgeOrientations[i];

			if (edge.ClippedEnds[orientation] == null) {
				Debug.LogError("XXX: Null detected when there should be a Vector2!");
			}
			if (edge.ClippedEnds[SideHelper.Other(orientation)] == null) {
				Debug.LogError("XXX: Null detected when there should be a Vector2!");
			}
			points.Add((Vector2)edge.ClippedEnds[orientation]);
			points.Add((Vector2)edge.ClippedEnds[SideHelper.Other(orientation)]);

			for (int j = i + 1; j < n; ++j) {
				edge = edges[j];
				if (edge.Visible == false) {
					continue;
				}
				Connect(points, j, bounds);
			}

			// close up the polygon by adding another corner point of the bounds if needed:
			Connect(points, i, bounds, true);

			return points;
		}

		private void Connect(List<Vector2> points, int j, Rect bounds, bool closingUp = false) {
			Vector2 rightPoint = points[points.Count - 1];
			Edge newEdge = edges[j] as Edge;
			Side newOrientation = edgeOrientations[j];

			// the point that  must be connected to rightPoint:
			if (newEdge.ClippedEnds[newOrientation] == null) {
				Debug.LogError("XXX: Null detected when there should be a Vector2!");
			}
			Vector2 newPoint = (Vector2)newEdge.ClippedEnds[newOrientation];
			if (!CloseEnough(rightPoint, newPoint)) {
				// The points do not coincide, so they must have been clipped at the bounds;
				// see if they are on the same border of the bounds:
				if (rightPoint.x != newPoint.x
					&& rightPoint.y != newPoint.y) {
					// They are on different borders of the bounds;
					// insert one or two corners of bounds as needed to hook them up:
					// (NOTE this will not be correct if the region should take up more than
					// half of the bounds rect, for then we will have gone the wrong way
					// around the bounds and included the smaller part rather than the larger)
					int rightCheck = BoundsCheck.Check(rightPoint, bounds);
					int newCheck = BoundsCheck.Check(newPoint, bounds);
					float px, py;
					if ((rightCheck & BoundsCheck.RIGHT) != 0) {
						px = bounds.xMax;
						if ((newCheck & BoundsCheck.BOTTOM) != 0) {
							py = bounds.yMax;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.TOP) != 0) {
							py = bounds.yMin;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.LEFT) != 0) {
							if (rightPoint.y - bounds.y + newPoint.y - bounds.y < bounds.height) {
								py = bounds.yMin;
							}
							else {
								py = bounds.yMax;
							}
							points.Add(new Vector2(px, py));
							points.Add(new Vector2(bounds.xMin, py));
						}
					}
					else if ((rightCheck & BoundsCheck.LEFT) != 0) {
						px = bounds.xMin;
						if ((newCheck & BoundsCheck.BOTTOM) != 0) {
							py = bounds.yMax;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.TOP) != 0) {
							py = bounds.yMin;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.RIGHT) != 0) {
							if (rightPoint.y - bounds.y + newPoint.y - bounds.y < bounds.height) {
								py = bounds.yMin;
							}
							else {
								py = bounds.yMax;
							}
							points.Add(new Vector2(px, py));
							points.Add(new Vector2(bounds.xMax, py));
						}
					}
					else if ((rightCheck & BoundsCheck.TOP) != 0) {
						py = bounds.yMin;
						if ((newCheck & BoundsCheck.RIGHT) != 0) {
							px = bounds.xMax;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.LEFT) != 0) {
							px = bounds.xMin;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.BOTTOM) != 0) {
							if (rightPoint.x - bounds.x + newPoint.x - bounds.x < bounds.width) {
								px = bounds.xMin;
							}
							else {
								px = bounds.xMax;
							}
							points.Add(new Vector2(px, py));
							points.Add(new Vector2(px, bounds.yMax));
						}
					}
					else if ((rightCheck & BoundsCheck.BOTTOM) != 0) {
						py = bounds.yMax;
						if ((newCheck & BoundsCheck.RIGHT) != 0) {
							px = bounds.xMax;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.LEFT) != 0) {
							px = bounds.xMin;
							points.Add(new Vector2(px, py));
						}
						else if ((newCheck & BoundsCheck.TOP) != 0) {
							if (rightPoint.x - bounds.x + newPoint.x - bounds.x < bounds.width) {
								px = bounds.xMin;
							}
							else {
								px = bounds.xMax;
							}
							points.Add(new Vector2(px, py));
							points.Add(new Vector2(px, bounds.yMin));
						}
					}
				}
				if (closingUp) {
					// newEdge's ends have already been added
					return;
				}
				points.Add(newPoint);
			}
			if (newEdge.ClippedEnds[SideHelper.Other(newOrientation)] == null) {
				Debug.LogError("XXX: Null detected when there should be a Vector2!");
			}
			Vector2 newRightPoint = (Vector2)newEdge.ClippedEnds[SideHelper.Other(newOrientation)];
			if (!CloseEnough(points[0], newRightPoint)) {
				points.Add(newRightPoint);
			}
		}

		public float X => coord.x;

		internal float Y => coord.y;

		public float Dist(ICoord p) {
			return Vector2.Distance(p.Coord, coord);
		}

	}
}

internal static class BoundsCheck {
	public const int TOP = 1;
	public const int BOTTOM = 2;
	public const int LEFT = 4;
	public const int RIGHT = 8;

	/**
		 * 
		 * @param point
		 * @param bounds
		 * @return an int with the appropriate bits set if the Point lies on the corresponding bounds lines
		 * 
		 */
	public static int Check(Vector2 point, Rect bounds) {
		int value = 0;
		if (point.x == bounds.xMin) {
			value |= LEFT;
		}
		if (point.x == bounds.xMax) {
			value |= RIGHT;
		}
		if (point.y == bounds.yMin) {
			value |= TOP;
		}
		if (point.y == bounds.yMax) {
			value |= BOTTOM;
		}
		return value;
	}
}