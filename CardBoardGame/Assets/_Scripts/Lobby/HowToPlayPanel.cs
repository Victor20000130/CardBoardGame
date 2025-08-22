using System;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : LobbyPanel
{
    [SerializeField]
    private Button nextPageBTN;
    [SerializeField]
    private Button prevPageBTN;
    [SerializeField]
    private GameObject[] firtPageObjs;
    [SerializeField]
    private GameObject[] secondPageObjs;
    public override void InitializePanel()
    {
        base.InitializePanel();
        exitButton.onClick.AddListener(() => lobbyUIHandler.OpenPanel(LobbyPanelType.MainPanel));
        panelType = LobbyPanelType.HowToPlayPanel;
        nextPageBTN.onClick.AddListener(OnSecondPage);
        prevPageBTN.onClick.AddListener(OnFirstPage);
    }

    private void OnSecondPage()
    {
        foreach (GameObject obj in secondPageObjs)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in firtPageObjs)
        {
            obj.SetActive(false);
        }
    }

    private void OnFirstPage()
    {
        foreach (GameObject obj in secondPageObjs)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in firtPageObjs)
        {
            obj.SetActive(true);
        }
    }
}
