using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatCorner : Corner {
	public FlatCorner() {}

	public FlatCorner(
		Vector3 beginCorner, Vector3 leftCorner, Vector3 rightCorner,
		HexCell beginCell, HexCell leftCell, HexCell rightCell
	) : base(beginCorner, leftCorner, rightCorner, beginCell, leftCell, rightCell) {}

	public override void Triangulate(HexMesh mesh) {
		mesh.AddTriangle(beginCorner, leftCorner, rightCorner);
		mesh.AddTriangleColor(beginCell.Color, leftCell.Color, rightCell.Color);
	}
}
