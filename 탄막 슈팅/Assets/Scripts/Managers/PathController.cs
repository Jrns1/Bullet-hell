using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathController : Singleton<PathController> {

    public Tilemap tilemap;
    public Tile wall;

    const string DUSTNAME = "Dust";

    public void Shut(Vector3Int[] locations)
    {
        foreach (Vector3Int location in locations)
        {
            tilemap.SetTile(location, wall);
            GameManager.Ins.Particle(DUSTNAME, (Vector3)location);
        }
    }

    public void Free(Vector3Int[] locations)
    {
        foreach (Vector3Int location in locations)
        {
            tilemap.SetTile(location, null);
            GameManager.Ins.Particle(DUSTNAME, (Vector3)location);
        }
    }
}
