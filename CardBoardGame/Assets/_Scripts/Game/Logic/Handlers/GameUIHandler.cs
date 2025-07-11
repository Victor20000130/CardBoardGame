using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : Handler
{
    private const int LevelPerUsedCards = 10;

    [SerializeField]
    private Button cardPanelBTN;
    public Button CardPanelBTN => cardPanelBTN;

    [SerializeField]
    private Material levelOnMat;
    [SerializeField]
    private Material levelOffMat;

    [SerializeField]
    private ElementObj[] elementObjs;
    private void Awake()
    {
        if (elementObjs.Length < 3)
        {
            Debug.LogError($"특수효과 오브젝트 갯수 부족");
        }

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

    public void ElemEffLevelOn(Dictionary<Shape, int> usedCardDic)
    {
        foreach (Shape shape in usedCardDic.Keys)
        {
            switch (shape)
            {
                case Shape.Spade:
                    elementObjs[0].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;
                case Shape.Club:
                    elementObjs[1].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;
                case Shape.Diamond:
                    elementObjs[2].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;
                case Shape.Heart:
                    elementObjs[3].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;

            }
        }
    }
}