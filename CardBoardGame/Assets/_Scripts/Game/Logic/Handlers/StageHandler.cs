using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
public class StageHandler : Handler
{
    [SerializeField] private GridHandler gridHandler;
    [SerializeField] private Button[] stageButtons;
    [SerializeField] private GameObject selectStagePanel;
    private Difficulty curDiff;
    private Stage curStage;
    private StageSO curStageSO;
    public MonsterSO CurMonsterSO;
    //TODO : 적이 죽었을 때 스테이지 넘어가기
    public Stage CurrentStage
    {
        get { return curStage; }
        set
        {
            curStage = value;
            Debug.Log($"Current Stage set to: {curStage}");
            if (curStage == Stage.None)
            {
                return;
            }
            stageButtons[(int)CurrentStage - 1].interactable = true;
        }
    }

    private void Awake()
    {
        selectStagePanel.SetActive(true);
    }

    private void GetStageData()
    {
        curDiff = ManagerHandler.Instance.dataManager.CurGameData.Difficulty;
        CurrentStage = ManagerHandler.Instance.dataManager.CurGameData.Stage;
    }
    private void InitializeStageButtons()
    {
        foreach (var button in stageButtons)
        {
            button.onClick.AddListener(() => OnStageButtonClicked(CurrentStage));
        }
    }
    private void OnStageButtonClicked(Stage curStage)
    {
        if (curStage == Stage.None)
        {
            curStage = Stage.Stage1;
        }
        switch (curStage)
        {
            case Stage.Stage1:
                ManagerHandler.Instance.dataManager.CurGameData.Stage = Stage.Stage1;
                CurMonsterSO = curStageSO.MonsterSO[0];
                break;
            case Stage.Stage2:
                ManagerHandler.Instance.dataManager.CurGameData.Stage = Stage.Stage2;
                CurMonsterSO = curStageSO.MonsterSO[1];
                break;
            case Stage.Stage3:
                ManagerHandler.Instance.dataManager.CurGameData.Stage = Stage.Stage3;
                CurMonsterSO = curStageSO.MonsterSO[2];
                break;
            case Stage.Stage4:
                ManagerHandler.Instance.dataManager.CurGameData.Stage = Stage.Stage4;
                CurMonsterSO = curStageSO.MonsterSO[3];
                break;
            case Stage.Stage5:
                ManagerHandler.Instance.dataManager.CurGameData.Stage = Stage.Stage5;
                CurMonsterSO = curStageSO.MonsterSO[4];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(curStage), curStage, null);
        }
        selectStagePanel.SetActive(false);
        Debug.Log($"StageHandler: Stage {curStage} button clicked.");

        ManagerHandler.Instance.gameManager.StartGame();

    }
    private void InitializeStage()
    {
        switch (curDiff)
        {
            case Difficulty.Easy:
                Debug.Log("Initializing Easy Stage");
                curStageSO = ManagerHandler.Instance.dataManager.EasyStageSO;
                break;
            case Difficulty.Normal:
                Debug.Log("Initializing Normal Stage");
                curStageSO = ManagerHandler.Instance.dataManager.NormalStageSO;
                break;
            case Difficulty.Hard:
                Debug.Log("Initializing Hard Stage");
                curStageSO = ManagerHandler.Instance.dataManager.HardStageSO;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(curDiff), curDiff, null);
        }
        gridHandler.InitializeGridData(curStageSO, curDiff);
        Debug.Log("StageHandler: Stage initialized with " + curDiff + " difficulty.");
    }

    protected override void OnInitialize()
    {

        // 난이도를 설정하고 스테이지를 초기화합니다.
        GetStageData();
        InitializeStageButtons();
        InitializeStage();
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.StageHandler;
    }
    // public MonsterGridSO InitStageHandler(Difficulty diff, Stage stage)
    // {
    //     // 난이도를 설정하고 스테이지를 초기화합니다.
    //     curDiff = diff;
    //     CurrentStage = stage;
    //     Debug.Log($"Setting Difficulty: {curDiff}");
    //     InitializeStageButtons();
    //     return InitializeStage(curDiff);
    // }

}
