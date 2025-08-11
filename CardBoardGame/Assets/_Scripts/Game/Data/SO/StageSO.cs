using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
[CreateAssetMenu(fileName = "StageScriptableObject", menuName = "Scriptable Objects/StageScriptableObject")]
public class StageSO : ScriptableObject
{

    [Serializable]
    public struct GridInfo
    {
        public GridType gridType;
        public string title;

        [TextArea(minLines: 2, maxLines: 10)]
        public string info;
    }
    public GridInfo[] gridInfoByType;

    private Dictionary<GridType, (string, string)> gridInfoMap;

    [SerializeField]
    private MonsterSO[] monsterSO;

    public MonsterSO[] MonsterSO => monsterSO;

    [SerializeField]
    private GridSO[] gridSO;

    [SerializeField]
    private RankPerDamageSO rankPerDamageSO;
    public RankPerDamageSO RankPerDamageSO => rankPerDamageSO;

    [SerializeField]
    private ElementEffectSO elementEffectSO;
    public ElementEffectSO ElementEffectSO => elementEffectSO;

    public int GridDataLength => gridSO[0].GridDataLength;

    /// <summary>
    /// 현재 스테이지에 따른 보드게임 그리드 데이터를 반환합니다.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public GridData[] GetGridDatas(int stage)
    {
        if (stage < 0 || stage >= gridSO.Length)
        {
            Debug.LogError("Invalid stage index: " + stage);
            return null;
        }

        InitGridInfo(gridSO[stage]);

        return gridSO[stage].GridDataArray;
    }

    public int GetGridLength(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Easy:
            case Difficulty.Normal:
            case Difficulty.Hard:
                return gridSO[(int)diff - 1].GridDataLength;
            default:
                throw new ArgumentOutOfRangeException($"확인되지 않은 난이도 {diff}");

        }
    }

    public void InitGridInfo(GridSO curGridSO)
    {
        if (gridInfoMap == null)
        {
            gridInfoMap = new Dictionary<GridType, (string, string)>();
            foreach (GridInfo gridInfo in gridInfoByType)
            {
                if (!gridInfoMap.ContainsKey(gridInfo.gridType))
                {
                    gridInfoMap.Add(gridInfo.gridType, (gridInfo.title, gridInfo.info));
                }
            }
        }

        foreach (GridData gridData in curGridSO.GridDataArray)
        {
            gridData.Title = gridInfoMap[gridData.gridType].Item1;
            gridData.Info = gridInfoMap[gridData.gridType].Item2;
        }

    }
}
