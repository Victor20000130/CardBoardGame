using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;

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
    private HandRankings handRankings;
    private int selectedCount = 0;

    private List<CardData> selectedCards = new List<CardData>(5);
    public List<CardData> SelectedCards
    {
        get => selectedCards;
        set => selectedCards = value;
    }

    private void Awake()
    {

    }

    public void Shuffle()
    {
        currIdx = 0;
        var shuffler = new CardShuffle();
        shuffler.Shuffle(deck);
        SetCards();
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
            monsterCard[i].Initialize(this);
            monsterCard[i].CardData = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
        // userCard 처리
        for (int i = 0; i < userCard.Length; i++)
        {
            userCard[i].Initialize(this);
            userCard[i].CardData = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
    }
    public void ReSetCards()
    {
        for (int i = 0; i < monsterCard.Length; i++)
        {
            monsterCard[i].CardData = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
        for (int i = 0; i < userCard.Length; i++)
        {
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
        CardGameActivation(false);
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.CardHandler;
    }

    public void CardGameActivation(bool isOn)
    {
        attackButton.interactable = isOn;
        throwButton.interactable = isOn;
    }

    public void OnSelectedCard(bool isOn)
    {
        if (isOn)
        {
            if (selectedCount == 1)
            {
                attackButton.interactable = true;
            }

            selectedCount++;
        }
        else
        {

            selectedCount--;
            if (selectedCount == 4)
            {
                IsSelectMax(false);
            }
        }

        if (selectedCount == 5)
        {
            IsSelectMax(true);
        }

        if (selectedCount == 0)
        {
            attackButton.interactable = false;
        }
        HandRankingCalc();
    }

    private void IsSelectMax(bool isTrue)
    {
        if (isTrue)
        {
            ActivationCards(monsterCard, false);
            ActivationCards(userCard, false);
        }
        else
        {
            ActivationCards(monsterCard, true);
            ActivationCards(userCard, true);
        }
    }

    private void ActivationCards(Card[] cards, bool isOn)
    {
        foreach (Card card in cards)
        {
            if (card.IsClicked == false)
            {
                card.Button.interactable = isOn;
            }
        }
    }

    private void HandRankingCalc()
    {

        switch (selectedCount)
        {
            case 0:
                handRankings = HandRankings.None;
                break;
            case 1:
                handRankings = HandRankings.Solo;
                break;
        }
        if (IsDyad())
        {
            handRankings = HandRankings.Dyad;
        }
        if (IsDyad_Set())
        {
            handRankings = HandRankings.Dyad_Set;
        }
        if (IsTriad())
        {
            handRankings = HandRankings.Triad;
        }
        if (IsTetrad())
        {
            handRankings = HandRankings.Tetrad;
        }
        if (IsSoma())
        {
            handRankings = HandRankings.Soma;
        }
        if (IsLegion())
        {
            handRankings = HandRankings.Legion;
        }
        if (IsNemesis())
        {
            handRankings = HandRankings.Nemesis;
        }
        if (IsAtropos())
        {
            handRankings = HandRankings.Atropos;
        }
        if (IsAion())
        {
            handRankings = HandRankings.Aion;
        }
        print(handRankings);
    }
    // 아래는 각 랭킹별 판별 메서드
    private bool IsDyad()
    {
        if (selectedCount < 2)
        {
            return false;
        }
        return selectedCards
            .GroupBy(card => card.number)
            .Any(g => g.Count() == 2);
    }

    private bool IsTriad()
    {
        if (selectedCount < 3)
        {
            return false;
        }
        return selectedCards
            .GroupBy(card => card.number)
            .Any(g => g.Count() == 3);
    }

    private bool IsDyad_Set()
    {
        if (selectedCount < 4)
        {
            return false;
        }
        // 같은 숫자의 카드가 2장씩 2쌍
        return selectedCards
            .GroupBy(card => card.number)
            .Count(g => g.Count() == 2) == 2;
    }

    private bool IsTetrad()
    {
        if (selectedCount < 4)
        {
            return false;
        }
        return selectedCards
            .GroupBy(card => card.number)
            .Any(g => g.Count() == 4);
    }
    private bool IsSoma()
    {
        if (selectedCount < 5)
        {
            return false;
        }
        return selectedCards
            .GroupBy(card => card.shape)
            .Any(g => g.Count() == 5);
    }
    private bool IsLegion()
    {
        if (selectedCount < 5)
        {
            return false;
        }
        return selectedCards
                .GroupBy(card => card.number)
                .Any(g => g.Count() == 3) &&
                    selectedCards
                .GroupBy(card => card.number)
                .Any(g => g.Count() == 2);
    }
    private bool IsNemesis()
    {
        // 모든 카드의 문양이 같은지 확인
        if (selectedCount < 5)
        {
            return false;
        }
        var firstShape = selectedCards[0].shape;
        bool allSameShape = selectedCards.All(card => card.shape == firstShape);
        if (!allSameShape)
        {
            return false;
        }

        // 숫자를 오름차순 정렬
        var numbers = selectedCards.Select(card => (int)card.number).OrderBy(n => n).ToList();

        // 연속되는지 확인
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsAtropos()
    {
        if (selectedCount < 4)
        {
            return false;
        }
        // shape가 모두 다른지 확인
        bool allShapeDistinct = selectedCards.Select(card => card.shape).Distinct().Count() == 5;
        if (!allShapeDistinct)
        {
            return false;
        }
        // 모든 카드의 number가 Ace 또는 King 중 하나인지 확인
        bool allAce = selectedCards.All(card => card.number == Number.Ace);
        bool allKing = selectedCards.All(card => card.number == Number.King);

        return allAce || allKing;
    }

    private bool IsAion()
    {
        if (selectedCount < 5)
        {
            return false;
        }
        // 5장 모두 같은 문양인지 확인
        bool allShapeSame = selectedCards.
                        GroupBy(card => card.shape).
                        Any(g => g.Count() == 5);
        if (!allShapeSame)
        {
            return false;
        }
        // 5장 모두 King인지 확인
        bool allKing = selectedCards.All(card => card.number == Number.King);
        return allKing;

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
