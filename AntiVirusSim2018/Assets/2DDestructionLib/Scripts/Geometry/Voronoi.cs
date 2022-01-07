/*
 * The author of this software is Steven Fortune.  Copyright (c) 1994 by AT&T
 * Bell Laboratories.
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace Delaunay {
	public sealed class Voronoi : IDisposable {
		private SiteList sites;
		private Dictionary<Vector2, Site> sitesIndexedByLocation;
		private List<Triangle> triangles;
		private List<Edge> edges;

		// TODO generalize this so it doesn't have to be a rectangle;
		private readonly Rect plotBounds;

		public void Dispose() {
			int i, n;
			if (sites != null) {
				sites.Dispose();
				sites = null;
			}
			if (triangles != null) {
				n = triangles.Count;
				for (i = 0; i < n; ++i) {
					triangles[i].Dispose();
				}
				triangles.Clear();
				triangles = null;
			}
			if (edges != null) {
				n = edges.Count;
				for (i = 0; i < n; ++i) {
					edges[i].Dispose();
				}
				edges.Clear();
				edges = null;
			}

			sitesIndexedByLocation = null;
		}

		public Voronoi(List<Vector2> points, Rect plotBounds) {
			sites = new SiteList();
			sitesIndexedByLocation = new Dictionary<Vector2, Site>(); // XXX: Used to be Dictionary(true) -- weak refs. 
			AddSites(points);
			this.plotBounds = plotBounds;
			triangles = new List<Triangle>();
			edges = new List<Edge>();
			FortunesAlgorithm();
		}

		private void AddSites(List<Vector2> points) {
			int length = points.Count;
			for (int i = 0; i < length; ++i) {
				AddSite(points[i], i);
			}
		}

		private void AddSite(Vector2 p, int index) {
			if (sitesIndexedByLocation.ContainsKey(p))
				return; // Prevent duplicate site! (Adapted from https://github.com/nodename/as3delaunay/issues/1)
			float weight = UnityEngine.Random.value * 100f;
			Site site = Site.Create(p, (uint)index);
			sites.Add(site);
			sitesIndexedByLocation[p] = site;
		}

		public List<Triangle> Triangles() {
			return triangles;
		}

		public SiteList Sites() {
			return sites;
		}

		public List<List<Vector2>> Regions() {
			return sites.Regions(plotBounds);
		}

		private Site fortunesAlgorithmBottomMostSite;

		private void FortunesAlgorithm() {
			Site newSite, bottomSite, topSite, tempSite;
			Vertex v, vertex;
			Vector2 newintstar = Vector2.zero; //Because the compiler doesn't know that it will have a value - Julian
			Side leftRight;
			HalfEdge lbnd, rbnd, llbnd, rrbnd, bisector;
			Edge edge;

			Rect dataBounds = sites.GetSitesBounds();

			int sqrtSites = (int)(Mathf.Sqrt(sites.Count + 4));
			HalfEdgePriorityQueue heap = new HalfEdgePriorityQueue(dataBounds.y, dataBounds.height, sqrtSites);
			EdgeList edgeList = new EdgeList(dataBounds.x, dataBounds.width, sqrtSites);
			List<HalfEdge> halfEdges = new List<HalfEdge>();
			List<Vertex> vertices = new List<Vertex>();

			fortunesAlgorithmBottomMostSite = sites.Next();
			newSite = sites.Next();

			for (;;) {
				if (heap.Empty() == false) {
					newintstar = heap.Min();
				}

				if (newSite != null
					&& (heap.Empty() || CompareByYThenX(newSite, newintstar) < 0)) {
					/* new site is smallest */
					//trace("smallest: new site " + newSite);

					// Step 8:
					lbnd = edgeList.EdgeListLeftNeighbor(newSite.Coord); // the HalfEdge just to the left of newSite

					//trace("lbnd: " + lbnd);
					rbnd = lbnd.edgeListRightNeighbor; // the HalfEdge just to the right

					//trace("rbnd: " + rbnd);
					bottomSite = FortunesAlgorithm_rightRegion(lbnd); // this is the same as leftRegion(rbnd)

					// this Site determines the region containing the new site
					//trace("new Site is in region of existing site: " + bottomSite);

					// Step 9:
					edge = Edge.CreateBisectingEdge(bottomSite, newSite);

					//trace("new edge: " + edge);
					edges.Add(edge);

					bisector = HalfEdge.Create(edge, Side.Left);
					halfEdges.Add(bisector);

					// inserting two Halfedges into edgeList constitutes Step 10:
					// insert bisector to the right of lbnd:
					edgeList.Insert(lbnd, bisector);

					// first half of Step 11:
					if ((vertex = Vertex.Intersect(lbnd, bisector)) != null) {
						vertices.Add(vertex);
						heap.Remove(lbnd);
						lbnd.vertex = vertex;
						lbnd.ystar = vertex.Y + newSite.Dist(vertex);
						heap.Insert(lbnd);
					}

					lbnd = bisector;
					bisector = HalfEdge.Create(edge, Side.Right);
					halfEdges.Add(bisector);

					// second HalfEdge for Step 10:
					// insert bisector to the right of lbnd:
					edgeList.Insert(lbnd, bisector);

					// second half of Step 11:
					if ((vertex = Vertex.Intersect(bisector, rbnd)) != null) {
						vertices.Add(vertex);
						bisector.vertex = vertex;
						bisector.ystar = vertex.Y + newSite.Dist(vertex);
						heap.Insert(bisector);
					}

					newSite = sites.Next();
				}
				else if (heap.Empty() == false) {
					/* intersection is smallest */
					lbnd = heap.ExtractMin();
					llbnd = lbnd.edgeListLeftNeighbor;
					rbnd = lbnd.edgeListRightNeighbor;
					rrbnd = rbnd.edgeListRightNeighbor;
					bottomSite = FortunesAlgorithm_leftRegion(lbnd);
					topSite = FortunesAlgorithm_rightRegion(rbnd);

					// these three sites define a Delaunay triangle
					// (not actually using these for anything...)
					triangles.Add(new Triangle(bottomSite, topSite, FortunesAlgorithm_rightRegion(lbnd)));

					v = lbnd.vertex;
					v.SetIndex();
					lbnd.edge.SetVertex((Side)lbnd.leftRight, v);
					rbnd.edge.SetVertex((Side)rbnd.leftRight, v);
					edgeList.Remove(lbnd);
					heap.Remove(rbnd);
					edgeList.Remove(rbnd);
					leftRight = Side.Left;
					if (bottomSite.Y > topSite.Y) {
						tempSite = bottomSite;
						bottomSite = topSite;
						topSite = tempSite;
						leftRight = Side.Right;
					}
					edge = Edge.CreateBisectingEdge(bottomSite, topSite);
					edges.Add(edge);
					bisector = HalfEdge.Create(edge, leftRight);
					halfEdges.Add(bisector);
					edgeList.Insert(llbnd, bisector);
					edge.SetVertex(leftRight.Other(), v);
					if ((vertex = Vertex.Intersect(llbnd, bisector)) != null) {
						vertices.Add(vertex);
						heap.Remove(llbnd);
						llbnd.vertex = vertex;
						llbnd.ystar = vertex.Y + bottomSite.Dist(vertex);
						heap.Insert(llbnd);
					}
					if ((vertex = Vertex.Intersect(bisector, rrbnd)) != null) {
						vertices.Add(vertex);
						bisector.vertex = vertex;
						bisector.ystar = vertex.Y + bottomSite.Dist(vertex);
						heap.Insert(bisector);
					}
				}
				else {
					break;
				}
			}

			// heap should be empty now
			heap.Dispose();
			edgeList.Dispose();

			foreach (HalfEdge halfEdge in halfEdges) {
				halfEdge.ReallyDispose();
			}
			halfEdges.Clear();

			// we need the vertices to clip the edges
			foreach (Edge e in edges) {
				edge = e;
				edge.ClipVertices(plotBounds);
			}

			// but we don't actually ever use them again!
			foreach (Vertex t in vertices) {
				vertex = t;
				vertex.Dispose();
			}
			vertices.Clear();
		}

		private Site FortunesAlgorithm_leftRegion(HalfEdge he) {
			Edge edge = he.edge;
			if (edge == null) {
				return fortunesAlgorithmBottomMostSite;
			}
			return edge.Site((Side)he.leftRight);
		}

		private Site FortunesAlgorithm_rightRegion(HalfEdge he) {
			Edge edge = he.edge;
			if (edge == null) {
				return fortunesAlgorithmBottomMostSite;
			}
			return edge.Site(((Side)he.leftRight).Other());
		}

		public static int CompareByYThenX(Site s1, Site s2) {
			if (s1.Y < s2.Y)
				return -1;
			if (s1.Y > s2.Y)
				return 1;
			if (s1.X < s2.X)
				return -1;
			if (s1.X > s2.X)
				return 1;
			return 0;
		}

		private static int CompareByYThenX(Site s1, Vector2 s2) {
			if (s1.Y < s2.y)
				return -1;
			if (s1.Y > s2.y)
				return 1;
			if (s1.X < s2.x)
				return -1;
			if (s1.X > s2.x)
				return 1;
			return 0;
		}
	}
}