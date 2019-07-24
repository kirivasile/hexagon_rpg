using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;

	Color color;
	int elevation = int.MinValue;

	public RectTransform uiRect;

	public HexGridChunk chunk;

	HexEdge[] edges;
	Corner[] corners;

	void Awake() {
		edges = new HexEdge[3];
		corners = new Corner[2];
	}

	public void Triangulate(HexMesh mesh) {
		for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++) {
			Vector3 center = Position;
			EdgeVertices edge = new EdgeVertices(
				Position + HexMetrics.GetFirstSolidCorner(direction),
				Position + HexMetrics.GetSecondSolidCorner(direction)
			);

			mesh.AddTriangle(Position, edge.v1, edge.v2);
			mesh.AddTriangleColor(Color, Color, Color);
			mesh.AddTriangle(Position, edge.v2, edge.v3);
			mesh.AddTriangleColor(Color, Color, Color);
			mesh.AddTriangle(Position, edge.v3, edge.v4);
			mesh.AddTriangleColor(Color, Color, Color);
			mesh.AddTriangle(Position, edge.v4, edge.v5);
			mesh.AddTriangleColor(Color, Color, Color);

			HexCell neighbor = GetNeighbor(direction);
			if (direction <= HexDirection.SE && neighbor != null) {
				edges[(int)direction] = HexEdge.Build(this, neighbor, direction);
				edges[(int)direction].Triangulate(mesh);

				HexCell nextNeighbor = GetNeighbor(direction.Next());
				if (direction <= HexDirection.E && nextNeighbor != null) {
					corners[(int)direction] = Corner.Build(this, neighbor, nextNeighbor, direction);
					corners[(int)direction].Triangulate(mesh);
				}
			}
		}
	}

	public int Elevation {
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y +=
					(HexMetrics.SampleNoise(position).y * 2f - 1f) *
					HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;

			Refresh();
		}
	}

	public Color Color {
		get {
			return color;
		}
		set {
			if (color == value) {
				return;
			}
			color = value;
			Refresh();
		}
	}

	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}

	[SerializeField]
	HexCell[] neighbors;

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	void Refresh() {
		if (chunk) {
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; ++i) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk) {
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	void RefreshSelfOnly() {
		chunk.Refresh();
	}
}
