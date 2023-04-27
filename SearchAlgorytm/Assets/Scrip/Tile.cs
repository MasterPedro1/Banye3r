using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public float cellSize;
    public FloofFill sr;
    [ HideInInspector]
    public Vector3Int cellPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
    }
}
