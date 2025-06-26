using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{

}
[Serializable]
public class GameData
{
    private Difficulty difficulty;
    private Stage stage;
    public Difficulty Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }
    public Stage Stage
    {
        get { return stage; }
        set { stage = value; }
    }
    public PlayerData playerData;

}
public class DataManager : MonoBehaviour
{
    [SerializeField]
    private StageSO[] StageSO;

    public StageSO EasyStageSO => StageSO[0];
    public StageSO NormalStageSO => StageSO[1];
    public StageSO HardStageSO => StageSO[2];
    private GameData curGameData;

    public int TotalCardCount = 52;
    public bool testInGameScene;
    public GameData CurGameData
    {
        get { return curGameData; }
        set
        {
            curGameData = value;
        }
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                HandleLobbyScene();
                break;
            case 1:
            case 2:
            case 3:
                HandleGameScene(scene.buildIndex);
                break;
            default:
                Debug.LogError($"DM: 예상치 못한 씬 Index: {scene.buildIndex}");
                break;
        }
        // ForTest();
    }
    private void HandleLobbyScene()
    {
        Debug.Log("dm 로비 씬 초기화");
        curGameData = new GameData();
    }

    private void HandleGameScene(int sceneInx)
    {
        Debug.Log("DM HandleGameScene");
        if (testInGameScene)
        {
            ForTest();

        }
    }
    private void ForTest()
    {
        Debug.LogWarning("테스트 메서드 동작중");
        // Initialize the current game data
        curGameData = new GameData();
        curGameData.Difficulty = Difficulty.Easy;
        curGameData.Stage = Stage.Stage1;
    }
    // public StageSO SendStageSO()
    // {
    //     switch (CurGameData.Difficulty)
    //     {
    //         case Difficulty.Easy:
    //             CurStageSO = EasyStageSO;
    //             break;
    //         case Difficulty.Normal:
    //             CurStageSO = NormalStageSO;
    //             break;
    //         case Difficulty.Hard:
    //             CurStageSO = HardStageSO;
    //             break;
    //         default:
    //             Debug.LogError("난이도 설정 오류");
    //             break;
    //     }
    // }

    // private StageSO SendMonsterSO()
    // {
    //     switch (CurGameData.Stage)
    //     {
    //         case Stage.Stage1:
    //             CurStageSO[0];
    //             break;
    //         case Stage.Stage2:
    //             break;
    //         case Stage.Stage3:
    //             break;
    //         case Stage.Stage4:
    //             break;
    //         case Stage.Stage5:
    //             break;
    //         default:
    //             break;

    //     }
    // }
}
