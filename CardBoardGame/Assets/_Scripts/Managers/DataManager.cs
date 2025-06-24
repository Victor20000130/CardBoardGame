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
    private StageSO[] monsterGridSO;

    public StageSO EasyStageSO => monsterGridSO[0];
    public StageSO NormalStageSO => monsterGridSO[1];
    public StageSO HardStageSO => monsterGridSO[2];

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

        // Initialize the current game data
        currentGameData = new GameData();
        currentGameData.Difficulty = Difficulty.Easy;
        currentGameData.Stage = Stage.Stage1;
    }

}
