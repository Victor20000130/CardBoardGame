using UnityEngine;

public class SaveDataPanel : LobbyPanel
{

    public override void InitializePanel()
    {
        base.InitializePanel();
        panelType = LobbyPanelType.SaveDataPanel;
    }
    private void SetDataPanelActive(bool isActive)
    {
        // if (saveDataPanel != null)
        // {
        //     //TODO : 데이터 불러오기
        //     saveDataPanel.SetActive(isActive);
        // }
        // else
        // {
        //     Debug.LogError("Data Panel is not assigned in the LobbyUIHandler.");
        // }
    }

}
