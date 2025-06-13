using System;
using UnityEngine;

[Serializable]
public struct PlayerData
{

}
[Serializable]
public struct GameData
{
    public Difficulty difficulty;
    // public string playerName;
    public void SetDifficulty(Difficulty diff)
    {
        difficulty = diff;
    }
    public PlayerData playerData;

}
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}
public class DataManager : MonoBehaviour
{
    [SerializeField] private GridScriptableObject[] gridScriptableObjects;
    private GameData currentGameData;
    public GameData CurrentGameData
    {
        get { return currentGameData; }
        set { currentGameData = value; }
    }
    public GridScriptableObject[] GridScriptableObjects
    {
        get => gridScriptableObjects;
    }

}
