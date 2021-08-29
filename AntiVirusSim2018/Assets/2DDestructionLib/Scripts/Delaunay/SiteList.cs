using UnityEngine;
using System;
using System.Collections.Generic;

namespace Delaunay {

	public sealed class SiteList : IDisposable {
		public List<Site> sites;
		private int currentIndex;

		private bool sorted;

		public SiteList() {
			sites = new List<Site>();
			sorted = false;
		}

		public void Dispose() {
			if (sites != null) {
				for (int i = 0; i < sites.Count; i++) {
					Site site = sites[i];
					site.Dispose();
				}
				sites.Clear();
				sites = null;
			}
		}

		public int Add(Site site) {
			sorted = false;
			sites.Add(site);
			return sites.Count;
		}

		public int Count => sites.Count;

		public Site Next() {
			if (sorted == false) {
				Debug.LogError("SiteList::next():  sites have not been sorted");
			}
			if (currentIndex < sites.Count) {
				return sites[currentIndex++];
			}
			return null;
		}

		internal Rect GetSitesBounds() {
			if (sorted == false) {
				Site.SortSites(sites);
				currentIndex = 0;
				sorted = true;
			}
			float xmin, xmax, ymin, ymax;
			if (sites.Count == 0) {
				return new Rect(0, 0, 0, 0);
			}
			xmin = float.MaxValue;
			xmax = float.MinValue;
			for (int i = 0; i < sites.Count; i++) {
				Site site = sites[i];
				if (site.X < xmin) {
					xmin = site.X;
				}
				if (site.X > xmax) {
					xmax = site.X;
				}
			}

			// here's where we assume that the sites have been sorted on y:
			ymin = sites[0].Y;
			ymax = sites[sites.Count - 1].Y;

			return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
		}

		public List<List<Vector2>> Regions(Rect plotBounds) {
			List<List<Vector2>> regions = new List<List<Vector2>>();
			foreach (Site site in sites) {
				regions.Add(site.Region(plotBounds));
			}
			return regions;
		}
	}
}