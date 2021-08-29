using UnityEngine;
using System.Collections.Generic;

namespace Delaunay.Geo {
	public sealed class Polygon {
		private readonly List<Vector2> vertices;

		public Polygon(List<Vector2> verts) {
			vertices = verts;
		}

		public Winding Winding() {
			float signedDoubleArea = SignedDoubleArea();
			if (signedDoubleArea < 0) {
				return Geo.Winding.Clockwise;
			}
			if (signedDoubleArea > 0) {
				return Geo.Winding.CounterClockwise;
			}
			return Geo.Winding.None;
		}

		private float SignedDoubleArea() // XXX: I'm a bit nervous about this because Actionscript represents everything as doubles, not floats
		{
			int index, nextIndex;
			int n = vertices.Count;
			Vector2 point, next;
			float signedDoubleArea = 0; // Losing lots of precision?
			for (index = 0; index < n; ++index) {
				nextIndex = (index + 1) % n;
				point = vertices[index];
				next = vertices[nextIndex];
				signedDoubleArea += point.x * next.y - next.x * point.y;
			}
			return signedDoubleArea;
		}
	}
}