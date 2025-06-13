using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{

    [Header("UI Panels")]
    [SerializeField] private List<LobbyPanel> lobbyPanels;
    private Dictionary<LobbyPanelType, LobbyPanel> panelDictionary = new Dictionary<LobbyPanelType, LobbyPanel>();
    private void Awake()
    {
        // Initialize the panel dictionary
        foreach (var panel in lobbyPanels)
        {
            if (panel != null)
            {
                panel.GetHandler();
                panelDictionary[panel.GetPanelType()] = panel;
            }
            else
            {
                Debug.LogError("Lobby Panel is not assigned in the LobbyUIHandler.");
            }
        }
    }

    private void Start()
    {
        CloseAllPanels();
        // Optionally, you can set the initial active panel here
        OpenPanel(LobbyPanelType.MainPanel);
    }
    public void OpenPanel(LobbyPanelType panelType)
    {
        if (panelType == LobbyPanelType.None)
        {
            Debug.LogError("Cannot open a panel of type None.");
            return;
        }
        if (panelDictionary.TryGetValue(panelType, out LobbyPanel panel))
        {
            panel.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"Panel of type {panelType} not found in the LobbyUIHandler.");
        }
    }
    private void CloseAllPanels()
    {
        foreach (var panel in panelDictionary.Values)
        {
            if (panel != null)
            {
                panel.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Lobby Panel is not assigned in the LobbyUIHandler.");
            }
        }
    }
}