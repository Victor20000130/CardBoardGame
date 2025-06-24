using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
[Serializable]
public class GridData
{

    [SerializeField]
    public GridType gridType;
}

public class GridHandler : Handler
{
    [SerializeField] private Grid[] grid;
    [SerializeField] private GridData[] gridData;
    [SerializeField] private Grid gridPrefab;
    [SerializeField] private Image boardEffect_IMG;
    public void InitializeGridData(StageSO monsterGridSO, Difficulty diff)
    {
        //TODO : 그리드 동적 생성 로직 작성 예정
        // InstantiateGrids(monsterGridSO.GetGridLength(diff));
        // grid = new Grid[monsterGridSO.GetGridLength(diff)];
        // gridData = new GridData[monsterGridSO.GetGridLength(diff)];

        gridData = monsterGridSO.GetGridDatas(0);
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

    private void InstantiateGrids(int length)
    {
        int gridLength = length;
        grid = new Grid[gridLength];
        for (int i = 0; i < gridLength; i++)
        {
            grid[i] = Instantiate(gridPrefab, transform, false);
        }
    }

    public void GetCurrentGridData(int idx)
    {
        boardEffect_IMG.sprite = grid[idx].gridSprite;
        // return gridData[idx];
    }

    public override void Initialize()
    {
        base.Initialize();
        handlerType = HandlerType.GridHandler;
    }
}