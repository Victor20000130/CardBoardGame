using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    [SerializeField]
    private HexTile hexTilePref;
    private Dictionary<Vector2Int, HexTile> hexTiles = new();

    private Dictionary<Vector2Int, HexTile> path = new Dictionary<Vector2Int, HexTile>();
    public float pathTilesYvalue = 15;
    public int pathCount => path.Count;
    public int radius = 3;
    public List<HexTile> pathList = new List<HexTile>();

    private static readonly Vector2Int[] hexDirections = new Vector2Int[]
    {
    new Vector2Int(1, 0),     // 동
    new Vector2Int(0, 1),     // 북동
    new Vector2Int(-1, 1),    // 북서
    new Vector2Int(-1, 0),    // 서
    new Vector2Int(0, -1),    // 남서
    new Vector2Int(1, -1)     // 남동
    };

    public void GenerateTileArea(Vector2Int centerCoord)
    {
        Queue<Vector2Int> toVisit = new();
        HashSet<Vector2Int> visited = new();

        // 중심 타일 생성
        HexTile centerTile = Instantiate(hexTilePref, transform.position, Quaternion.identity, transform);
        centerTile.SetCoords(centerCoord);
        hexTiles.Add(centerCoord, centerTile);

        toVisit.Enqueue(centerCoord);
        visited.Add(centerCoord);

        for (int step = 0; step < radius; step++)
        {
            int count = toVisit.Count;
            for (int i = 0; i < count; i++)
            {
                Vector2Int current = toVisit.Dequeue();
                HexTile baseTile = hexTiles[current];
                for (int dir = 0; dir < 6; dir++)
                {
                    Vector2Int neighborCoord = baseTile.GetNeighborCoord(dir);

                    if (visited.Contains(neighborCoord))
                    {
                        if (visited.TryGetValue(neighborCoord, out Vector2Int key))
                        {
                            baseTile.neighbors.Add(neighborCoord, hexTiles[key]);
                        }
                        continue;
                    }

                    Transform middlePoint = baseTile.middlePos[dir];

                    float distance = Vector3.Distance(middlePoint.position, baseTile.transform.position);

                    Vector3 direction = (middlePoint.position - baseTile.transform.position).normalized;

                    Vector3 spawnPos = middlePoint.position + direction * distance;

                    HexTile newTile = Instantiate(hexTilePref, spawnPos, Quaternion.identity, transform);
                    baseTile.neighbors.Add(neighborCoord, newTile);

                    newTile.SetCoords(neighborCoord);
                    hexTiles.Add(neighborCoord, newTile);

                    toVisit.Enqueue(neighborCoord);
                    visited.Add(neighborCoord);

                }

            }
        }
        AddPathTiles();
    }

    private void AddPathTiles()
    {
        int halfRadi = radius / 2;

        Vector2Int currentCoord = halfRadi * hexDirections[4];

        path.Add(currentCoord, hexTiles[currentCoord]);

        hexTiles[currentCoord].ActivationTile(pathTilesYvalue);

        pathList.Add(hexTiles[currentCoord]);

        for (int i = 0; i < 6; i++)
        {
            Vector2Int dir = hexDirections[i];

            for (int j = 0; j < halfRadi; j++)
            {
                Vector2Int nextCoord = currentCoord + dir;

                if (!path.ContainsKey(nextCoord))
                {
                    path.Add(nextCoord, hexTiles[nextCoord]);

                    hexTiles[nextCoord].ActivationTile(pathTilesYvalue);

                    pathList.Add(hexTiles[nextCoord]);

                    hexTiles[currentCoord].neighborList.Add(hexTiles[nextCoord]);
                    hexTiles[nextCoord].neighborList.Add(hexTiles[currentCoord]);

                }
                else
                {

                }

                currentCoord += dir;
            }
        }
        pathList[0].neighborList.Add(pathList[^1]);
        pathList[^1].neighborList.Add(pathList[0]);
    }

    [Obsolete]
    public void DeActivateTiles(int deActiveCount)
    {
        HashSet<HexTile> deActivedTiles = new();
        HashSet<Vector2Int> blockedCoords = new();

        // 1. 후보군 준비: 현재 pathList 전체 복사
        List<HexTile> candidates = new(pathList);

        // 2. 랜덤 셔플 (랜덤성 유지)
        candidates = candidates.OrderBy(_ => UnityEngine.Random.value).ToList();

        // 3. 후보에서 순차적으로 선택
        foreach (HexTile tile in candidates)
        {
            // 본인 또는 이웃이 이미 비활성 처리된 곳과 겹치면 패스
            if (blockedCoords.Contains(tile.coord) || tile.neighbors.Values.Any(n => blockedCoords.Contains(n.coord)))
                continue;

            // 현재 타일 비활성화 예약
            blockedCoords.Add(tile.coord);
            foreach (var neighbor in tile.neighbors.Values)
                blockedCoords.Add(neighbor.coord);

            deActivedTiles.Add(tile);

            tile.DeActivatonTile(pathTilesYvalue);
            pathList.Remove(tile);
            path.Remove(tile.coord);

            // 목표 수만큼 완료 시 종료
            if (deActivedTiles.Count >= deActiveCount)
                break;
        }

        // 4. 부족할 경우 경고
        if (deActivedTiles.Count < deActiveCount)
        {
            Debug.LogWarning($"{deActiveCount}개의 타일 중 {deActivedTiles.Count}개만 비활성화. 유효한 타일이 부족함.");
        }
    }

    public void DeactivateTilesExactly(int deActiveCount)
    {
        List<HexTile> candidates = new(pathList);
        Shuffle(candidates);
        List<HexTile> selected = new();
        if (TrySelectNonAdjacentTilesBiDirectional(candidates, selected, 0, deActiveCount))
        {

            foreach (HexTile tile in selected)
            {
                tile.DeActivatonTile(pathTilesYvalue);
                tile.SetColor(Color.red);
                path.Remove(tile.coord);
                pathList.Remove(tile);
            }

            Debug.Log($"성공적으로 {selected.Count}개 비활성화 완료");
        }
        else
        {
            Debug.LogWarning($"조건을 만족하며 {deActiveCount}개를 선택할 수 없음");
        }

    }
    private bool TrySelectNonAdjacentTilesBiDirectional(List<HexTile> candidates, List<HexTile> selected,
                                                        int startIndex, int targetCount)
    {
        if (selected.Count == targetCount)
            return true;
        for (int i = startIndex; i < candidates.Count; i++)
        {
            HexTile current = candidates[i];
            bool isAdjacent = false;
            foreach (var selectedTile in selected)
            {
                // 양방향 neighborList 검사
                if (selectedTile.neighborList.Contains(current) || current.neighborList.Contains(selectedTile))
                {
                    isAdjacent = true;
                    break;
                }
            }

            if (isAdjacent) continue;

            selected.Add(current);

            if (TrySelectNonAdjacentTilesBiDirectional(candidates, selected, i + 1, targetCount))
                return true;

            selected.RemoveAt(selected.Count - 1); // 백트래킹
        }

        return false;
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}

