using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField]
    private HexTile hexTilePref;
    private Dictionary<Vector2Int, HexTile> hexTiles = new();
    public int radius = 3;
    void Start()
    {
        GenerateTileArea(Vector2Int.zero, radius);
    }

    public void GenerateTileArea(Vector2Int centerCoord, int maxRadius)
    {
        Queue<Vector2Int> toVisit = new();
        HashSet<Vector2Int> visited = new();

        // 중심 타일 생성
        HexTile centerTile = Instantiate(hexTilePref, transform.position, Quaternion.identity, transform);
        centerTile.SetCoords(centerCoord.x, centerCoord.y);
        hexTiles.Add(centerCoord, centerTile);

        toVisit.Enqueue(centerCoord);
        visited.Add(centerCoord);

        for (int step = 0; step < maxRadius; step++)
        {
            int count = toVisit.Count;

            for (int i = 0; i < count; i++)
            {
                Vector2Int current = toVisit.Dequeue();
                HexTile baseTile = hexTiles[current];

                for (int dir = 0; dir < 6; dir++)
                {
                    Vector2Int neighborCoord = baseTile.GetNeighborCoord(dir);
                    if (visited.Contains(neighborCoord)) continue;

                    Transform middlePoint = baseTile.middlePos[dir];

                    float distance = Vector3.Distance(middlePoint.position, baseTile.centerPos.position);

                    Vector3 direction = (middlePoint.position - baseTile.centerPos.position).normalized;

                    Vector3 spawnPos = middlePoint.position + direction * distance;

                    HexTile newTile = Instantiate(hexTilePref, spawnPos, Quaternion.identity, transform);
                    newTile.SetCoords(neighborCoord.x, neighborCoord.y);
                    hexTiles.Add(neighborCoord, newTile);

                    toVisit.Enqueue(neighborCoord);
                    visited.Add(neighborCoord);
                }
            }
        }

    }
}

