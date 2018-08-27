using UnityEngine;

public class HeapNode : IHeapItem<HeapNode>
{
    public bool walkable;
    public Vector3Int position;
    public Vector3Int gridIndex;

    public int gCost;
    public int hCost;

    public HeapNode parent;

    int heapIndex;

    public HeapNode(bool _walkable, Vector3Int _pos, Vector3Int _index)
    {
        walkable = _walkable;
        position = _pos;
        gridIndex = _index;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(HeapNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}