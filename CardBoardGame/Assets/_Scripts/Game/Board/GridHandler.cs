using System;
using UnityEngine;

[Serializable]
public struct GridData
{
    public enum GridType
    {
        Start = 0,
        Day = 1,
        Night = 2,
        PlayerHeal = 3,
        MonsterHeal = 4,
        Draw = 5,
        MiniGame = 6
    }
    [SerializeField]
    public GridType gridType;
}

public class GridHandler : MonoBehaviour
{
    [SerializeField] private Grid[] grid;
    [SerializeField] private GridData[] gridData;
    private void Awake()
    {
        InitializeGridData();
    }

    private void InitializeGridData()
    {
        for (int i = 0; i < grid.Length; i++)
        {

        }
    }
}
