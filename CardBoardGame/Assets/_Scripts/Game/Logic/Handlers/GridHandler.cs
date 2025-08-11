using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
[Serializable]
public class GridData
{
    [SerializeField]
    public GridType gridType;
    // 런타임 때 필요한 데이터
    private int idx;
    public int Idx { get => idx; set => idx = value; }
    [SerializeField]
    private string title;
    public string Title
    {
        get => title;

        set => title = value;
    }
    [SerializeField]
    private string info;
    public string Info
    {
        get => info;
        set => info = value;
    }
}

public class GridHandler : Handler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private BoardGrid[] grid;
    [SerializeField] private GridData[] gridData;
    [SerializeField] private BoardGrid gridPrefab;
    [SerializeField] private BoardEffectInfo boardEffectInfo;
    public RectTransform canvasRt;
    private GridData curGridData = new GridData();
    public GridData CurGridData => curGridData;
    [SerializeField]
    private TileGenerator tileGenerator;

    public List<HexTile> PathTiles => tileGenerator.pathList;

    #region MarbleUI_On
    [SerializeField] private GameObject marbleUI_On;
    [SerializeField] private BoardGrid[] onGrids;
    private bool isGridPopUpOn = false;
    public bool IsGridPopUpOn
    {
        get => isGridPopUpOn;
        set
        {
            isGridPopUpOn = value;
            ManagerHandler.Instance.gameManager.PopUpActivation(isGridPopUpOn);
        }
    }
    private bool isMarbleUIOn = false;

    public bool IsMarbleUIOn
    {
        get => isMarbleUIOn;
        set
        {
            isMarbleUIOn = value;
            marbleUI_On.SetActive(isMarbleUIOn);
        }

    }
    private IEnumerator gridFollowEnumerator;
    #endregion

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
            grid[i].GridData.Idx = i;
            onGrids[i].GridData = grid[i].GridData;
        }
        tileGenerator.GenerateTileArea(Vector2Int.zero);
        int deActiveCount = tileGenerator.pathCount - grid.Length;
        if (deActiveCount > 0)
        {
            tileGenerator.DeactivateTilesExactly(deActiveCount);
        }
        boardEffectInfo.gridData = grid[0].GridData;
    }

    public void GetCurrentGridData(int idx)
    {
        curGridData = grid[idx].GridData;
        boardEffectInfo.sprite = grid[idx].gridSprite;
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

    public void GridPopUp(string title, string infos, Sprite sprite)
    {
        ManagerHandler.Instance.gameManager.SetPopUpInfos(title, infos, sprite, new Vector2(0, 1));

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.GridInfosUIActive(true);
        gridFollowEnumerator = ManagerHandler.Instance.gameManager.PopUpFollowMousePoint();
        StartCoroutine(gridFollowEnumerator);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.GridInfosUIActive(false);
        StopCoroutine(gridFollowEnumerator);
    }

}

