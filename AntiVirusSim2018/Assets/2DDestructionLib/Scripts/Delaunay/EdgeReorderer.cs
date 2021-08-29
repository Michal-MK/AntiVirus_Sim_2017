using System;
using System.Collections.Generic;

/** This class is horrible, and ought to be nuked from orbit. But the library is
heavily dependent upon it in undocumented ways.

It's viciously complicated, and is used all over the library in odd places where it
shouldn't be used, with no explanation - but with a hard dependency in that it
doesn't merely "re-order" edges (as the name suggests!) but often "generates" them
too.

It feels like it was intended to be semi-optimized (in the original AS3? probably),
but in a modern language like C#, there are far far better ways of doing this.

Currently: in my own projects, I am DELETING the output of this class, it's far
too dangerous to use in production. I recommend you do the same: write an
equivalent class (or better: set of classes) that are C#-friendly and do what they
say, and no more and no less. Hopefully one day someone will re-write this thing
and REMOVE IT from the rest of the library (all the places where it shouldn't be used)
*/
namespace Delaunay {
	public enum VertexOrSite {
		VERTEX
	}

	sealed class EdgeReorderer : IDisposable {
		public List<Edge> Edges { get; private set; }

		public List<Side> EdgeOrientations { get; private set; }

		public EdgeReorderer(List<Edge> origEdges, VertexOrSite criterion) {
			Edges = new List<Edge>();
			EdgeOrientations = new List<Side>();
			if (origEdges.Count > 0) {
				Edges = ReorderEdges(origEdges, criterion);
			}
		}

		public void Dispose() {
			Edges = null;
			EdgeOrientations = null;
		}

		private List<Edge> ReorderEdges(List<Edge> origEdges, VertexOrSite criterion) {
			int i;
			int n = origEdges.Count;
			Edge edge;

			// we're going to reorder the edges in order of traversal
			bool[] done = new bool[n];
			int nDone = 0;
			for (int j = 0; j < n; j++) {
				done[j] = false;
			}
			List<Edge> newEdges = new List<Edge>(); // TODO: Switch to Deque if performance is a concern

			i = 0;
			edge = origEdges[i];
			newEdges.Add(edge);
			EdgeOrientations.Add(Side.Left);
			ICoord firstPoint = (criterion == VertexOrSite.VERTEX) ? (ICoord)edge.LeftVertex : (ICoord)edge.LeftSite;
			ICoord lastPoint = (criterion == VertexOrSite.VERTEX) ? (ICoord)edge.RightVertex : (ICoord)edge.RightSite;

			if (firstPoint == Vertex.VERTEX_AT_INFINITY || lastPoint == Vertex.VERTEX_AT_INFINITY) {
				return new List<Edge>();
			}

			done[i] = true;
			++nDone;

			while (nDone < n) {
				for (i = 1; i < n; ++i) {
					if (done[i]) {
						continue;
					}
					edge = origEdges[i];
					ICoord leftPoint = (criterion == VertexOrSite.VERTEX) ? (ICoord)edge.LeftVertex : (ICoord)edge.LeftSite;
					ICoord rightPoint = (criterion == VertexOrSite.VERTEX) ? (ICoord)edge.RightVertex : (ICoord)edge.RightSite;
					if (leftPoint == Vertex.VERTEX_AT_INFINITY || rightPoint == Vertex.VERTEX_AT_INFINITY) {
						return new List<Edge>();
					}
					if (leftPoint == lastPoint) {
						lastPoint = rightPoint;
						EdgeOrientations.Add(Side.Left);
						newEdges.Add(edge);
						done[i] = true;
					}
					else if (rightPoint == firstPoint) {
						firstPoint = leftPoint;
						EdgeOrientations.Insert(0, Side.Left); // TODO: Change data structure if this is slow
						newEdges.Insert(0, edge);
						done[i] = true;
					}
					else if (leftPoint == firstPoint) {
						firstPoint = rightPoint;
						EdgeOrientations.Insert(0, Side.Right);
						newEdges.Insert(0, edge);
						done[i] = true;
					}
					else if (rightPoint == lastPoint) {
						lastPoint = leftPoint;
						EdgeOrientations.Add(Side.Right);
						newEdges.Add(edge);
						done[i] = true;
					}
					if (done[i]) {
						++nDone;
					}
				}
			}
			return newEdges;
		}
	}
}