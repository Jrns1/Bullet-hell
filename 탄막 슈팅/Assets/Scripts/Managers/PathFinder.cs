using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : Singleton<PathFinder> 
{
    public TileBase[] unwalkable;

    public Tilemap[] tilemaps;
    
    HeapNode[,] map;
    Vector3Int bottomLeft;
    int gridSizeX, gridSizeY;
    //Rect mapRect;

    public bool isMapValid = false;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void SetMap()
    {
        MapManager mapManager = MapManager.Instance;
        bottomLeft = Vector3Int.RoundToInt(mapManager.regions[mapManager.currentRegionNum].lowerLeft);
        Vector3Int topRight = Vector3Int.RoundToInt(mapManager.regions[mapManager.currentRegionNum].upperRight);

        gridSizeX = topRight.x - bottomLeft.x;
        gridSizeY = topRight.y - bottomLeft.y;

        map = new HeapNode[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3Int position = bottomLeft;
                position.x += x;
                position.y += y;

                foreach (Tilemap tMap in tilemaps)
                {
                    TileBase tile = tMap.GetTile<TileBase>(position);
                    bool walkable = IsWalkable(tile);
                    map[x, y] = new HeapNode(walkable, position, position - bottomLeft);
                    if (!walkable)
                    {
                        break;
                    }
                }

            }
        }

        isMapValid = true;
        //mapRect = new Rect(bottomLeft.x, topRight.y, gridSizeX, gridSizeY);
        //LogMap();
    }

    public IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        if (map == null || !isMapValid)
        {
            PathRequestManager.Instance.FinishedProcessingPath(null, false);
            yield break;
        }

        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        HeapNode startNode = GetNodeInWorldPos(startPos);
        HeapNode targetNode = GetNodeInWorldPos(targetPos);
        if (startNode == null || targetNode == null)
        {
            PathRequestManager.Instance.FinishedProcessingPath(null, false);
            yield break;
        }

        if (startNode != targetNode)
        {
            Heap<HeapNode> openSet = new Heap<HeapNode>(MaxSize);
            HashSet<HeapNode> closedSet = new HashSet<HeapNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                HeapNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (HeapNode neighbour in GetNeighbours(currentNode))
                {

                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        PathRequestManager.Instance.FinishedProcessingPath(waypoints, pathSuccess);
    }

    HeapNode GetNodeInWorldPos(Vector3 worldPos)
    {
        Vector3Int cellPos = tilemaps[0].WorldToCell(worldPos);
        //if (!mapRect.Contains(cellPos))
            //return null;

        cellPos -= bottomLeft;
        return map[cellPos.x, cellPos.y];
    }

    bool IsWalkable(TileBase tile)
    {
        if (!tile)
        {
            return true;
        }

        for (int i = 0; i < unwalkable.Length; i++)
        {
            if (tile == unwalkable[i])
                return false;
        }

        return true;
    }

    public List<HeapNode> GetNeighbours(HeapNode node)
    {
        List<HeapNode> neighbours = new List<HeapNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                // 1 0 0
                // 1 n 0
                // 1 1 1
                if (!(map[node.gridIndex.x + x, node.gridIndex.y].walkable &&
                    map[node.gridIndex.x, node.gridIndex.y + y].walkable))
                {
                    continue;
                }
                int checkX = node.gridIndex.x + x;
                int checkY = node.gridIndex.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(map[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    int GetDistance(HeapNode nodeA, HeapNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridIndex.x - nodeB.gridIndex.x);
        int dstY = Mathf.Abs(nodeA.gridIndex.y - nodeB.gridIndex.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    Vector2[] RetracePath(HeapNode startNode, HeapNode endNode)
    {
        List<HeapNode> path = new List<HeapNode>();
        HeapNode currentNode = endNode;
        while (currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        
    }

    Vector2[] SimplifyPath(List<HeapNode> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i].gridIndex.x - path[i - 1].gridIndex.x, path[i].gridIndex.y - path[i - 1].gridIndex.y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].position + new Vector3(.5f, .5f, 0)); 
                directionOld = directionNew;
            }
        }
        return waypoints.ToArray();
    }

    ///*
    void LogMap()
    {
        string mapString = "";
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                mapString += map[x, y].walkable?"1 ":"0 ";
            }
            mapString += "\n";
        }
        print(mapString);
    }
    //*/
}

