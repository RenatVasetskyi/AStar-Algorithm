using System;
using Interfaces;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int GCost;
    public int HCost;

    public Node Parent;
    
    public int HeapIndex { get; set; }
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

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
            compare = HCost.CompareTo(nodeToCompare.HCost);

        return -compare;
    }
}