using System;
using UnityEngine;

namespace Delaunay {

	internal sealed class EdgeList : IDisposable {
		private readonly float deltax;
		private readonly float xmin;

		private readonly int hashSize;
		private HalfEdge[] hash;

		private HalfEdge LeftEnd { get; set; }
		private HalfEdge RightEnd { get; set; }

		public void Dispose() {
			HalfEdge halfEdge = LeftEnd;
			while (halfEdge != RightEnd) {
				HalfEdge prevHe = halfEdge;
				halfEdge = halfEdge.edgeListRightNeighbor;
				prevHe.Dispose();
			}
			LeftEnd = null;
			RightEnd.Dispose();
			RightEnd = null;

			int i;
			for (i = 0; i < hashSize; ++i) {
				hash[i] = null;
			}
			hash = null;
		}

		public EdgeList(float xMin, float deltaX, int sqrtSites) {
			xmin = xMin;
			deltax = deltaX;
			hashSize = 2 * sqrtSites;

			hash = new HalfEdge[hashSize];

			// two dummy Halfedges:
			LeftEnd = HalfEdge.CreateDummy();
			RightEnd = HalfEdge.CreateDummy();
			LeftEnd.edgeListLeftNeighbor = null;
			LeftEnd.edgeListRightNeighbor = RightEnd;
			RightEnd.edgeListLeftNeighbor = LeftEnd;
			RightEnd.edgeListRightNeighbor = null;
			hash[0] = LeftEnd;
			hash[hashSize - 1] = RightEnd;
		}

		/**
		 * Insert newHalfedge to the right of lb 
		 * @param lb
		 * @param newHalfedge
		 * 
		 */
		public void Insert(HalfEdge lb, HalfEdge newHalfEdge) {
			newHalfEdge.edgeListLeftNeighbor = lb;
			newHalfEdge.edgeListRightNeighbor = lb.edgeListRightNeighbor;
			lb.edgeListRightNeighbor.edgeListLeftNeighbor = newHalfEdge;
			lb.edgeListRightNeighbor = newHalfEdge;
		}

		/**
		 * This function only removes the HalfEdge from the left-right list.
		 * We cannot dispose it yet because we are still using it. 
		 * @param halfEdge
		 * 
		 */
		public void Remove(HalfEdge halfEdge) {
			halfEdge.edgeListLeftNeighbor.edgeListRightNeighbor = halfEdge.edgeListRightNeighbor;
			halfEdge.edgeListRightNeighbor.edgeListLeftNeighbor = halfEdge.edgeListLeftNeighbor;
			halfEdge.edge = Edge.DELETED;
			halfEdge.edgeListLeftNeighbor = halfEdge.edgeListRightNeighbor = null;
		}

		/**
		 * Find the rightmost HalfEdge that is still left of p 
		 * @param p
		 * @return 
		 * 
		 */
		public HalfEdge EdgeListLeftNeighbor(Vector2 p) {
			int i, bucket;
			HalfEdge halfEdge;

			/* Use hash table to get close to desired halfedge */
			bucket = (int)((p.x - xmin) / deltax * hashSize);
			if (bucket < 0) {
				bucket = 0;
			}
			if (bucket >= hashSize) {
				bucket = hashSize - 1;
			}
			halfEdge = GetHash(bucket);
			if (halfEdge == null) {
				for (i = 1; true; ++i) {
					if ((halfEdge = GetHash(bucket - i)) != null)
						break;
					if ((halfEdge = GetHash(bucket + i)) != null)
						break;
				}
			}
			/* Now search linear list of halfedges for the correct one */
			if (halfEdge == LeftEnd || (halfEdge != RightEnd && halfEdge.IsLeftOf(p))) {
				do {
					halfEdge = halfEdge.edgeListRightNeighbor;
				} while (halfEdge != RightEnd && halfEdge.IsLeftOf(p));
				halfEdge = halfEdge.edgeListLeftNeighbor;
			}
			else {
				do {
					halfEdge = halfEdge.edgeListLeftNeighbor;
				} while (halfEdge != LeftEnd && !halfEdge.IsLeftOf(p));
			}

			/* Update hash table and reference counts */
			if (bucket > 0 && bucket < hashSize - 1) {
				hash[bucket] = halfEdge;
			}
			return halfEdge;
		}

		/* Get entry from hash table, pruning any deleted nodes */
		private HalfEdge GetHash(int b) {
			HalfEdge halfEdge;

			if (b < 0 || b >= hashSize) {
				return null;
			}
			halfEdge = hash[b];
			if (halfEdge != null && halfEdge.edge == Edge.DELETED) {
				/* Hash table points to deleted halfedge.  Patch as necessary. */
				hash[b] = null;

				// still can't dispose halfEdge yet!
				return null;
			}
			return halfEdge;
		}

	}
}