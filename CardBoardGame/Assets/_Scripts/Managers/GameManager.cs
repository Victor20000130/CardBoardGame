using System;
using System.Collections;
using Unity.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private MonsterGridSO currMonsterGridData;
    private StageHandler stageHandler;
    private BattleHandler battleHandler;
    private PieceHandler pieceHandler;
    private GridHandler gridHandler;
    private Dice dice;
    private Action<int> onPieceMove;

    private bool isRoll = false;
    public int GridLenght => currMonsterGridData.GetGridDatas((int)stageHandler.CurrentStage).Length;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void Start()
    {
        print("스타트");

    }

    private void FixedUpdate()
    {
        if (isRoll == true)
        {
            isRoll = false;
            dice.RollDice();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name} (Index: {scene.buildIndex})");

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
                Debug.LogError($"GameManager: 예상치 못한 씬 Index: {scene.buildIndex}");
                break;
        }
    }
    private void OnSceneUnLoaded(Scene scene)
    {
        switch (scene.buildIndex)
        {
            case 0:
                // UnloadLobbyScene();
                break;
            case 1:
            case 2:
            case 3:
                // UnloadGameScene(scene.buildIndex);
                onPieceMove = null;
                break;
            default:
                Debug.LogError($"GameManager: 예상치 못한 씬 Index: {scene.buildIndex}");
                break;
        }
    }

    private void HandleLobbyScene()
    {
        Debug.Log("로비 씬 초기화");
    }
    private void HandleGameScene(int sceneInx)
    {
        Debug.Log($"{sceneInx}번 게임 씬 초기화");

        InitializeHandlers();

        if (stageHandler == null || battleHandler == null || dice == null || pieceHandler == null || gridHandler == null)
        {
            Debug.LogError("GameManager: 필수 핸들러가 누락되었습니다.");
            return;
        }

        currMonsterGridData = stageHandler.InitStageHandler(
            ManagerHandler.Instance.dataManager.CurrentGameData.Difficulty,
            ManagerHandler.Instance.dataManager.CurrentGameData.Stage);
        onPieceMove += gridHandler.GetCurrentGridData;
        Debug.Log("GameManager: 핸들러 초기화 완료");
    }

    private void InitializeHandlers()
    {
        stageHandler = FindHandler<StageHandler>("StageHandler");
        battleHandler = FindHandler<BattleHandler>("BattleHandler");
        pieceHandler = FindHandler<PieceHandler>("PieceHandler");
        gridHandler = FindHandler<GridHandler>("GridHandler");
        dice = FindHandler<Dice>("Dice");
    }

    private T FindHandler<T>(string handlerName) where T : MonoBehaviour
    {
        T handler = FindAnyObjectByType<T>();
        if (handler == null)
        {
            Debug.LogError($"{handlerName}를 찾을 수 없습니다.");
        }
        return handler;
    }

    public void ReceiveDiceValue(int diceValue)
    {
        print($"받은 주사위 값: {diceValue}");
        StartCoroutine(pieceHandler.MoveCorou(diceValue, onPieceMove));
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        print($"TimeScale : {Time.timeScale}");
        StartCoroutine(StartEffectCoru());
    }
    private IEnumerator StartEffectCoru()
    {
        print("게임 시작");
        yield return null;
        isRoll = true;
    }
}
