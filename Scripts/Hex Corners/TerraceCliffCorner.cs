using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraceCliffCorner : Corner {

	public TerraceCliffCorner() {}

	public TerraceCliffCorner(
		Vector3 beginCorner, Vector3 leftCorner, Vector3 rightCorner,
		HexCell beginCell, HexCell leftCell, HexCell rightCell
	) : base(beginCorner, leftCorner, rightCorner, beginCell, leftCell, rightCell) {}

	public override void Triangulate(HexMesh mesh) {
		float lerpCoef = (float)(leftCell.Elevation - beginCell.Elevation) / (rightCell.Elevation - beginCell.Elevation);
		if (lerpCoef < 0) {
			lerpCoef = -lerpCoef;
		}
		Vector3 boundary = Vector3.Lerp(HexMetrics.Perturb(beginCorner), HexMetrics.Perturb(rightCorner), lerpCoef);
		Color boundaryColor = Color.Lerp(beginCell.Color, rightCell.Color, lerpCoef);

		Corner.TriangulateBoundaryTriangle(mesh, beginCorner, beginCell, leftCorner, leftCell, boundary, boundaryColor);

		if (HexMetrics.GetEdgeType(leftCell.Elevation, rightCell.Elevation) == HexEdgeType.Slope) {
			Corner.TriangulateBoundaryTriangle(mesh, leftCorner, leftCell, rightCorner, rightCell, boundary, boundaryColor);			
		} else {
			mesh.AddTriangle(HexMetrics.Perturb(leftCorner), HexMetrics.Perturb(rightCorner), boundary, false);
			mesh.AddTriangleColor(leftCell.Color, rightCell.Color, boundaryColor);
		}
	}
}
