using UnityEngine;

public class Node
{
    public bool Walkable { get; }
    public Vector3 WorldPosition { get; }

    public Node(bool walkable, Vector3 worldPosition)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
    }
}