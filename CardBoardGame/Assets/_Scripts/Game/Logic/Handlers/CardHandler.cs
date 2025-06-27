using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CardHandler : Handler
{
    private CardSO cardSO;
    private List<int> deck;
    private int currIdx = 0;
    [SerializeField]
    private Card[] monsterCard;
    [SerializeField]
    private Card[] userCard;

    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button throwButton;
    [SerializeField]
    private GameObject cardPanel;
    private void Awake()
    {

    }

    public void Shuffle()
    {
        currIdx = 0;
        var shuffler = new CardShuffle();
        shuffler.Shuffle(deck);
        print(1);
        SetCards();
        print(3);

    }

    public void SetCards()
    {
        #region 2차원배열로 구현
        // monsterCard와 userCard를 1차원 배열로 합치기
        // Card[] allCards = new Card[monsterCard.Length + userCard.Length];
        // monsterCard.CopyTo(allCards, 0);
        // userCard.CopyTo(allCards, monsterCard.Length);

        // foreach (Card card in allCards)
        // {
        //     card.Button.image.sprite = cardSO.cards[deck[currIdx]];
        //     currIdx++;
        // }

        // 위 방식을 써도 충분하지만, 굳이 추가로 배열을 생성할 필요가 없음.
        #endregion
        // monsterCard 처리
        print(2);
        for (int i = 0; i < monsterCard.Length; i++)
        {
            monsterCard[i].Initialize();
            // monsterCard[i].Button.image.sprite = cardSO.cards[deck[currIdx]].sprite;
            monsterCard[i].CardData = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
        // userCard 처리
        for (int i = 0; i < userCard.Length; i++)
        {
            userCard[i].Initialize();
            // userCard[i].Button.image.sprite = cardSO.cards[deck[currIdx]].sprite;
            userCard[i].CardData = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
    }
    public void CardPanelOnOff()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
    }

    private void ButtonInitialize()
    {
        foreach (Card card in userCard)
        {

        }
        foreach (Card card in monsterCard)
        {

        }
    }
    private void UserCardInit()
    {

    }
    private void MonsterCardInit()
    {

    }

    protected override void OnInitialize()
    {
        cardSO = Resources.Load<CardSO>("Card/CardSO");
        cardSO.InitCardSO();
        deck = new List<int>();
        for (int i = 0; i < 52; i++) deck.Add(i);
        ButtonInitialize();
        Shuffle();
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.CardHandler;
    }
}

public class CardShuffle
{
    private Random random;
    public CardShuffle() : this(Environment.TickCount) { }
    public CardShuffle(int seed)
    {
        random = new Random(seed);
    }

    public void Shuffle<T>(IList<T> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }
}
[Serializable]
public class CardData
{
    public Sprite sprite;
    public Shape shape;
    public Number number;
}
