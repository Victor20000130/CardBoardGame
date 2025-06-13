using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyPanel : LobbyPanel
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    protected override void Awake()
    {
        base.Awake();
        panelType = LobbyPanelType.DifficultyPanel;
    }
    protected override void InitializePanel()
    {
        base.InitializePanel();
        easyButton.onClick.AddListener(() => SetDifficulty(Difficulty.Easy));
        normalButton.onClick.AddListener(() => SetDifficulty(Difficulty.Normal));
        hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));
    }

    private void SetDifficulty(Difficulty diff)
    {
        ManagerHandler.Instance.dataManager.CurrentGameData.SetDifficulty(diff);
        print($"Difficulty set to: {diff}");
        // TODO : 데이터 저장관리 로직 완성 시 호출
        // SetDataPanelActive(true);
        // SetDifficultyPanelActive(false);
        SceneManager.LoadScene("GameScene"); // Load the game scene
    }
}
