using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
[Serializable]
public class GridData
{

    [SerializeField]
    public GridType gridType;
}

public class GridHandler : MonoBehaviour
{
    [SerializeField] private Grid[] grid;
    [SerializeField] private GridData[] gridData;

    public void InitializeGridData(MonsterGridSO monsterGridScriptableObject)
    {
        gridData = monsterGridScriptableObject.GetGridDatas(0);
        if (grid.Length != gridData.Length)
        {
            Debug.LogError("Grid and GridData arrays must have the same length.");
            return;
        }

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].GridData = gridData[i];
            // Additional initialization logic can be added here if needed
        }
    }
}