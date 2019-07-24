using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeHexEdge : HexEdge {
	public SlopeHexEdge() {}

	public SlopeHexEdge(EdgeVertices edge1, EdgeVertices edge2, Color color1, Color color2) : base(edge1, edge2, color1, color2) {}

	public override void Triangulate(HexMesh mesh) {
		EdgeVertices e1 = this.edge1;
		Color c1 = this.color1;

		for (int step = 1; step < HexMetrics.terraceSteps + 1; ++step) {
			EdgeVertices e2 = EdgeVertices.TerraceLerp(this.edge1, this.edge2, step);
			Color c2 = HexMetrics.ColorLerp(this.color1, this.color2, step);
			HexEdge.TriangulateQuad(mesh, e1, e2, c1, c2);

			c1 = c2;
			e1 = e2;
		}
	}
}
