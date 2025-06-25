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

    private GameData currentGameData;

    public int TotalCardCount = 52;

    public GameData CurrentGameData
    {
        get { return currentGameData; }
        set
        {
            currentGameData = value;
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
        currentGameData = new GameData();
    }

    private void HandleGameScene(int sceneInx)
    {
        Debug.Log("DM HandleGameScene");
    }
    private void ForTest()
    {
        // Initialize the current game data
        currentGameData.Difficulty = Difficulty.Easy;
        currentGameData.Stage = Stage.Stage1;
    }
}
