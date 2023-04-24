using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Camera main;
    public Tilemap tilemap;
    public Vector3 offset = new Vector3 (0, 0.3f, 0);
    public TileBase originTile;
    public TileBase destinationTile;
    public DeTinMarin floodFill;

    private Dictionary<Tilemap, Vector3Int> _previousPosition = new Dictionary<Tilemap, Vector3Int>();
    private Dictionary<Tilemap, Vector3Int> _origin = new Dictionary<Tilemap, Vector3Int>();
    private Dictionary<Tilemap, Vector3Int> _goal = new Dictionary<Tilemap, Vector3Int>();

    //Cambiar Mode de TileMap y progect settings cambiar graphics a 0,1,-1

    private void Start()
    {
        _previousPosition[tilemap] = new Vector3Int(-1, -1, 0);
    }

    private void Update()
    {
        SelecTile();

        if(Input.GetMouseButtonDown(0)) 
        {
            DetectTileClick(isOrigin:true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            DetectTileClick(isOrigin:false);
        }

        if(Input.GetKeyDown(KeyCode.A)) 
        {
            FloodFill();
        }
    }

    private void SelecTile()
    {
        Vector3 mousePosition = main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePosition = tilemap.WorldToCell(mousePosition);
        tilePosition.z = 0;
        
        if (tilemap.HasTile(tilePosition)) 
        {
            tilemap.SetTransformMatrix(tilePosition, Matrix4x4.TRS(offset, Quaternion.Euler(0, 0, 0), Vector3.one));
            tilemap.SetTransformMatrix(_previousPosition[tilemap], Matrix4x4.identity);

            _previousPosition[tilemap] = tilePosition;

        }

    }

    private void DetectTileClick(bool isOrigin)
    {
        Vector3 mousePosition = main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePosition = tilemap.WorldToCell(mousePosition);
        tilePosition.z = 0;

        TileBase newTile = isOrigin ? originTile : destinationTile;
        Dictionary<Tilemap, Vector3Int> selectedDictionary = isOrigin ? _origin : _goal;


        if (tilemap.HasTile(tilePosition))
        {
            var oldTile = tilemap.GetTile(tilePosition);
            tilemap.SetTile(tilePosition, newTile);

            if (selectedDictionary.ContainsKey(tilemap))
            {
                tilemap.SetTile(selectedDictionary[tilemap], oldTile);
            }

           
            selectedDictionary[tilemap] = tilePosition;
        }

    }

    private void FloodFill()
    {
        floodFill.Origin = _origin[tilemap];
        floodFill.Goal = _goal[tilemap];
        floodFill.visitedTile = originTile;
        floodFill.tileMap = tilemap;
        floodFill.pathTile = destinationTile;

        StartCoroutine(floodFill.FloodFill2D());
    }
}
