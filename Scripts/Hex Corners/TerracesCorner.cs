using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerracesCorner : Corner {
	public TerracesCorner() {}

	public TerracesCorner(
		Vector3 beginCorner, Vector3 leftCorner, Vector3 rightCorner,
		HexCell beginCell, HexCell leftCell, HexCell rightCell
	) : base(beginCorner, leftCorner, rightCorner, beginCell, leftCell, rightCell) {}

	public override void Triangulate(HexMesh mesh) {
		Vector3 v1 = beginCorner;
		Vector3 v2 = beginCorner;
		Color c1 = beginCell.Color;
		Color c2 = beginCell.Color;

		for (int step = 1; step < HexMetrics.terraceSteps + 1; ++step) {
			Vector3 v3 = HexMetrics.TerraceLerp(beginCorner, leftCorner, step);
			Vector3 v4 = HexMetrics.TerraceLerp(beginCorner, rightCorner, step);

			Color c3 = HexMetrics.ColorLerp(beginCell.Color, leftCell.Color, step);
			Color c4 = HexMetrics.ColorLerp(beginCell.Color, rightCell.Color, step);

			if (step == 1) {
				mesh.AddTriangle(v1, v3, v4);
				mesh.AddTriangleColor(c1, c3, c4);
			} else {
				mesh.AddQuad(v1, v2, v3, v4);
				mesh.AddQuadColor(c1, c2, c3, c4);
			}
			v1 = v3;
			v2 = v4;
			c1 = c3;
			c2 = c4;
		}
	}
}
