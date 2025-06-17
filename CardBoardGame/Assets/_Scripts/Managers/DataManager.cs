using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;

[Serializable]
public class PlayerData
{

}
[Serializable]
public class GameData
{
    Difficulty difficulty;
    public void SetDifficulty(Difficulty diff)
    {
        difficulty = diff;
    }
    public Difficulty GetDifficulty()
    {
        return difficulty;

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
    }

}
