using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;

public class CardHandler : Handler
{
    // 카드 조합 관련 상수
    private const int MaxHandCount = 5;
    private const int MaxPairCount = 2;
    private const int MaxTripleCount = 3;
    private const int MaxQuadCount = 4;

    private CardSO cardSO;
    private List<int> deck;
    private int currIdx = 0;
    [SerializeField] private Card[] monsterCard;
    [SerializeField] private Card[] userCard;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button throwButton;
    [SerializeField] private GameObject cardPanel;

    private HandRankings handRankings;
    private int selectedCount = 0;

    private List<Card> selectedCards = new List<Card>(MaxHandCount);
    public List<Card> SelectedCards => selectedCards;
    private List<Card> throwedCards = new List<Card>(MaxPairCount + 1);
    private List<Card> selectedUserCards = new List<Card>(MaxHandCount);
    private (HandRankings rankings, Func<bool> checker)[] rankingChecks;
    public List<Card> SelectedUserCards => selectedUserCards;

    public int CanThrowCount = 0;

    private Dictionary<Shape, int> spEffectDic =
        new Dictionary<Shape, int>() {
        { Shape.Spade, 0 },
        { Shape.Club, 0 },
        { Shape.Diamond, 0 },
        { Shape.Heart, 0 }
        };

    protected override void OnInitialize()
    {
        cardSO = Resources.Load<CardSO>("Data/UtilityData/CardSO");
        cardSO.InitCardSO();
        deck = Enumerable.Range(0, 52).ToList();
        Shuffle();
        SetAllCardsInteractable(false);
        InitializeButtons();

        rankingChecks = new (HandRankings, Func<bool>)[]
        {
        (HandRankings.Aion, IsAion),
        (HandRankings.Atropos, IsAtropos),
        (HandRankings.Nemesis, IsNemesis),
        (HandRankings.Legion, IsLegion),
        (HandRankings.Soma, IsSoma),
        (HandRankings.Tetrad, IsTetrad),
        (HandRankings.Triad, IsTriad),
        (HandRankings.Dyad_Set, IsDyad_Set),
        (HandRankings.Dyad, IsDyad),
        (HandRankings.Solo, () => selectedCount == 1),
        };
    }
    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.CardHandler;
    }

    public void Shuffle()
    {
        currIdx = 0;
        new CardShuffle().Shuffle(deck);
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
        foreach (var card in monsterCard)
        {
            card.Initialize(this);
            card.CardData = cardSO.cards[deck[currIdx++]];
        }
        foreach (var card in userCard)
        {
            card.Initialize(this);
            card.CardData = cardSO.cards[deck[currIdx++]];
        }
    }
    public void CardPanelOnOff() => cardPanel.SetActive(!cardPanel.activeSelf);

    private void InitializeButtons()
    {
        SetAllCardsInteractable(false);
        attackButton.onClick.AddListener(HandRankingCalc);
        attackButton.interactable = false;
        throwButton.onClick.AddListener(OnCardThrow);
        throwButton.interactable = false;
    }

    public void SetAllCardsInteractable(bool isOn)
    {
        foreach (var card in userCard)
        {
            card.Button.interactable = isOn;
        }
        foreach (var card in monsterCard)
        {
            card.Button.interactable = isOn;
        }
    }

    private void OnCardThrow()
    {
        throwedCards.Clear();
        foreach (var card in selectedCards.Where(c => c.IsUserCard))
        {
            card.CardData = cardSO.cards[deck[currIdx++]];
            card.SetDefault();
            throwedCards.Add(card);
        }
        foreach (var card in throwedCards)
        {
            card.deSelectAction?.Invoke();
        }
        throwedCards.Clear();
        throwButton.interactable = false;
        attackButton.interactable = false;
        CanThrowCount--;
    }

    public void OnSelectedCard(bool isOn)
    {
        if (isOn)
        {
            selectedCount++;
            if (selectedUserCards.Count > 0)
            {
                attackButton.interactable = true;
            }
            if (selectedUserCards.Count == 1 && CanThrowCount > 0)
            {
                throwButton.interactable = true;
            }
            if (selectedUserCards.Count == 4)
            {
                throwButton.interactable = false;
            }
        }
        else
        {
            selectedCount--;
            if (selectedCount == 4)
            {
                SetSelectMax(false);
            }

            if (selectedUserCards.Count == 3 && CanThrowCount > 0)
            {
                throwButton.interactable = true;
            }
        }

        if (selectedCount == 5)
        {
            SetSelectMax(true);
        }

        if (selectedUserCards.Count == 0)
        {
            attackButton.interactable = false;
        }

        if (selectedUserCards.Count == 0)
        {
            throwButton.interactable = false;
        }
        // HandRankingCalc();   
    }

    private void SetSelectMax(bool isMax)
    {
        SetCardsInteractable(monsterCard, !isMax);
        SetCardsInteractable(userCard, !isMax);
    }

    private void SetCardsInteractable(Card[] cards, bool isOn)
    {
        foreach (var card in cards)
        {
            if (!card.IsClicked)
            {
                card.Button.interactable = isOn;
            }
        }
    }

    /// <summary>
    /// 선택된 카드의 핸드 랭킹을 판별하고 결과를 전달
    /// </summary>
    private void HandRankingCalc()
    {

        handRankings = HandRankings.None;

        foreach (var (ranking, checker) in rankingChecks)
        {
            if (checker())
            {
                handRankings = ranking;
                break;
            }
        }

        if (handRankings == HandRankings.None)
        {
            handRankings = HandRankings.Solo;
        }

        foreach (Card card in selectedCards)
        {
            switch (card.CardData.shape)
            {
                case Shape.Spade:
                    spEffectDic[Shape.Spade]++;
                    break;
                case Shape.Club:
                    spEffectDic[Shape.Club]++;
                    break;
                case Shape.Diamond:
                    spEffectDic[Shape.Diamond]++;
                    break;
                case Shape.Heart:
                    spEffectDic[Shape.Heart]++;
                    break;
            }
            card.SetDefault();
        }

        Debug.Log($"현재까지 사용된 카드: Spade: {spEffectDic[Shape.Spade]}, Club: {spEffectDic[Shape.Club]}, Dia: {spEffectDic[Shape.Diamond]}, Heart: {spEffectDic[Shape.Heart]}");
        Debug.Log(handRankings);

        ManagerHandler.Instance.gameManager.ReceiveCardResult(handRankings, spEffectDic);

        cardPanel.SetActive(false);

        CardListsClear();

        Shuffle();

        attackButton.interactable = false;
        throwButton.interactable = false;
    }

    private void CardListsClear()
    {
        selectedCards.Clear();
        selectedUserCards.Clear();
        throwedCards.Clear();
    }

    // --- 카드 특수 효과 ---

    /// <summary> 특수효과 검출 전 전부 같은 모양인지 확인 </summary>
    [Obsolete]
    private bool IsSameShape()
    {
        var firstShape = selectedCards[0].CardData.shape;

        if (!selectedCards.All(c => c.CardData.shape == firstShape))
        {
            return false;
        }
        return true;
    }

    // --- 핸드 랭킹 판별 메서드들 ---

    /// <summary> 같은 숫자 2장(페어) </summary>
    private bool IsDyad() =>
        selectedCount >= MaxPairCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxPairCount);

    /// <summary> 같은 숫자 3장(트리플) </summary>
    private bool IsTriad() =>
        selectedCount >= MaxTripleCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxTripleCount);

    /// <summary> 2장짜리 페어가 2쌍 </summary>
    private bool IsDyad_Set() =>
        selectedCount >= MaxQuadCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Count(g => g.Count() == MaxPairCount) == MaxPairCount;

    /// <summary> 같은 숫자 4장(쿼드) </summary>
    private bool IsTetrad() =>
        selectedCount >= MaxQuadCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxQuadCount);

    /// <summary> 같은 문양 5장(플러시) </summary>
    private bool IsSoma() =>
        selectedCount == MaxHandCount &&
        selectedCards.GroupBy(c => c.CardData.shape)
                     .Any(g => g.Count() == MaxHandCount);

    /// <summary> 3장+2장(풀하우스) </summary>
    private bool IsLegion()
    {
        if (selectedCount != MaxHandCount)
        {
            return false;
        }

        var groups = selectedCards.GroupBy(c => c.CardData.number)
                                 .Select(g => g.Count())
                                 .ToList();

        return groups.Contains(MaxTripleCount) && groups.Contains(MaxPairCount);
    }

    /// <summary> 같은 문양, 연속된 숫자 5장(스트레이트 플러시) </summary>
    private bool IsNemesis()
    {
        if (selectedCount != MaxHandCount)
        {
            return false;
        }

        if (selectedCards.Count == 0)
        {
            return false;
        }

        var firstShape = selectedCards[0].CardData.shape;

        if (!selectedCards.All(c => c.CardData.shape == firstShape))
        {
            return false;
        }

        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 모두 다른 문양, 모두 Ace 또는 모두 King </summary>
    private bool IsAtropos()
    {
        if (selectedCount != MaxHandCount)
        {
            return false;
        }

        if (selectedCards.Count == 0)
        {
            return false;
        }

        bool allShapeDistinct = selectedCards.Select(c => c.CardData.shape)
                                             .Distinct()
                                             .Count() == MaxHandCount;
        if (!allShapeDistinct)
        {
            return false;
        }

        return selectedCards.All(c => c.CardData.number == Number.Ace) ||
               selectedCards.All(c => c.CardData.number == Number.King);
    }

    /// <summary> 모두 같은 문양, 모두 King </summary>
    private bool IsAion()
    {
        if (selectedCount != MaxHandCount)
        {
            return false;
        }

        if (selectedCards.Count == 0)
        {
            return false;
        }

        bool allShapeSame =
        selectedCards.All(c => c.CardData.shape == selectedCards[0].CardData.shape);

        if (!allShapeSame)
        {
            return false;
        }
        return selectedCards.All(c => c.CardData.number == Number.King);
    }
}

// 카드 셔플 유틸리티
public class CardShuffle
{
    private readonly Random random;

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
            (deck[i], deck[j]) = (deck[j], deck[i]);
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
