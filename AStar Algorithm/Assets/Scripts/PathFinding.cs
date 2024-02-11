using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    private void Update()
    {
        FindPath(_start.position, _end.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = _grid.GetNodeFromWorldPoint(startPosition);
        Node targetNode = _grid.GetNodeFromWorldPoint(targetPosition);

        List<Node> openSet = new();
        HashSet<Node> closedSet = new();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode
                        .FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);

                return;
            }

            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new();

        Node currentNode = endNode;
        
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            
            currentNode = currentNode.Parent;
        }
        
        path.Reverse();

        _grid.SetPath(path);
    }

    private int GetDistance(Node firstNode, Node secondNode)
    {
        int distanceX = Mathf.Abs(firstNode.GridX - secondNode.GridX);
        int distanceY = Mathf.Abs(firstNode.GridY - secondNode.GridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}