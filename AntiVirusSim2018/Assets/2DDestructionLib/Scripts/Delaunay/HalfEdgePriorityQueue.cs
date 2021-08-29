using System;
using UnityEngine;

namespace Delaunay {

	internal sealed class HalfEdgePriorityQueue : IDisposable // also known as heap
	{
		private HalfEdge[] hash;
		private int count;
		private int minBucket;
		private readonly int hashSize;

		private readonly float yMin;
		private readonly float yDelta;

		public HalfEdgePriorityQueue(float yMin, float yDelta, int sqrtSites) {
			this.yMin = yMin;
			this.yDelta = yDelta;
			hashSize = 4 * sqrtSites;
			Initialize();
		}

		public void Dispose() {
			for (int i = 0; i < hashSize; ++i) {
				hash[i].Dispose();
				hash[i] = null;
			}
			hash = null;
		}

		private void Initialize() {
			int i;

			count = 0;
			minBucket = 0;
			hash = new HalfEdge[hashSize];

			// dummy HalfEdge at the top of each hash
			for (i = 0; i < hashSize; ++i) {
				hash[i] = HalfEdge.CreateDummy();
				hash[i].nextInPriorityQueue = null;
			}
		}

		public void Insert(HalfEdge halfEdge) {
			HalfEdge previous, next;
			int insertionBucket = Bucket(halfEdge);
			if (insertionBucket < minBucket) {
				minBucket = insertionBucket;
			}
			previous = hash[insertionBucket];
			while ((next = previous.nextInPriorityQueue) != null
				   && (halfEdge.ystar > next.ystar || (halfEdge.ystar == next.ystar && halfEdge.vertex.X > next.vertex.X))) {
				previous = next;
			}
			halfEdge.nextInPriorityQueue = previous.nextInPriorityQueue;
			previous.nextInPriorityQueue = halfEdge;
			++count;
		}

		public void Remove(HalfEdge halfEdge) {
			HalfEdge previous;
			int removalBucket = Bucket(halfEdge);

			if (halfEdge.vertex != null) {
				previous = hash[removalBucket];
				while (previous.nextInPriorityQueue != halfEdge) {
					previous = previous.nextInPriorityQueue;
				}
				previous.nextInPriorityQueue = halfEdge.nextInPriorityQueue;
				count--;
				halfEdge.vertex = null;
				halfEdge.nextInPriorityQueue = null;
				halfEdge.Dispose();
			}
		}

		private int Bucket(HalfEdge halfEdge) {
			int theBucket = (int)((halfEdge.ystar - yMin) / yDelta * hashSize);
			if (theBucket < 0)
				theBucket = 0;
			if (theBucket >= hashSize)
				theBucket = hashSize - 1;
			return theBucket;
		}

		private bool IsEmpty(int bucket) {
			return (hash[bucket].nextInPriorityQueue == null);
		}

		private void AdjustMinBucket() {
			while (minBucket < hashSize - 1 && IsEmpty(minBucket)) {
				++minBucket;
			}
		}

		public bool Empty() {
			return count == 0;
		}

		public Vector2 Min() {
			AdjustMinBucket();
			HalfEdge answer = hash[minBucket].nextInPriorityQueue;
			return new Vector2(answer.vertex.X, answer.ystar);
		}

		public HalfEdge ExtractMin() {
			HalfEdge answer;

			// get the first real HalfEdge in minBucket
			answer = hash[minBucket].nextInPriorityQueue;

			hash[minBucket].nextInPriorityQueue = answer.nextInPriorityQueue;
			count--;
			answer.nextInPriorityQueue = null;

			return answer;
		}
	}
}