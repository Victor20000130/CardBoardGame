using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "Scriptable Objects/CardScriptableObject")]
public class CardSO : ScriptableObject
{
    public List<CardData> cards;
    public string spadeName;
    public string diamondName;
    public string clubName;
    public string heartName;
    public int minRange;
    public int maxRange;
    private int initCount = 0;

    private void InitCardData(string name)
    {
        for (int i = minRange; i <= maxRange; i++)
        {
            cards.Add(new CardData());
            cards[initCount].sprite = Resources.Load<Sprite>("Card/" + name + i.ToString());
            cards[initCount].shape = InitShape(name);
            cards[initCount].number = (Number)i;
            Debug.Log($"Init Card {name}, {i}, {initCount}");
            initCount++;
        }
    }

    private Shape InitShape(string name)
    {
        switch (name.ToLower())
        {
            case "spade":
                return Shape.Spade;
            case "diamond":
                return Shape.Diamond;
            case "club":
                return Shape.Club;
            case "heart":
                return Shape.Heart;
            default:
                Debug.LogWarning($"Unknown shape name: {name}");
                return Shape.None; // Shape.None이 없다면 적절한 기본값으로 변경
        }
    }

    public void InitCardSO()
    {
        if (cards.Count == ManagerHandler.Instance.dataManager.TotalCardCount)
        {
            Debug.Log("이미 SO가 초기화 되어 있습니다.");
            return;
        }
        else
        {
            cards.Clear();
        }
        initCount = 0;
        InitCardData(spadeName);
        InitCardData(diamondName);
        InitCardData(clubName);
        InitCardData(heartName);
    }
}