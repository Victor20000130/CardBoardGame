using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : Handler
{
    [SerializeField]
    private Button cardPanelBTN;
    public Button CardPanelBTN => cardPanelBTN;

    private void Awake()
    {

    }
    protected override void OnInitialize()
    {

    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.GameUIHandler;
    }

    public void GetCardHandler(CardHandler cardPanel)
    {
        cardPanelBTN.onClick.AddListener(cardPanel.CardPanelOnOff);
    }
}