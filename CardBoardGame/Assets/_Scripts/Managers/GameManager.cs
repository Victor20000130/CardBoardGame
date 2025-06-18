using System;
using Unity.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private MonsterGridSO currMonsterGridData;
    private StageHandler stageHandler;
    private BattleHandler battleHandler;
    private Dice dice;
    private PlayerPiece playerPiece;
    private int diceValue = -1;
    private bool isGameStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int GridLenght => currMonsterGridData.GetGridDatas((int)stageHandler.CurrentStage).Length;
    private void Awake()
    {
        SceneManager.sceneLoaded +=
            (scene, mode) =>
            {
                if (scene.name == "GameScene")
                {
                    // Initialize the game components
                    stageHandler = FindAnyObjectByType<StageHandler>();
                    battleHandler = FindAnyObjectByType<BattleHandler>();
                    dice = FindAnyObjectByType<Dice>();
                    playerPiece = FindAnyObjectByType<PlayerPiece>();
                    if (stageHandler == null)
                    {
                        Debug.LogError("StageHandler not found in the scene.");
                        return;
                    }
                    if (battleHandler == null)
                    {
                        Debug.LogError("BattleHandler not found in the scene.");
                        return;
                    }
                    if (dice == null)
                    {
                        Debug.LogError("Dice not found in the scene.");
                        return;
                    }
                    if (playerPiece == null)
                    {
                        Debug.LogError("PlayerPiece not found in the scene.");
                        return;
                    }
                    Debug.Log("GameManager: StageHandler and BattleHandler initialized successfully.");
                    // You can also initialize other game components here if needed
                }
            };
    }

    private void Start()
    {
        // Optionally, you can initialize the game state here
        Debug.Log("GameManager: Game started.");

        currMonsterGridData =
        stageHandler.InitStageHandler(ManagerHandler.Instance.dataManager.CurrentGameData.Difficulty,
                                         ManagerHandler.Instance.dataManager.CurrentGameData.Stage);
    }

    // Update is called once per frame
    private void Update()
    {
        diceValue = dice.GetUpFace();
        if (diceValue != -1)
        {
            // TODO 플레이어 말 움직임

            diceValue = -1;
        }

    }

}
