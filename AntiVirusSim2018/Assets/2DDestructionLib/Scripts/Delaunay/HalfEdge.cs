using UnityEngine;
using System;
using System.Collections.Generic;

namespace Delaunay {

	public sealed class HalfEdge : IDisposable {
		private static readonly Stack<HalfEdge> POOL = new Stack<HalfEdge>();

		public static HalfEdge Create(Edge edge, Side? lr) {
			if (POOL.Count > 0) {
				return POOL.Pop().Init(edge, lr);
			}
			return new HalfEdge(edge, lr);
		}

		public static HalfEdge CreateDummy() {
			return Create(null, null);
		}

		public HalfEdge edgeListLeftNeighbor, edgeListRightNeighbor;
		public HalfEdge nextInPriorityQueue;

		public Edge edge;
		public Side? leftRight;
		public Vertex vertex;

		// the vertex's y-coordinate in the transformed Voronoi space V*
		public float ystar;

		public HalfEdge(Edge edge = null, Side? lr = null) {
			Init(edge, lr);
		}

		private HalfEdge Init(Edge fromEdge, Side? lr) {
			edge = fromEdge;
			leftRight = lr;
			nextInPriorityQueue = null;
			vertex = null;
			return this;
		}

		public override string ToString() {
			return "HalfEdge (leftRight: " + leftRight + "; vertex: " + vertex + ")";
		}

		public void Dispose() {
			if (edgeListLeftNeighbor != null || edgeListRightNeighbor != null) {
				// still in EdgeList
				return;
			}
			if (nextInPriorityQueue != null) {
				// still in PriorityQueue
				return;
			}
			edge = null;
			leftRight = null;
			vertex = null;
			POOL.Push(this);
		}

		public void ReallyDispose() {
			edgeListLeftNeighbor = null;
			edgeListRightNeighbor = null;
			nextInPriorityQueue = null;
			edge = null;
			leftRight = null;
			vertex = null;
			POOL.Push(this);
		}

		internal bool IsLeftOf(Vector2 p) {
			Site topSite;
			bool rightOfSite, above, fast;
			float dxp, dyp, dxs, t1, t2, t3, yl;

			topSite = edge.RightSite;
			rightOfSite = p.x > topSite.X;
			if (rightOfSite && this.leftRight == Side.Left) {
				return true;
			}
			if (!rightOfSite && this.leftRight == Side.Right) {
				return false;
			}

			if (edge.a == 1.0) {
				dyp = p.y - topSite.Y;
				dxp = p.x - topSite.X;
				fast = false;
				if ((!rightOfSite && edge.b < 0.0) || (rightOfSite && edge.b >= 0.0)) {
					above = dyp >= edge.b * dxp;
					fast = above;
				}
				else {
					above = p.x + p.y * edge.b > edge.c;
					if (edge.b < 0.0) {
						above = !above;
					}
					if (!above) {
						fast = true;
					}
				}
				if (!fast) {
					dxs = topSite.X - edge.LeftSite.X;
					above = edge.b * (dxp * dxp - dyp * dyp) <
							dxs * dyp * (1.0 + 2.0 * dxp / dxs + edge.b * edge.b);
					if (edge.b < 0.0) {
						above = !above;
					}
				}
			}
			else {
				/* edge.b == 1.0 */
				yl = edge.c - edge.a * p.x;
				t1 = p.y - yl;
				t2 = p.x - topSite.X;
				t3 = yl - topSite.Y;
				above = t1 * t1 > t2 * t2 + t3 * t3;
			}
			return leftRight == Side.Left ? above : !above;
		}

	}
}