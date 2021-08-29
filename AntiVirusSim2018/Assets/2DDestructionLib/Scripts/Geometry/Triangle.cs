using System;
using System.Collections.Generic;

namespace Delaunay {

	public sealed class Triangle : IDisposable {
		public List<Site> Sites { get; private set; }

		public Triangle(Site a, Site b, Site c) {
			Sites = new List<Site> { a, b, c };
		}

		public void Dispose() {
			Sites.Clear();
			Sites = null;
		}
	}
}