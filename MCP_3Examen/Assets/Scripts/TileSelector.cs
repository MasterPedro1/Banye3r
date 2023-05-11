using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;

public class TileSelector : MonoBehaviour
{
    [SerializeField] Camera main;
    [SerializeField] Grid grid;
    public Tilemap tileMap;
    public Tilemap infantery;
    public Tilemap tank;
    public Tilemap reconocimiento;
    public Tilemap Caballeria;
    public Vector3 offset = new Vector3(0f, 0.02f, 0f);
    public TileBase originTile, destinyTile;

    [HideInInspector]
    public TileBase tb;

    private Dictionary<Tilemap, Vector3Int> _previousPosition = new Dictionary<Tilemap, Vector3Int>();
    private Dictionary<Tilemap, Vector3Int> _origin = new Dictionary<Tilemap, Vector3Int>();
    private Dictionary<Tilemap, Vector3Int> _goal = new Dictionary<Tilemap, Vector3Int>();

    [SerializeField] Infantery infantery2;
    [SerializeField] Tank tank2;
    [SerializeField] Reconocimiento  reco;
    [SerializeField] Caballeria cab;

    bool _isPlayerSelected = false;
    Vector3Int tilePosition;

    private void Start()
    {
        _previousPosition[tileMap] = new Vector3Int(-1, -1, 0);
    }


    private void Update()
    {
        if (!infantery2.IsPlayerSelected) SelectTile();

        if (Input.GetMouseButtonDown(0))
        {
            if (infantery.HasTile(tilePosition))
            {
               
                infantery2.maxSteps = 30;
                DetectTileClick(isOrigin: true);
                infantery2.IsPlayerSelected = true;
                ShowMovementArea();
                DetectTileClick(isOrigin: false);
                 
            }
            
            if (tank.HasTile(tilePosition))
            {
               
                    tank2.maxSteps = 50;
                    DetectTileClick(isOrigin: true);
                    tank2.IsPlayerSelected = true;
                    ShowMovementArea();
                    DetectTileClick(isOrigin: false);
              
            }

            if (reconocimiento.HasTile(tilePosition))
            {

                reco.maxSteps = 60;
                DetectTileClick(isOrigin: true);
                reco.IsPlayerSelected = true;
                ShowMovementArea();
                DetectTileClick(isOrigin: false);

            }

            if (Caballeria.HasTile(tilePosition))
            {

                cab.maxSteps = 60;
                DetectTileClick(isOrigin: true);
                cab.IsPlayerSelected = true;
                ShowMovementArea();
                DetectTileClick(isOrigin: false);

            }
        }

        if (infantery2.IsPlayerSelected)
        {
            DetectTileClick(isOrigin: false);
            infantery2.DrawPath(tilePosition);
           
        }

        if (tank2.IsPlayerSelected)
        {
            DetectTileClick(isOrigin: false);
           
            tank2.DrawPath(tilePosition);
           
        }

        if (reco.IsPlayerSelected)
        {
            DetectTileClick(isOrigin: false);
            
            reco.DrawPath(tilePosition);
           
        }

        if (cab.IsPlayerSelected)
        {
            DetectTileClick(isOrigin: false);
           
            cab.DrawPath(tilePosition);
        }

        if (infantery2.IsPlayerSelected  && Input.GetKeyDown(KeyCode.Return))
        {
            DetectTileClick(isOrigin: false);
            infantery2.MovePlayer();
           
        }

        if (tank2.IsPlayerSelected && Input.GetKeyDown(KeyCode.Return))
        {
            DetectTileClick(isOrigin: false);
            tank2.MovePlayer();
            
        }

        if (reco.IsPlayerSelected && Input.GetKeyDown(KeyCode.Return))
        {
            DetectTileClick(isOrigin: false);
            reco.MovePlayer();
           
        }

        if (cab.IsPlayerSelected && Input.GetKeyDown(KeyCode.Return))
        {
            DetectTileClick(isOrigin: false);
            cab.MovePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            infantery2.IsPlayerSelected = false;
            infantery2.ClearTiles();

            tank2.IsPlayerSelected = false;
            tank2.ClearTiles();

            reco.IsPlayerSelected = false;
            reco.ClearTiles();

            cab.IsPlayerSelected = false;
            cab.ClearTiles();
        }

    }


    private void SelectTile()
    {
        Vector3 mousePosition = main.ScreenToWorldPoint(Input.mousePosition);
        tilePosition = tileMap.WorldToCell(mousePosition);
        tilePosition.z = 0;

        if (tilePosition != _previousPosition[tileMap])
        {
            if (tileMap.HasTile(tilePosition))
            {
                tileMap.SetTransformMatrix(tilePosition, Matrix4x4.TRS(offset, Quaternion.Euler(0, 0, 0), Vector3.one));
            }
            if (tileMap.HasTile(_previousPosition[tileMap]))
            {
                tileMap.SetTransformMatrix(_previousPosition[tileMap], Matrix4x4.identity);
            }
            _previousPosition[tileMap] = tilePosition;
        }
    }


    private void DetectTileClick(bool isOrigin)
    {
        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        tilePosition = tileMap.WorldToCell(mousePos);
        tilePosition.z = 0;

        TileBase newTile = isOrigin ? originTile : destinyTile;
        Dictionary<Tilemap, Vector3Int> selectedDictionary = isOrigin ? _origin : _goal;

        if (tileMap.HasTile(tilePosition))
        {
            var oldTile = tileMap.GetTile(tilePosition);
            selectedDictionary[tileMap] = tilePosition;
        }
    }

    private void ShowMovementArea()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0f));
        Vector3 worldPosition = grid.CellToWorld(cellPosition) + new Vector3(1 / 2f, 1 / 2f, 0f);
        Debug.Log("Clicked on cell " + cellPosition + " at position " + worldPosition);
        infantery2.Origin = cellPosition;
        infantery2.StartScan();

        tank2.Origin = cellPosition;
        tank2.StartScan();

        reco.Origin = cellPosition;
        reco.StartScan();

        cab.Origin = cellPosition;
        cab.StartScan();
    }
}
