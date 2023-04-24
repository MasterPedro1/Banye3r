using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public float cellSize;
    public DeTinMarin sr;
    [ HideInInspector]
    public Vector3Int cellPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0f));
            Vector3 worldPosition = grid.CellToWorld(cellPosition) + new Vector3(cellSize / 2f, cellSize / 2f, 0f);
            Debug.Log("Clicked on cell " + cellPosition + " at position " + worldPosition);
            sr._origin = cellPosition;
            sr.StartCoroutine(sr.FloodFill2D());
        }
    }
}
