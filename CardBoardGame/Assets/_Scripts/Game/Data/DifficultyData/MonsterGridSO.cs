using UnityEngine;
[CreateAssetMenu(fileName = "DifficultyScriptableObject", menuName = "Scriptable Objects/DifficultyScriptableObject")]
public class MonsterGridSO : ScriptableObject
{
    [SerializeField] private MonsterSO[] monsterScriptableObjects;
    [SerializeField] private GridSO[] gridScriptableObjects;

    /// <summary>
    /// 현재 스테이지에 따른 보드게임 그리드 데이터를 반환합니다.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public GridData[] GetGridDatas(int stage)
    {
        if (stage < 0 || stage >= gridScriptableObjects.Length)
        {
            Debug.LogError("Invalid stage index: " + stage);
            return null;
        }
        return gridScriptableObjects[stage].GridDataArray;
    }
}
