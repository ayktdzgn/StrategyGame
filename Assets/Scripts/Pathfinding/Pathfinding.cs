using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using UnityEngine;

namespace Core.Pathfinding
{
public class Pathfinding:Singleton<Pathfinding>
{
    private GenericGrid<Tile> grid;
    private List<Tile> openList;
    private List<Tile> closedList;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Pathfinding(int width, int height, float cellSize, Vector3 originPosition)
    {
        Instance = this;
        //initialize the grid with the given parametres from grid manager and with the tilemapobject type
        grid = new GenericGrid<Tile>(width, height, cellSize, originPosition, (GenericGrid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    private Tile GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<Tile> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Tile pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.GetX(), pathNode.GetY()) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<Tile> FindPath(int startX, int startY, int endX, int endY)
    {
        Tile startNode = grid.GetGridObject(startX, startY);
        Tile endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }


        var neighbourList = GetNeighbourList(endNode);
        int i = 0;
        while (endNode.isOccupiedByUnit || !endNode.isWalkable)
        {
            foreach (var neighbour in neighbourList)
            {
                if (!neighbour.isOccupiedByUnit || !endNode.isWalkable)
                {
                    endNode = neighbour;
                    break;
                }
            }
            if (endNode.isOccupiedByUnit || !endNode.isWalkable && i <= (neighbourList.Count - 1))
            {
                endNode = neighbourList[i];
                i++;
            }
            if (i >= (neighbourList.Count - 1))
            {
                neighbourList = GetNeighbourList(neighbourList[i]);
                i = 0;
            }
        }

        startNode.isOccupiedByUnit = false;

        openList = new List<Tile> { startNode };
        closedList = new List<Tile>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Tile tile = grid.GetGridObject(x, y);
                tile.gCost = 99999999;
                tile.CalculateFCost();
                tile.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        //search until there is no nodes in openlist
        while (openList.Count > 0)
        {
            Tile currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //calculate path is called when you reach the end node
                currentNode.isOccupiedByUnit = true;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Tile neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                //automatically add unwalkable nodes to closed list
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                //temp G cost is assigned to neighbour nodes
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                //if the temp g is lower than assigned g cost assigns the new g cost
                if (tentativeGCost < neighbourNode.gCost)
                {
                    //assigns the previous node to next node so we can reverse track while calculating path
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost(); ;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //Out of nodes on the openList
        return null;
    }

    //gets the neighbours of a given current node
    //with current node in the middle and 3x3 square neighbours around the current node
    private List<Tile> GetNeighbourList(Tile currentNode)
    {
        List<Tile> neighbourList = new List<Tile>();

        if (currentNode.GetX() - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode.GetX() - 1, currentNode.GetY()));
            //Left Down
            if (currentNode.GetY() - 1 >= 0) neighbourList.Add(GetNode(currentNode.GetX() - 1, currentNode.GetY() - 1));
            //Left Up
            if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.GetX() - 1, currentNode.GetY() + 1));
        }
        if (currentNode.GetX() + 1 < grid.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(currentNode.GetX() + 1, currentNode.GetY()));
            //Right Down
            if (currentNode.GetY() - 1 >= 0) neighbourList.Add(GetNode(currentNode.GetX() + 1, currentNode.GetY() - 1));
            //Right Up
            if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.GetX() + 1, currentNode.GetY() + 1));
        }
        //Down
        if (currentNode.GetY() - 1 >= 0) neighbourList.Add(GetNode(currentNode.GetX(), currentNode.GetY() - 1));
        //Up
        if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.GetX(), currentNode.GetY() + 1));

        return neighbourList;
    }


    private List<Tile> CalculatePath(Tile endNode)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endNode);
        Tile currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private Tile GetLowestFCostNode(List<Tile> pathNodeList)
    {
        Tile lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private int CalculateDistanceCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.GetX() - b.GetX());
        int yDistance = Mathf.Abs(a.GetY() - b.GetY());
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}
}