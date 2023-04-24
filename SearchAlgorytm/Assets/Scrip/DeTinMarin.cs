using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeTinMarin : MonoBehaviour
{
    private Queue<Vector3> _frontier = new Queue<Vector3>();
    private Dictionary<Vector3, Vector3> _came = new Dictionary<Vector3, Vector3>();
    public Vector3 _origin;
   
    public TileBase visitedTile;
    public TileBase pathTile;
    public float delay = 0.4f;
  
    public Vector3 Origin { get; set; }
    public Vector3 Goal { get; set; }
    public Tilemap tileMap  { get; set; }

    public bool earlyExit = false;

    public IEnumerator FloodFill2D()
    {
        _frontier.Enqueue(_origin);
        _came[_origin] = Vector3.zero;
       
        
        while (_frontier.Count > 0 && !earlyExit )
        {
            Vector3 current = _frontier.Dequeue();
            foreach (Vector3 next in GetNeighbours(current))
            {
                if (next == Goal) earlyExit = true; yield return null;
                if (!_came.ContainsKey(next))
                {
                    yield return new WaitForSeconds(delay);
                    _frontier.Enqueue(next);
                    _came[next] = current;
                }
            }
        }
        DrawPath(Goal);
    }

    public void DrawPath(Vector3 goal)
    {
        Vector3 current = goal;
        while(current != Origin)
        {
            Vector3Int currentInt = new Vector3Int((int) current.x, (int) current.y, (int)current.z);
            tileMap.SetTile(currentInt, pathTile);
            current = _came[current];

        }
    }
    List<Vector3> GetNeighbours(Vector3 current)
    {
        List<Vector3> neighbours = new List<Vector3>();
        ValidateCoord(current + Vector3.right, neighbours);
        ValidateCoord(current + Vector3.left, neighbours);
        ValidateCoord(current + Vector3.up, neighbours);
        ValidateCoord(current + Vector3.down, neighbours);
        return neighbours;
    }
    void ValidateCoord(Vector3 neighbour, List<Vector3> neighbours)
    {
        Vector3Int coordInt = new Vector3Int((int)neighbour.x, (int)neighbour.y, (int)neighbour.z);

        if (!tileMap.HasTile(coordInt)) return;
        if (!_frontier.Contains(coordInt))
        {
            TileFlags flags = tileMap.GetTileFlags(coordInt);
            neighbours.Add(neighbour);
            tileMap.SetTile(coordInt, visitedTile);
            tileMap.SetTileFlags(coordInt, flags);

        }
    }
}
