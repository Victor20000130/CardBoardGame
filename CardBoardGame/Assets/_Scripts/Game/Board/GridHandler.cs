using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
[Serializable]
public struct GridData
{

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
