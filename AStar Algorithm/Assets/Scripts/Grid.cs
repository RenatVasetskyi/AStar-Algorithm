using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _unwalkableLayer;
    
    [SerializeField] private float _distanceBetweenNodes;
    [SerializeField] private float _nodeDiameter;
    
    private Node[,] _grid;

    private int _gridSizeX;
    private int _gridSizeY;
    
    private float _nodeRadius;
    
    private void Start()
    {
        SetGridSize();
        CreateGrid();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_size.x, 1, _size.y));

        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));
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

        for (int i = 0; i < _gridSizeX; i++)
        {
            for (int j = 0; j < _gridSizeY; j++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * _nodeDiameter + _nodeRadius) + Vector3
                    .forward * (j * _nodeDiameter + _nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableLayer);

                _grid[i, j] = new Node(walkable, worldPoint);
            }
        }
    }

    private Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _size.x / 2) / _size.x;
        float percentY = (worldPosition.z + _size.y / 2) / _size.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
;    }
}