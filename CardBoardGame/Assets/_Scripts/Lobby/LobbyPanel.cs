using System;
using UnityEngine;
using UnityEngine.UI;

public enum LobbyPanelType
{
    None,
    MainPanel,
    HowToPlayPanel,
    OptionsPanel,
    EditorsPanel,
    DifficultyPanel,
    SaveDataPanel
}

public class LobbyPanel : MonoBehaviour
{
    protected LobbyPanelType panelType;
    protected LobbyUIHandler lobbyUIHandler;
    [SerializeField] protected Button exitButton;
    protected virtual void Awake()
    {
        // Ensure the panel is set up correctly
        if (gameObject == null)
        {
            Debug.LogError("Panel game object is not assigned.");
            return;
        }
    }

    protected virtual void Start()
    {
        // Initialize the panel if needed
        InitializePanel();
    }

    protected virtual void InitializePanel()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        else
        {
            Debug.LogError("Exit button is not assigned in the Panel.");
        }
    }

    private void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public virtual void GetHandler()
    {
        if (lobbyUIHandler != null)
        {
            Debug.LogWarning("LobbyUIHandler is already assigned.");
            return;
        }
        lobbyUIHandler = GetComponentInParent<LobbyUIHandler>();
    }

    public LobbyPanelType GetPanelType()
    {
        return panelType;
    }
}
