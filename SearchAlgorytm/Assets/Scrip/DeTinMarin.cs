using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeTinMarin : MonoBehaviour
{
    private Queue<Vector3> _frontier = new Queue<Vector3>();
    private Dictionary<Vector3, Vector3> _came = new Dictionary<Vector3, Vector3>();
    public Vector3 _origin;
    public Tilemap tileMap;
    public TileBase visitedTile;
    public TileBase pathTile;
    public float delay = 0.4f;


    public IEnumerator FloodFill2D()
    {
        _frontier.Enqueue(_origin);
        _came[_origin] = Vector3.zero;

        while (_frontier.Count > 0)
        {
            Vector3 current = _frontier.Dequeue();
            foreach (Vector3 next in GetNeighbours(current))
            {
                if (!_came.ContainsKey(next))
                {
                    yield return new WaitForSeconds(delay);
                    _frontier.Enqueue(next);
                    _came[next] = current;
                }
            }
        }
        yield return _frontier;
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
