using System;
using UnityEngine;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.UI;
using UnityEngine.Events;
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
        }
    }

    private void Awake()
    {
        SelectStagePanelOpen();
    }

    private void GetStageData()
    {
        var dataManager = ManagerHandler.Instance.dataManager;
        curDiff = dataManager.CurGameData.Difficulty;
        CurrentStage = dataManager.CurGameData.Stage;
    }
    private void InitializeStageButtons()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int idx = i;
            stageButtons[i].onClick.AddListener(() =>
            {
                CurrentStage = (Stage)(idx + 1);
                OnStageButtonClicked();
            });
        }
    }

    private void OnStageButtonClicked()
    {
        var dataManager = ManagerHandler.Instance.dataManager;
        int stageIdx = (int)CurrentStage - 1;
        dataManager.CurGameData.Stage = CurrentStage;
        CurMonsterSO = curStageSO.MonsterSO[stageIdx];

        selectStagePanel.SetActive(false);
        Debug.Log($"StageHandler: Stage {curStage} button clicked.");
        ManagerHandler.Instance.gameManager.StartGame();

    }
    private void InitializeStage()
    {
        var dataManager = ManagerHandler.Instance.dataManager;
        switch (curDiff)
        {
            case Difficulty.Easy:
                Debug.Log("Initializing Easy Stage");
                curStageSO = dataManager.EasyStageSO;
                break;
            case Difficulty.Normal:
                Debug.Log("Initializing Normal Stage");
                curStageSO = dataManager.NormalStageSO;
                break;
            case Difficulty.Hard:
                Debug.Log("Initializing Hard Stage");
                curStageSO = dataManager.HardStageSO;
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
        print(curStageSO);
        ManagerHandler.Instance.gameManager.StageSO = curStageSO;
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.StageHandler;
    }

    public void SelectStagePanelOpen()
    {
        selectStagePanel.SetActive(true);
    }
    public void StageClear()
    {
        stageButtons[(int)CurrentStage - 1].interactable = true;
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
