using System;
using System.Collections;
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
    private PieceHandler pieceHandler;
    private int diceValue = -1;

    private bool isRoll = false;
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
                    pieceHandler = FindAnyObjectByType<PieceHandler>();
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
                    if (pieceHandler == null)
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

    private void FixedUpdate()
    {
        if (isRoll == true)
        {
            isRoll = false;
            dice.RollDice();

        }
    }

    public void ReceiveDiceValue(int diceValue)
    {
        this.diceValue = diceValue;
        print($"받은 눈금 {this.diceValue}");
        // pieceHandler.
    }

    public void StartGame()
    {
        StartCoroutine(StartEffectCoru());
    }
    WaitForSeconds waitForOneSeconds = new WaitForSeconds(1);
    private IEnumerator StartEffectCoru()
    {
        print("게임시작 3초 전");
        yield return waitForOneSeconds;
        print("게임시작 2초 전");
        yield return waitForOneSeconds;
        print("게임시작 1초 전");
        yield return waitForOneSeconds;
        print("게임 시작");
        yield return null;
        isRoll = true;
    }
}
