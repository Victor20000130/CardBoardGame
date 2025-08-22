using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CardBoardGame.Assets._Scripts.Utility;
using System.Collections;
using UnityEditor;

public class DifficultyPanel : LobbyPanel
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;

    public override void InitializePanel()
    {
        base.InitializePanel();
        panelType = LobbyPanelType.DifficultyPanel;
        easyButton.onClick.AddListener(() => SetDifficulty(Difficulty.Easy));
        normalButton.onClick.AddListener(() => SetDifficulty(Difficulty.Normal));
        hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));
        exitButton.onClick.AddListener(() => lobbyUIHandler.OpenPanel(LobbyPanelType.MainPanel));
    }

    private void SetDifficulty(Difficulty diff)
    {
        ManagerHandler.Instance.dataManager.CurGameData.Difficulty = diff;
        print($"Difficulty set to: {diff}");
        // TODO : 데이터 저장관리 로직 완성 시 호출
        // SetDataPanelActive(true);
        // SetDifficultyPanelActive(false);
        // SceneManager.LoadScene("GameScene"); // Load the game scene
        // SceneManager.LoadScene((int)diff);
        StartCoroutine(LoadSceneAsync((int)diff));
    }

    private IEnumerator LoadSceneAsync(int diff)
    {

        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(diff);
        asyncOperation.allowSceneActivation = false;

        print($"Scene Load Progress : {asyncOperation.progress}");
        while (!asyncOperation.isDone)
        {
            print("씬 로딩중..");
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
