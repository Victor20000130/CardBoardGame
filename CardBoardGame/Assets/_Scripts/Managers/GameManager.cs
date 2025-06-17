using System;
using Unity.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GridScriptableObject currentGridData;
    private StageHandler stageHandler;
    private BattleHandler battleHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
                    Debug.Log("GameManager: StageHandler and BattleHandler initialized successfully.");
                    stageHandler.SetDifficulty(ManagerHandler.Instance.dataManager.CurrentGameData.GetDifficulty());

                    // You can also initialize other game components here if needed
                }
            };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
