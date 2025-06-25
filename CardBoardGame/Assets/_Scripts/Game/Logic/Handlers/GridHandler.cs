using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
[Serializable]
public class GridData
{

    [SerializeField]
    public GridType gridType;

    // 런타임 때 필요한 데이터
    private int idx;
    public int Idx { get => idx; set => idx = value; }
}

public class GridHandler : Handler
{
    [SerializeField] private Grid[] grid;
    [SerializeField] private GridData[] gridData;
    [SerializeField] private Grid gridPrefab;
    [SerializeField] private Image boardEffect_IMG;
    private GridData curGridData = new GridData();
    public GridData CurGridData => curGridData;

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

    // 그리드가 동적으로 변할 경우 사용
    // private void InstantiateGrids(int length)
    // {
    //     int gridLength = length;
    //     grid = new Grid[gridLength];
    //     for (int i = 0; i < gridLength; i++)
    //     {
    //         grid[i] = Instantiate(gridPrefab, transform, false);
    //     }
    // }

    public void GetCurrentGridData(int idx)
    {
        boardEffect_IMG.sprite = grid[idx].gridSprite;
        curGridData.gridType = grid[idx].GridData.gridType;
        curGridData.Idx = idx;
        ManagerHandler.Instance.gameManager.ReceiveGridData(curGridData);
    }

    protected override void OnInitialize()
    {
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.GridHandler;
    }
}