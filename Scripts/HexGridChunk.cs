using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridChunk : MonoBehaviour {
	HexCell[] cells;

	public HexMesh terrain;
	Canvas gridCanvas;

	void Awake() {
		gridCanvas = GetComponentInChildren<Canvas>();
		terrain = GetComponentInChildren<HexMesh>();

		cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];

		ShowUI(false);
	}

	public void AddCell(int index, HexCell cell) {
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}

	public void Refresh() {
		enabled = true;
	}

	public void Triangulate() {
		terrain.Clear();

		for (int i = 0; i < cells.Length; ++i) {
			cells[i].Triangulate(terrain);
		}

		terrain.Apply();
	}

	void LateUpdate() {
		Triangulate();
		enabled = false;
	}

	public void ShowUI (bool visible) {
		gridCanvas.gameObject.SetActive(visible);
	}
}
