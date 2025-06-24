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
            stageButtons[(int)CurrentStage - 1].interactable = true;
        }
    }

    private void Awake()
    {
        selectStagePanel.SetActive(true);
    }
    public override void Initialize()
    {
        base.Initialize();
        handlerType = HandlerType.StageHandler;

        // 난이도를 설정하고 스테이지를 초기화합니다.
        GetStageData();
        InitializeStageButtons();
        InitializeStage();
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
        switch (currStage)
        {
            case Stage.Stage1:
                Debug.Log("Stage 1 button clicked.");
                break;
            case Stage.Stage2:
                Debug.Log("Stage 2 button clicked.");
                break;
            case Stage.Stage3:
                Debug.Log("Stage 3 button clicked.");
                break;
            case Stage.Stage4:
                Debug.Log("Stage 4 button clicked.");
                break;
            case Stage.Stage5:
                Debug.Log("Stage 5 button clicked.");
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
