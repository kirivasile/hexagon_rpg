using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexEdge {
	protected EdgeVertices edge1, edge2;
	protected Color color1, color2;

	public HexEdge() {}

	public HexEdge(EdgeVertices edge1, EdgeVertices edge2, Color color1, Color color2) {
		this.edge1 = edge1;
		this.edge2 = edge2;
		this.color1 = color1;
		this.color2 = color2;
	}

	public abstract void Triangulate(HexMesh mesh);

	public static HexEdge Build(HexCell cell1, HexCell cell2, HexDirection direction) {
		EdgeVertices e1 = new EdgeVertices(
			cell1.Position + HexMetrics.GetFirstSolidCorner(direction),
			cell1.Position + HexMetrics.GetSecondSolidCorner(direction)
		);
		EdgeVertices e2 = new EdgeVertices(
			cell2.Position + HexMetrics.GetSecondSolidCorner(direction.Opposite()),
			cell2.Position + HexMetrics.GetFirstSolidCorner(direction.Opposite())
		);
		HexEdge edge;
		HexEdgeType edgeType = HexMetrics.GetEdgeType(cell1.Elevation, cell2.Elevation);
		if (edgeType == HexEdgeType.Flat) {
			edge = new FlatHexEdge(e1, e2, cell1.Color, cell2.Color);
		}
		else if (edgeType == HexEdgeType.Slope) {
			edge = new SlopeHexEdge(e1, e2, cell1.Color, cell2.Color);	
		}
		else {
			edge = new CliffHexEdge(e1, e2, cell1.Color, cell2.Color);	
		}
		return edge;
	}

	protected static void TriangulateQuad(HexMesh mesh, EdgeVertices e1, EdgeVertices e2, Color c1, Color c2) {
		mesh.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
		mesh.AddQuadColor(c1, c1, c2, c2);
		mesh.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
		mesh.AddQuadColor(c1, c1, c2, c2);
		mesh.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
		mesh.AddQuadColor(c1, c1, c2, c2);
		mesh.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
		mesh.AddQuadColor(c1, c1, c2, c2);
	}
}
