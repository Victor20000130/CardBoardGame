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
    private GameData currentGameData;
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

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            currentGameData.playerData = new PlayerData();
            currentGameData.Difficulty = Difficulty.Easy;
            Debug.Log("DataManager: Game data initialized with Easy difficulty.");
        }
    }

}
