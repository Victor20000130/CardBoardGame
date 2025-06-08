using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button editorsButton;

    private void Awake()
    {
        // Ensure all buttons are assigned
        if (gameStartButton == null ||
            howToPlayButton == null ||
            optionButton == null ||
            exitButton == null ||
            editorsButton == null)
        {
            Debug.LogError("One or more buttons are not assigned in the LobbyUIHandler.");
            return;
        }

        // Add listeners to buttons
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        howToPlayButton.onClick.AddListener(OnHowToPlayButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        editorsButton.onClick.AddListener(OnEditorsButtonClicked);

    }

    private void OnGameStartButtonClicked()
    {
        // Logic to start the game
        print("Game Start Button Clicked");
    }
    private void OnHowToPlayButtonClicked()
    {
        // Logic to show how to play
        print("How To Play Button Clicked");
    }
    private void OnOptionButtonClicked()
    {
        // Logic to open options menu
        print("Options Button Clicked");
    }
    private void OnExitButtonClicked()
    {
        // Logic to exit the game
        print("Exit Button Clicked");
#if UNITY_EDITOR
        // Stop playing in the editor
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void OnEditorsButtonClicked()
    {
        // Logic to open editors menu
        print("Editors Button Clicked");
    }

}