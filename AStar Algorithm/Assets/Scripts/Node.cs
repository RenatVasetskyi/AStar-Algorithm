using UnityEngine;

public class Node
{
    public int GCost;
    public int HCost;

    public Node Parent;
    
    public bool Walkable { get; }
    public Vector3 WorldPosition { get; }
    public int GridX { get; }
    public int GridY { get; }
    public int FCost => GCost + HCost;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }
}