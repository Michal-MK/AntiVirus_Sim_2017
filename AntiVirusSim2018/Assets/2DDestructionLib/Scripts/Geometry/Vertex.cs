using UnityEngine;
using System.Collections.Generic;

namespace Delaunay {

	public sealed class Vertex : ICoord {
		public static readonly Vertex VERTEX_AT_INFINITY = new Vertex(float.NaN, float.NaN);

		private static readonly Stack<Vertex> POOL = new Stack<Vertex>();

		private static Vertex Create(float x, float y) {
			if (float.IsNaN(x) || float.IsNaN(y)) {
				return VERTEX_AT_INFINITY;
			}
			if (POOL.Count > 0) {
				return POOL.Pop().Init(x, y);
			}
			return new Vertex(x, y);
		}

		private static int nvertices;

		private Vector2 coord;

		public Vector2 Coord => coord;

		public int VertexIndex { get; private set; }

		private Vertex(float x, float y) {
			Init(x, y);
		}

		private Vertex Init(float x, float y) {
			coord = new Vector2(x, y);
			return this;
		}

		public void Dispose() {
			POOL.Push(this);
		}

		public void SetIndex() {
			VertexIndex = nvertices++;
		}

		public override string ToString() {
			return $"Vertex ({VertexIndex})";
		}

		/**
		 * This is the only way to make a Vertex
		 * 
		 * @param halfedge0
		 * @param halfedge1
		 * @return 
		 * 
		 */
		public static Vertex Intersect(HalfEdge halfedge0, HalfEdge halfedge1) {
			Edge edge0, edge1, edge;
			HalfEdge halfEdge;
			float determinant, intersectionX, intersectionY;
			bool rightOfSite;

			edge0 = halfedge0.edge;
			edge1 = halfedge1.edge;
			if (edge0 == null || edge1 == null) {
				return null;
			}
			if (edge0.RightSite == edge1.RightSite) {
				return null;
			}

			determinant = edge0.a * edge1.b - edge0.b * edge1.a;
			if (-1.0e-10 < determinant && determinant < 1.0e-10) {
				// the edges are parallel
				return null;
			}

			intersectionX = (edge0.c * edge1.b - edge1.c * edge0.b) / determinant;
			intersectionY = (edge1.c * edge0.a - edge0.c * edge1.a) / determinant;

			if (Voronoi.CompareByYThenX(edge0.RightSite, edge1.RightSite) < 0) {
				halfEdge = halfedge0;
				edge = edge0;
			}
			else {
				halfEdge = halfedge1;
				edge = edge1;
			}
			rightOfSite = intersectionX >= edge.RightSite.X;
			if ((rightOfSite && halfEdge.leftRight == Side.Left)
				|| (!rightOfSite && halfEdge.leftRight == Side.Right)) {
				return null;
			}

			return Create(intersectionX, intersectionY);
		}

		public float X => coord.x;

		public float Y => coord.y;
	}
}