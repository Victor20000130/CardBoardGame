using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEditor;
using UnityEngine;

public class MapHandler : MonoBehaviour
{

    [SerializeField]
    private HexTile hexTilePref;
    private Dictionary<Vector2Int, HexTile> hexTiles = new();

    private List<HexTile> path = new List<HexTile>();

    private List<HexTile> corner = new List<HexTile>();

    public int radius = 3;

    void Start()
    {
        StartCoroutine(GenerateTileArea(Vector2Int.zero, radius));
    }

    public IEnumerator GenerateTileArea(Vector2Int centerCoord, int maxRadius)
    {
        Queue<Vector2Int> toVisit = new();
        HashSet<Vector2Int> visited = new();

        // 중심 타일 생성
        HexTile centerTile = Instantiate(hexTilePref, transform.position, Quaternion.identity, transform);
        centerTile.SetCoords(centerCoord);
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
                baseTile.SetColor(Color.green);
                for (int dir = 0; dir < 6; dir++)
                {
                    Vector2Int neighborCoord = baseTile.GetNeighborCoord(dir);

                    if (visited.Contains(neighborCoord))
                    {
                        if (visited.TryGetValue(neighborCoord, out Vector2Int key))
                        {
                            baseTile.neighbors.Add(hexTiles[key]);
                        }
                        continue;
                    }

                    Transform middlePoint = baseTile.middlePos[dir];

                    float distance = Vector3.Distance(middlePoint.position, baseTile.transform.position);

                    Vector3 direction = (middlePoint.position - baseTile.transform.position).normalized;

                    Vector3 spawnPos = middlePoint.position + direction * distance;

                    HexTile newTile = Instantiate(hexTilePref, spawnPos, Quaternion.identity, transform);

                    yield return null;

                    baseTile.neighbors.Add(newTile);

                    newTile.SetColor(Color.blue);

                    // print($"baseTile: {baseTile.coord}, currentTileCoord: {current}, dir: {dir}, neighborCoord: {neighborCoord}");

                    // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                    newTile.SetCoords(neighborCoord);
                    hexTiles.Add(neighborCoord, newTile);

                    toVisit.Enqueue(neighborCoord);
                    visited.Add(neighborCoord);

                    newTile.SetColor(Color.white);
                }
                baseTile.SetColor(Color.gray);

            }
        }

    }

    private void SetStartEndCoord()
    {
        int half = radius / 2;

    }

}

