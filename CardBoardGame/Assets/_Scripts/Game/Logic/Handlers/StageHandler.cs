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
    private Stage currStage;
    private StageSO monsterGridSO;
    //TODO : 적이 죽었을 때 스테이지 넘어가기
    public Stage CurrentStage
    {
        get { return currStage; }
        set
        {
            currStage = value;
            Debug.Log($"Current Stage set to: {currStage}");
            if (currStage == Stage.None)
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
        curDiff = ManagerHandler.Instance.dataManager.CurrentGameData.Difficulty;
        CurrentStage = ManagerHandler.Instance.dataManager.CurrentGameData.Stage;
    }
    private void InitializeStageButtons()
    {
        foreach (var button in stageButtons)
        {
            button.onClick.AddListener(() => OnStageButtonClicked(CurrentStage));
        }
    }
    private void OnStageButtonClicked(Stage currStage)
    {
        if (currStage == Stage.None)
        {
            currStage = Stage.Stage1;
        }
        switch (currStage)
        {
            case Stage.Stage1:
                ManagerHandler.Instance.dataManager.CurrentGameData.Stage = Stage.Stage1;
                break;
            case Stage.Stage2:
                ManagerHandler.Instance.dataManager.CurrentGameData.Stage = Stage.Stage2;
                break;
            case Stage.Stage3:
                ManagerHandler.Instance.dataManager.CurrentGameData.Stage = Stage.Stage3;
                break;
            case Stage.Stage4:
                ManagerHandler.Instance.dataManager.CurrentGameData.Stage = Stage.Stage4;
                break;
            case Stage.Stage5:
                ManagerHandler.Instance.dataManager.CurrentGameData.Stage = Stage.Stage5;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currStage), currStage, null);
        }
        selectStagePanel.SetActive(false);
        Debug.Log($"StageHandler: Stage {currStage} button clicked.");

        ManagerHandler.Instance.gameManager.StartGame();

    }
    private void InitializeStage()
    {
        switch (curDiff)
        {
            case Difficulty.Easy:
                Debug.Log("Initializing Easy Stage");
                monsterGridSO = ManagerHandler.Instance.dataManager.EasyStageSO;
                break;
            case Difficulty.Normal:
                Debug.Log("Initializing Normal Stage");
                monsterGridSO = ManagerHandler.Instance.dataManager.NormalStageSO;
                break;
            case Difficulty.Hard:
                Debug.Log("Initializing Hard Stage");
                monsterGridSO = ManagerHandler.Instance.dataManager.HardStageSO;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(curDiff), curDiff, null);
        }
        gridHandler.InitializeGridData(monsterGridSO, curDiff);
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
