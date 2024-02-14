using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _unwalkableLayer;
    
    [SerializeField] private float _distanceBetweenNodes;
    [SerializeField] private float _nodeDiameter;
    
    [SerializeField] private bool _displayPathGizmos;
    
    private List<Node> _path;
    
    private Node[,] _grid;

    private int _gridSizeX;
    private int _gridSizeY;
    
    private float _nodeRadius;

    public int MaxSize => _gridSizeX * _gridSizeY;

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _size.x / 2) / _size.x;
        float percentY = (worldPosition.z + _size.y / 2) / _size.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        
        return _grid[x, y]; 
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _gridSizeX || checkY >= 0 && checkY < _gridSizeY)
                    neighbours.Add(_grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    public void SetPath(List<Node> path)
    {
        _path = path;
    }

    private void Start()
    {
        SetGridSize();
        CreateGrid();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_size.x, 1, _size.y));

        if (_displayPathGizmos)
        {
            if (_path != null)
            {
                foreach (Node node in _path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));
                }
            }
        }
        else
        {
            if (_grid != null)
            {
                foreach (Node node in _grid)
                {
                    Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                
                    if (_path != null)
                    {
                        if (_path.Contains(node))
                            Gizmos.color = Color.black;
                    }
                
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));
                }
            }   
        }
    }

    private void SetGridSize()
    {
        _nodeRadius = _nodeDiameter / 2;
        
        _gridSizeX = Mathf.RoundToInt(_size.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_size.y / _nodeDiameter);
    }

    private void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];

        Vector3 bottomLeft = transform.position - Vector3.right * _gridSizeX / 2 - Vector3.forward * _gridSizeY / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3
                    .forward * (y * _nodeDiameter + _nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableLayer);

                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
}