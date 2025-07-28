using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Transform[] middlePos;

    public Vector2Int coord;

    public Dictionary<Vector2Int, HexTile> neighbors = new Dictionary<Vector2Int, HexTile>();
    public Renderer rend;
    public List<HexTile> neighborList = new List<HexTile>();
    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
    /// <summary>
    /// 모든 이웃 방향의 좌표를 반환
    /// </summary>
    public Vector2Int[] GetAllNeighborCoords()
    {
        Vector2Int[] neighbors = new Vector2Int[6];
        for (int i = 0; i < 6; i++)
        {
            neighbors[i] = GetNeighborCoord(i);
        }
        return neighbors;
    }

    public void SetCoords(Vector2Int coord)
    {
        this.coord = coord;
        gameObject.name = $"Tile ({coord.x}, {coord.y})";
    }

    public Vector2Int GetNeighborCoord(int direction)
    {
        // 육각형 방향 오프셋 (축 기준 방향)
        var axialOffsets = new (int dq, int dr)[]
        {
        (1, 0),     // 0: 오른쪽
        (0, 1),    // 1: 오른쪽 아래
        (-1, 1),    // 2: 왼쪽 아래
        (-1, 0),    // 3: 왼쪽
        (0, -1),    // 4: 왼쪽 위
        (1, -1)      // 5: 오른쪽 위
        };
        direction %= 6;
        var (dq, dr) = axialOffsets[direction];
        return new Vector2Int(coord.x + dq, coord.y + dr);
    }
    public void ActivationTile(float valueY)
    {
        transform.position += new Vector3(0, valueY, 0);
    }

    public void DeActivatonTile(float valueY)
    {
        transform.position -= new Vector3(0, valueY, 0);
    }

}
