using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Corner {
	protected Vector3 beginCorner, leftCorner, rightCorner;
	protected HexCell beginCell, leftCell, rightCell;

	public Corner() {}

	public Corner(
		Vector3 beginCorner, Vector3 leftCorner, Vector3 rightCorner,
		HexCell beginCell, HexCell leftCell, HexCell rightCell
	) {
		this.beginCorner = beginCorner;
		this.leftCorner = leftCorner;
		this.rightCorner = rightCorner;
		this.beginCell = beginCell;
		this.leftCell = leftCell;
		this.rightCell = rightCell;
	}

	public abstract void Triangulate(HexMesh mesh);

	public static Corner Build(HexCell begin, HexCell left, HexCell right, HexDirection leftDirection) {
		Vector3 beginCorner = begin.Position + HexMetrics.GetSecondSolidCorner(leftDirection);
		Vector3 leftCorner = left.Position + HexMetrics.GetFirstSolidCorner(leftDirection.Opposite());
		Vector3 rightCorner = right.Position + HexMetrics.GetSecondSolidCorner(leftDirection.Previous2());

		HexEdgeType leftEdgeType = HexMetrics.GetEdgeType(begin.Elevation, left.Elevation);
		HexEdgeType rightEdgeType = HexMetrics.GetEdgeType(begin.Elevation, right.Elevation);
		HexEdgeType endEdgeType = HexMetrics.GetEdgeType(left.Elevation, right.Elevation);

		Corner corner;

		if (leftEdgeType == HexEdgeType.Slope) {
			if (rightEdgeType == HexEdgeType.Slope) {
				corner = new TerracesCorner(
					beginCorner, leftCorner, rightCorner,
					begin, left, right
				);
			} else if (rightEdgeType == HexEdgeType.Flat) {
				corner = new TerracesCorner(
					leftCorner, rightCorner, beginCorner,
					left, right, begin
				);
			} else {
				corner = new TerraceCliffCorner(
					beginCorner, leftCorner, rightCorner,
					begin, left, right
				);
			}
		} else if (rightEdgeType == HexEdgeType.Slope) {
			if (leftEdgeType == HexEdgeType.Flat) {
				corner = new TerracesCorner(
					rightCorner, beginCorner, leftCorner,
					right, begin, left
				);
			} else {
				corner = new CliffTerraceCorner(
					beginCorner, leftCorner, rightCorner,
					begin, left, right
				);
			}
		} else if (endEdgeType == HexEdgeType.Slope) {
			if (left.Elevation < right.Elevation) {
				corner = new CliffTerraceCorner(
					rightCorner, beginCorner, leftCorner,
					right, begin, left
				);
			} else {
				corner = new TerraceCliffCorner(
					leftCorner, rightCorner, beginCorner,
					left, right, begin
				);
			}
		} else {
			corner = new FlatCorner(
				beginCorner, leftCorner, rightCorner,
				begin, left, right
			);
		}

		return corner;
	}

	protected static void TriangulateBoundaryTriangle(
		HexMesh mesh,
		Vector3 beginCorner, HexCell beginCell,
		Vector3 leftCorner, HexCell leftCell, 
		Vector3 boundary, Color boundaryColor
	) {
		Vector3 v1 = beginCorner;
		Color c1 = beginCell.Color;

		for (int step = 1; step < HexMetrics.terraceSteps + 1; ++step) {
			Vector3 v3 = HexMetrics.TerraceLerp(beginCorner, leftCorner, step);

			Color c3 = HexMetrics.ColorLerp(beginCell.Color, leftCell.Color, step);

			mesh.AddTriangle(HexMetrics.Perturb(v1), HexMetrics.Perturb(v3), boundary, false);
			mesh.AddTriangleColor(c1, c3, boundaryColor);

			v1 = v3;
			c1 = c3;
		}
	}
}
