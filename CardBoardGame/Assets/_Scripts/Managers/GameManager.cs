using System;
using System.Collections;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private StageSO curStageSO;
    public StageSO StageSO
    {
        get => curStageSO;
        set => StageSO = value;
    }
    private Dictionary<HandlerType, Handler> handlers = new Dictionary<HandlerType, Handler>();
    private StageHandler StageHandler => GetHandler<StageHandler>(HandlerType.StageHandler);
    private PieceHandler PieceHandler => GetHandler<PieceHandler>(HandlerType.PieceHandler);
    private GridHandler GridHandler => GetHandler<GridHandler>(HandlerType.GridHandler);
    private DiceHandler DiceHandler => GetHandler<DiceHandler>(HandlerType.DiceHandler);
    private BattleHandler BattleHandler => GetHandler<BattleHandler>(HandlerType.BattleHandler);
    private CardHandler CardHandler => GetHandler<CardHandler>(HandlerType.CardHandler);
    // private MiniGameHandler MiniGameHandler => GetHandler<MiniGameHandler>(HandlerType.MiniGameHandler);
    private Action<int> onPieceMove;

    private bool isRoll = false;

    // 그리드가 스테이지마다 동적으로 변할 경우 사용
    // public int GridLenght => currMonsterGridData.GetGridDatas((int)StageHandler.CurrentStage).Length;

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
            DiceHandler.RollDice();
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
                Debug.LogError($"GM: 예상치 못한 씬 Index: {scene.buildIndex}");
                break;
        }
    }

    private void HandleLobbyScene()
    {
        Debug.Log("GM 로비 씬 초기화");
    }
    private void HandleGameScene(int sceneInx)
    {
        Debug.Log("GM HandleGameScene");
        Debug.Log($"{sceneInx}번 게임 씬 초기화");

        InitializeHandlers();

        onPieceMove += GridHandler.GetCurrentGridData;

        Debug.Log("GameManager: 핸들러 초기화 완료");
    }

    private void InitializeHandlers()
    {
        Handler[] handlers = FindObjectsByType<Handler>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Handler handler in handlers)
        {
            handler.SetHandlerType();
            this.handlers.Add(handler.HandlerType, handler);
            handler.Initialize();
        }
    }

    /// <summary>
    /// HandlerType과 T에 따라서 해당되는 Handler를 상속받는 자식클래스를 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handlerType"></param>
    /// <returns></returns>
    private T GetHandler<T>(HandlerType handlerType) where T : Handler
    {
        if (handlers.TryGetValue(handlerType, out Handler handler))
        {
            return handler as T;
        }
        else
        {
            Debug.LogError($"Handler of type {handlerType} not found.");
            return null;
        }
    }

    public void ReceiveDiceValue(int diceValue)
    {
        StartCoroutine(PieceHandler.MoveCorou(diceValue, onPieceMove));
    }
    public void ReceiveGridData(GridData gridData)
    {
        BattleHandler.SendGridType(gridData.gridType, CardHandler);
        print($"GridType {gridData.gridType}, idx {gridData.Idx}");
        //TODO 스테이지별 자동 저장이 아닌 그리드 이동마다 저장할 시 그리드 데이터 저장(데이터 매니저 호출)
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        SendMonsterSO();
        StartCoroutine(DiceRollCoroutine());
    }
    private IEnumerator DiceRollCoroutine()
    {
        yield return null;
        isRoll = true;
    }

    private void SendMonsterSO()
    {
        BattleHandler.ReceiveMonsterSO(StageHandler.CurMonsterSO);
    }
}
