using System;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
[CreateAssetMenu(fileName = "StageScriptableObject", menuName = "Scriptable Objects/StageScriptableObject")]
public class StageSO : ScriptableObject
{
    [SerializeField] private MonsterSO[] monsterSO;
    [SerializeField] private GridSO[] gridSO;
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
}
