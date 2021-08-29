using UnityEngine;
using System.Collections.Generic;

namespace Delaunay {
	/**
		 * The line segment connecting the two Sites is part of the Delaunay triangulation;
		 * the line segment connecting the two Vertices is part of the Voronoi diagram
		 * @author ashaw
		 * 
		 */
	public sealed class Edge {
		private static readonly Stack<Edge> POOL = new Stack<Edge>();

		public static Edge CreateBisectingEdge(Site site0, Site site1) {
			float dx, dy, absdx, absdy;
			float a, b, c;

			dx = site1.X - site0.X;
			dy = site1.Y - site0.Y;
			absdx = dx > 0 ? dx : -dx;
			absdy = dy > 0 ? dy : -dy;
			c = site0.X * dx + site0.Y * dy + (dx * dx + dy * dy) * 0.5f;
			if (absdx > absdy) {
				a = 1.0f;
				b = dy / dx;
				c /= dx;
			}
			else {
				b = 1.0f;
				a = dx / dy;
				c /= dy;
			}

			Edge edge = Create();

			edge.LeftSite = site0;
			edge.RightSite = site1;
			site0.AddEdge(edge);
			site1.AddEdge(edge);

			edge.LeftVertex = null;
			edge.RightVertex = null;

			edge.a = a;
			edge.b = b;
			edge.c = c;

			return edge;
		}

		private static Edge Create() {
			Edge edge;
			if (POOL.Count > 0) {
				edge = POOL.Pop();
				edge.Init();
			}
			else {
				edge = new Edge();
			}
			return edge;
		}

		private static int nedges = 0;

		public static readonly Edge DELETED = new Edge();

		// the equation of the edge: ax + by = c
		public float a, b, c;

		// the two Voronoi vertices that the edge connects
		//		(if one of them is null, the edge extends to infinity)

		public Vertex LeftVertex { get; private set; }

		public Vertex RightVertex { get; private set; }

		public void SetVertex(Side leftRight, Vertex v) {
			if (leftRight == Side.Left) {
				LeftVertex = v;
			}
			else {
				RightVertex = v;
			}
		}

		// Once clipVertices() is called, this Dictionary will hold two Points
		// representing the clipped coordinates of the left and right ends...

		public Dictionary<Side, Vector2?> ClippedEnds { get; private set; }

		// unless the entire Edge is outside the bounds.
		// In that case visible will be false:
		public bool Visible => ClippedEnds != null;

		// the two input Sites for which this Edge is a bisector:
		private Dictionary<Side, Site> sites;

		public Site LeftSite {
			get => sites[Side.Left];
			private set => sites[Side.Left] = value;

		}

		public Site RightSite {
			get => sites[Side.Right];
			private set => sites[Side.Right] = value;
		}

		public Site Site(Side leftRight) {
			return sites[leftRight];
		}

		private readonly int edgeIndex;

		public void Dispose() {
			LeftVertex = null;
			RightVertex = null;
			if (ClippedEnds != null) {
				ClippedEnds[Side.Left] = null;
				ClippedEnds[Side.Right] = null;
				ClippedEnds = null;
			}
			sites[Side.Left] = null;
			sites[Side.Right] = null;
			sites = null;

			POOL.Push(this);
		}

		private Edge() {
			edgeIndex = nedges++;
			Init();
		}

		private void Init() {
			sites = new Dictionary<Side, Site>();
		}

		public override string ToString() {
			return "Edge " + edgeIndex + "; sites " + sites[Side.Left] + ", " + sites[Side.Right]
				   + "; endVertices " + ((LeftVertex != null) ? LeftVertex.VertexIndex.ToString() : "null") + ", "
				   + ((RightVertex != null) ? RightVertex.VertexIndex.ToString() : "null") + "::";
		}

		/**
			 * Set _clippedVertices to contain the two ends of the portion of the Voronoi edge that is visible
			 * within the bounds.  If no part of the Edge falls within the bounds, leave _clippedVertices null. 
			 * @param bounds
			 * 
			 */
		public void ClipVertices(Rect bounds) {
			float xmin = bounds.xMin;
			float ymin = bounds.yMin;
			float xmax = bounds.xMax;
			float ymax = bounds.yMax;

			Vertex vertex0, vertex1;
			float x0, x1, y0, y1;

			if (a == 1.0 && b >= 0.0) {
				vertex0 = RightVertex;
				vertex1 = LeftVertex;
			}
			else {
				vertex0 = LeftVertex;
				vertex1 = RightVertex;
			}

			if (a == 1.0) {
				y0 = ymin;
				if (vertex0 != null && vertex0.Y > ymin) {
					y0 = vertex0.Y;
				}
				if (y0 > ymax) {
					return;
				}
				x0 = c - b * y0;

				y1 = ymax;
				if (vertex1 != null && vertex1.Y < ymax) {
					y1 = vertex1.Y;
				}
				if (y1 < ymin) {
					return;
				}
				x1 = c - b * y1;

				if ((x0 > xmax && x1 > xmax) || (x0 < xmin && x1 < xmin)) {
					return;
				}

				if (x0 > xmax) {
					x0 = xmax;
					y0 = (c - x0) / b;
				}
				else if (x0 < xmin) {
					x0 = xmin;
					y0 = (c - x0) / b;
				}

				if (x1 > xmax) {
					x1 = xmax;
					y1 = (c - x1) / b;
				}
				else if (x1 < xmin) {
					x1 = xmin;
					y1 = (c - x1) / b;
				}
			}
			else {
				x0 = xmin;
				if (vertex0 != null && vertex0.X > xmin) {
					x0 = vertex0.X;
				}
				if (x0 > xmax) {
					return;
				}
				y0 = c - a * x0;

				x1 = xmax;
				if (vertex1 != null && vertex1.X < xmax) {
					x1 = vertex1.X;
				}
				if (x1 < xmin) {
					return;
				}
				y1 = c - a * x1;

				if ((y0 > ymax && y1 > ymax) || (y0 < ymin && y1 < ymin)) {
					return;
				}

				if (y0 > ymax) {
					y0 = ymax;
					x0 = (c - y0) / a;
				}
				else if (y0 < ymin) {
					y0 = ymin;
					x0 = (c - y0) / a;
				}

				if (y1 > ymax) {
					y1 = ymax;
					x1 = (c - y1) / a;
				}
				else if (y1 < ymin) {
					y1 = ymin;
					x1 = (c - y1) / a;
				}
			}

			ClippedEnds = new Dictionary<Side, Vector2?>();
			if (vertex0 == LeftVertex) {
				ClippedEnds[Side.Left] = new Vector2(x0, y0);
				ClippedEnds[Side.Right] = new Vector2(x1, y1);
			}
			else {
				ClippedEnds[Side.Right] = new Vector2(x0, y0);
				ClippedEnds[Side.Left] = new Vector2(x1, y1);
			}
		}
	}
}