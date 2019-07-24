using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatHexEdge : HexEdge {
	public FlatHexEdge() {}

	public FlatHexEdge(EdgeVertices edge1, EdgeVertices edge2, Color color1, Color color2) : base(edge1, edge2, color1, color2) {}

	public override void Triangulate(HexMesh mesh) {
		HexEdge.TriangulateQuad(mesh, edge1, edge2, color1, color2);
	}
}
