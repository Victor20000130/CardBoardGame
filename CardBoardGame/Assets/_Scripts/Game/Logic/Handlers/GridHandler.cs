using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class GridData
{
    [SerializeField]
    public GridType gridType;
    // 런타임 때 필요한 데이터
    private int idx;
    public int Idx { get => idx; set => idx = value; }
    private string info;
    public string Info
    {
        get => info;
        set => info = value;
    }
}

public class GridHandler : Handler
{

    [SerializeField] private BoardGrid[] grid;
    [SerializeField] private GridData[] gridData;
    [SerializeField] private BoardGrid gridPrefab;
    [SerializeField] private BoardEffectInfo boardEffectInfo;
    private GridData curGridData = new GridData();
    public GridData CurGridData => curGridData;
    [SerializeField]
    private TileGenerator tileGenerator;

    public List<HexTile> PathTiles => tileGenerator.pathList;

    public void InitializeGridData(StageSO monsterGridSO, Difficulty diff)
    {
        //TODO : 그리드 동적 생성 로직 작성 예정

        gridData = monsterGridSO.GetGridDatas(0);
        if (grid.Length != gridData.Length)
        {
            Debug.LogError("Grid and GridData arrays must have the same length.");
            return;
        }

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].GridData = gridData[i];
        }
        tileGenerator.GenerateTileArea(Vector2Int.zero);
        int deActiveCount = tileGenerator.pathCount - grid.Length;
        if (deActiveCount > 0)
        {
            tileGenerator.DeactivateTilesExactly(deActiveCount);
        }
    }

    public void GetCurrentGridData(int idx)
    {
        boardEffectInfo.sprite = grid[idx].gridSprite;
        curGridData.gridType = grid[idx].GridData.gridType;
        curGridData.Idx = idx;
        ManagerHandler.Instance.gameManager.ReceiveGridData(curGridData);

        boardEffectInfo.gridData = curGridData;
    }

    protected override void OnInitialize()
    {
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.GridHandler;
    }
}