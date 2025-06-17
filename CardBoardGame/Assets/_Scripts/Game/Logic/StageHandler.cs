using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
public class StageHandler : MonoBehaviour
{
    private Difficulty curDiff;
    private MonsterGridScriptableObject monsterGridScriptableObject;

    public void SetDifficulty(Difficulty diff)
    {
        // 난이도를 설정하고 스테이지를 초기화합니다.
        curDiff = diff;
        Debug.Log($"Setting Difficulty: {curDiff}");
        InitializeStage(curDiff);
    }

    private void InitializeStage(Difficulty curDiff)
    {
        //TODO : 난이도에 따라 스테이지를 초기화하는 로직을 구현합니다.
        switch (curDiff)
        {
            case Difficulty.Easy:
                Debug.Log("Initializing Easy Stage");
                // Easy stage initialization logic
                monsterGridScriptableObject = Resources.Load<MonsterGridScriptableObject>("Data/EasyMonsterGrid");
                break;
            case Difficulty.Normal:
                Debug.Log("Initializing Normal Stage");
                // Normal stage initialization logic
                monsterGridScriptableObject = Resources.Load<MonsterGridScriptableObject>("Data/NormalMonsterGrid");
                break;
            case Difficulty.Hard:
                Debug.Log("Initializing Hard Stage");
                // Hard stage initialization logic
                monsterGridScriptableObject = Resources.Load<MonsterGridScriptableObject>("Data/HardMonsterGrid");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(curDiff), curDiff, null);
        }

    }
}
