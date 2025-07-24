using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Transform centerPos;
    public Transform[] topPos;
    public Transform[] middlePos;
    public Transform[] bottomPos;

    public int q, r;

    private static readonly (int dq, int dr)[] axialOffsets = new (int, int)[6]
    {
        (1, 0),     // 0: 동
        (1, -1),    // 1: 남동
        (0, -1),    // 2: 남서
        (-1, 0),    // 3: 서
        (-1, 1),    // 4: 북서
        (0, 1)      // 5: 북동
    };

    // /// <summary>
    // /// 모든 이웃 방향의 좌표를 반환
    // /// </summary>
    // public Vector2Int[] GetAllNeighborCoords()
    // {
    //     Vector2Int[] neighbors = new Vector2Int[6];
    //     for (int i = 0; i < 6; i++)
    //     {
    //         neighbors[i] = GetNeighborCoord(i);
    //     }
    //     return neighbors;
    // }

    public void SetCoords(int q, int r)
    {
        this.q = q;
        this.r = r;
        gameObject.name = $"Tile ({q}, {r})";
    }

    public Vector2Int GetNeighborCoord(int direction)
    {
        // 육각형 방향 오프셋 (축 기준 방향)
        var axialOffsets = new (int dq, int dr)[]
        {
        (1, 0),     // 0: 오른쪽
        (1, -1),    // 1: 오른쪽 아래
        (0, -1),    // 2: 아래
        (-1, 0),    // 3: 왼쪽
        (-1, 1),    // 4: 왼쪽 위
        (0, 1)      // 5: 위
        };
        direction = direction % 6;
        var (dq, dr) = axialOffsets[direction];
        return new Vector2Int(q + dq, r + dr);
    }

}
