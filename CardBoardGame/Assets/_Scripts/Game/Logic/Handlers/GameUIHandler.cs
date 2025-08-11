using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : Handler
{
    public RectTransform CanvasRt;
    public PopUpUI popUpUI;
    private const int LevelPerUsedCards = 10;
    [SerializeField]
    private Material levelOnMat;
    [SerializeField]
    private Material levelOffMat;

    [SerializeField]
    private ElementObj[] elementObjs;

    [SerializeField]
    private Image fillArea;
    [SerializeField] private Button[] gameUIBTNs;
    private float timer = 0f;
    public int CardSelectTime = 90;
    private bool isCardSelectTime = false;
    public bool IsCardSelectTime
    {
        get => isCardSelectTime;
        set
        {
            isCardSelectTime = value;
            if (IsCardSelectTime == false)
            {
                timer = 0;
            }
        }
    }

    private void Awake()
    {
        if (elementObjs.Length < 3)
        {
            Debug.LogError($"특수효과 오브젝트 갯수 부족");
        }

    }

    private void Update()
    {
        if (isCardSelectTime)
        {
            ClockStart();
        }
        fillArea.fillAmount = timer / CardSelectTime;
    }
    protected override void OnInitialize()
    {

    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.GameUIHandler;
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
                case Shape.Diamond:
                    elementObjs[1].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;
                case Shape.Heart:
                    elementObjs[2].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;
                case Shape.Club:
                    elementObjs[3].SetMaterial(usedCardDic[shape] / LevelPerUsedCards, levelOnMat);
                    break;

            }
        }
    }

    private void ClockStart()
    {
        timer += Time.deltaTime;
        if (timer >= CardSelectTime)
        {
            isCardSelectTime = false;
            ManagerHandler.Instance.gameManager.CardSelectTimeOver();
            timer = 0;
        }
    }

}

