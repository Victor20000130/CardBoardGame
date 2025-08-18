using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
public class CardHandler : Handler
{

    [Serializable]
    public class CardResultWrapper
    {
        private Dictionary<Shape, int> usedCardDic = new Dictionary<Shape, int>()
        {
          { Shape.Spade, 0 },
          { Shape.Club, 0 },
          { Shape.Diamond, 0 },
          { Shape.Heart, 0 }
        };
        public Dictionary<Shape, int> UsedCardDic
        {
            get => usedCardDic;
            set => usedCardDic = value;
        }
        [SerializeField]
        private Card[] additionalCards;
        public Card[] AdditionalCards
        {
            get => additionalCards;
        }
        private int additionalCardCount = 0;

        public int AdditionalCardCount
        {
            get => additionalCardCount;
            set
            {
                additionalCardCount = value;
                print($"추가 카드 활성화: {additionalCardCount}");
                cardShuffleAct?.Invoke();
            }
        }
        public int CanThrowCount = 0;

        public Action cardShuffleAct;
    }

    private const int MaxCardCounting = 50;

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
    [SerializeField] private SpriteRendererCard[] upDownCard;
    [SerializeField] private CardResultWrapper cardResultWrapper;
    public CardResultWrapper CardResultWrapperPropertie
    {
        get => cardResultWrapper;
    }
    [SerializeField] private Button attackButton;
    [SerializeField] private Button throwButton;
    [SerializeField] private Button numberOrderButton;
    [SerializeField] private Button shapeOrderButton;
    [SerializeField] private Button cardUpButton;
    [SerializeField] private Button cardDownButton;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject cardUpDownPanel;
    [SerializeField] private RectTransform drawnCard;
    [SerializeField] private RectTransform ownCard;

    private CurveLayout drawnCardCurveLayOut;
    private CurveLayout ownCardCurveLayOut;

    private HandRankings handRankings;
    public HandRankings HandRankings
    {
        get => handRankings;
        set => handRankings = value;
    }
    private int selectedCount = 0;
    private int currUpDownCardIdx = 0;
    private List<Card> selectedCards = new List<Card>(MaxHandCount);
    public List<Card> SelectedCards => selectedCards;
    private List<Card> throwedCards = new List<Card>(MaxPairCount + 1);
    private List<Card> selectedUserCards = new List<Card>(MaxHandCount);
    private (HandRankings rankings, Func<bool> checker)[] rankingChecks;
    public List<Card> SelectedUserCards => selectedUserCards;

    public float CardsHideY = 1000;
    public float UpDownCardYValue;
    public int CanThrowCount
    {
        get => cardResultWrapper.CanThrowCount;
        set => cardResultWrapper.CanThrowCount = value;
    }

    public int SpadeUsedCard
    {
        get => cardResultWrapper.UsedCardDic[Shape.Spade];
    }

    public int ClubUsedCard
    {
        get => cardResultWrapper.UsedCardDic[Shape.Club];
    }
    public int DiamondUsedCard
    {
        get => cardResultWrapper.UsedCardDic[Shape.Diamond];
    }
    public int HeartUsedCard
    {
        get => cardResultWrapper.UsedCardDic[Shape.Heart];
    }
    protected override void OnInitialize()
    {

        drawnCardCurveLayOut = drawnCard.GetComponent<CurveLayout>();
        ownCardCurveLayOut = ownCard.GetComponent<CurveLayout>();

        CardSO originCardSO = Resources.Load<CardSO>("Data/UtilityData/CardSO");
        originCardSO.InitCardSO();
        cardSO = ScriptableObject.CreateInstance<CardSO>();
        originCardSO.Copy(cardSO);
        deck = Enumerable.Range(0, 52).ToList();
        Shuffle();
        SetAllCardsInteractable(false);
        InitializeButtons();

        rankingChecks = new (HandRankings, Func<bool>)[]
        {
        (HandRankings.Aion, IsAion),
        (HandRankings.Atropos, IsAtropos),
        (HandRankings.Nemesis, IsNemesis),
        (HandRankings.Tetrad, IsTetrad),
        (HandRankings.Legion, IsLegion),
        (HandRankings.Soma, IsSoma),
        (HandRankings.Atlas, IsAtlas),
        (HandRankings.Ananke, IsAnanke),
        (HandRankings.Hermes, IsHermes),
        (HandRankings.Triad, IsTriad),
        (HandRankings.Dyad_Set, IsDyad_Set),
        (HandRankings.Dyad, IsDyad),
        (HandRankings.Solo, () => selectedCount == 1),
        };
        cardResultWrapper.cardShuffleAct += Shuffle;

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
        print("Shuffle");
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
        foreach (var card in cardResultWrapper.AdditionalCards)
        {
            card.Initialize(this);
            if (cardResultWrapper.AdditionalCardCount > 0)
            {
                if (!cardResultWrapper.AdditionalCards[cardResultWrapper.AdditionalCardCount - 1].gameObject.activeSelf)
                {
                    cardResultWrapper.AdditionalCards[cardResultWrapper.AdditionalCardCount - 1].gameObject.SetActive(true);
                }
            }
            if (card.gameObject.activeSelf)
            {
                card.CardData = cardSO.cards[deck[currIdx++]];
            }
        }
        StartCoroutine(drawnCardCurveLayOut.SortLayout());
    }
    public void CardPanelOnOff() => cardPanel.SetActive(!cardPanel.activeSelf);

    private void InitializeButtons()
    {
        SetAllCardsInteractable(false);
        attackButton.onClick.AddListener(HandRankingCalc);
        attackButton.interactable = false;
        throwButton.onClick.AddListener(OnCardThrow);
        throwButton.interactable = false;

        numberOrderButton.onClick.AddListener(SortCardsByNumber);

        numberOrderButton.onClick.AddListener(() => StartCoroutine(drawnCardCurveLayOut.SortLayout()));
        numberOrderButton.onClick.AddListener(() => StartCoroutine(ownCardCurveLayOut.SortLayout()));

        shapeOrderButton.onClick.AddListener(SortCardsByShape);

        shapeOrderButton.onClick.AddListener(() => StartCoroutine(drawnCardCurveLayOut.SortLayout()));
        shapeOrderButton.onClick.AddListener(() => StartCoroutine(ownCardCurveLayOut.SortLayout()));

        cardUpButton.onClick.AddListener(() => GetNextCardValue(true));
        cardDownButton.onClick.AddListener(() => GetNextCardValue(false));
    }

    /// <summary>
    /// ownCard의 경우 -로 적용
    /// </summary>
    private void CardsHide()
    {
        drawnCard.DOAnchorPosY(CardsHideY, 1f).SetEase(Ease.InOutBack);
        ownCard.DOAnchorPosY(-CardsHideY, 1f).SetEase(Ease.InOutBack).OnComplete(Shuffle);
    }
    public void CardsDOTween()
    {
        drawnCard.DOAnchorPosY(0, 2f).SetEase(Ease.InOutBack);
        ownCard.DOAnchorPosY(0, 2f).SetEase(Ease.InOutBack);
    }
    private void SortCardsByShape()
    {
        // userCard를 문양(Shape) 기준으로 오름차순 정렬
        var sortedUser = userCard.OrderBy(card => card.CardData.shape).ToArray();
        for (int i = 0; i < sortedUser.Length; i++)
        {
            sortedUser[i].transform.SetSiblingIndex(i);
        }

        // monsterCard도 동일하게 오름차순 정렬
        var sortedMonster = monsterCard.OrderBy(card => card.CardData.shape).ToArray();
        for (int i = 0; i < sortedMonster.Length; i++)
        {
            sortedMonster[i].transform.SetSiblingIndex(i);
        }
    }

    private void SortCardsByNumber()
    {
        // userCard를 숫자(Number) 기준으로 오름차순 정렬
        var sortedUser = userCard.OrderBy(card => card.CardData.number).ToArray();
        for (int i = 0; i < sortedUser.Length; i++)
        {
            sortedUser[i].transform.SetSiblingIndex(i);
        }

        // monsterCard도 동일하게 오름차순 정렬
        var sortedMonster = monsterCard.OrderBy(card => card.CardData.number).ToArray();
        for (int i = 0; i < sortedMonster.Length; i++)
        {
            sortedMonster[i].transform.SetSiblingIndex(i);
        }
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
                    if (++cardResultWrapper.UsedCardDic[Shape.Spade] > MaxCardCounting)
                    {
                        cardResultWrapper.UsedCardDic[Shape.Spade] = MaxCardCounting;
                    }
                    break;
                case Shape.Club:
                    if (++cardResultWrapper.UsedCardDic[Shape.Club] > MaxCardCounting)
                    {
                        cardResultWrapper.UsedCardDic[Shape.Club] = MaxCardCounting;
                    }
                    break;
                case Shape.Diamond:
                    if (++cardResultWrapper.UsedCardDic[Shape.Diamond] > MaxCardCounting)
                    {
                        cardResultWrapper.UsedCardDic[Shape.Diamond] = MaxCardCounting;
                    }
                    break;
                case Shape.Heart:
                    if (++cardResultWrapper.UsedCardDic[Shape.Heart] > MaxCardCounting)
                    {
                        cardResultWrapper.UsedCardDic[Shape.Heart] = MaxCardCounting;
                    }
                    break;
            }
            card.SetDefault();
        }

        Debug.Log($"현재까지 사용된 카드: Spade: {cardResultWrapper.UsedCardDic[Shape.Spade]}, Club: {cardResultWrapper.UsedCardDic[Shape.Club]}, Dia: {cardResultWrapper.UsedCardDic[Shape.Diamond]}, Heart: {cardResultWrapper.UsedCardDic[Shape.Heart]}");
        Debug.Log(handRankings);

        CardSelectFin();

    }
    public void TestCard()
    {
        selectedCount = 5;
        handRankings = HandRankings.None;
        foreach (var (ranking, checker) in rankingChecks)
        {
            if (checker())
            {
                handRankings = ranking;
                break;
            }
        }
        print(handRankings);
    }
    private void CardListsClear()
    {
        selectedCards.Clear();
        selectedUserCards.Clear();
        throwedCards.Clear();
    }

    public void CardSelectFin()
    {
        ManagerHandler.Instance.gameManager.ReceiveCardResult(handRankings, cardResultWrapper);
        CardListsClear();
        attackButton.interactable = false;
        throwButton.interactable = false;
        CardsHide();
    }

    // --- 카드 특수 효과 ---

    /// <summary> 특수효과 검출 전 전부 같은 모양인지 확인 </summary>
    private bool IsAnyDifferentShape()
    {
        var firstShape = selectedCards[0].CardData.shape;

        if (!selectedCards.Any(c => c.CardData.shape != firstShape))
        {
            return false;
        }
        return true;
    }

    // --- 핸드 랭킹 판별 메서드들 ---

    /// <summary> 같은 숫자 2장(원페어) </summary>
    private bool IsDyad() =>
        selectedCount >= MaxPairCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxPairCount);

    /// <summary> 2장짜리 페어가 2쌍(투페어) </summary>
    private bool IsDyad_Set() =>
        selectedCount >= MaxQuadCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Count(g => g.Count() == MaxPairCount) == MaxPairCount;

    /// <summary> 같은 숫자 3장(트리플) </summary>
    private bool IsTriad() =>
        selectedCount >= MaxTripleCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxTripleCount);

    /// <summary> 모두 다른 문양, 연속되는 숫자가 1~5, 1,10,11,12,13이 아닌경우만 true(스트레이트) </summary>
    private bool IsHermes()
    {
        print(1);
        if (!IsFullSelect())
        {
            return false;
        }
        print(11);

        if (!IsAllDifferentShape())
        {
            print(2);
            return false;
        }
        print(111);

        // 숫자 오름차순 정렬
        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        // 특정한 숫자 조합이 있는지 확인
        List<List<int>> validCombinations = new List<List<int>>()
        {
        new List<int> { 1, 2, 11, 12, 13 },
        new List<int> { 1, 2, 3, 12, 13 },
        new List<int> { 1, 2, 3, 4, 13 }
        };

        bool containsValidCombination = validCombinations.Any(combo => combo.All(c => numbers.Contains(c)));

        if (containsValidCombination)
        {
            return true;
        }

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary> 모두 다른 문양, A,2,3,4,5인 경우만 true(백스트레이트) </summary>
    private bool IsAnanke()
    {
        if (!IsFullSelect())
        {
            return false;
        }

        if (!IsAllDifferentShape())
        {
            return false;
        }

        // 숫자 오름차순 정렬
        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        // 1~5인지 확인
        bool isOneToFive = numbers.SequenceEqual(Enumerable.Range(1, 5));
        print(isOneToFive);
        return isOneToFive;
    }

    /// <summary> 모두 다른 문양, 10, J, Q, K, A인 경우만 true(마운틴) </summary>
    private bool IsAtlas()
    {
        print(44);
        if (!IsFullSelect())
        {
            return false;
        }
        print(33);
        // 모두 다른 문양인지 확인
        if (!IsAllDifferentShape())
        {
            print(1);
            return false;
        }
        print(2);

        // 숫자 오름차순 정렬
        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        List<int> compare = new List<int> { 1, 10, 11, 12, 13 };
        foreach (int num in numbers)
        {
            print(num);
        }
        foreach (int num in compare)
        {
            print(num);
        }
        bool isTenToOne = numbers.SequenceEqual(compare);

        print(isTenToOne);
        return isTenToOne;
    }

    /// <summary> 같은 문양 5장(플러시) </summary>
    private bool IsSoma() =>
        selectedCount == MaxHandCount &&
        selectedCards.GroupBy(c => c.CardData.shape)
                     .Any(g => g.Count() == MaxHandCount);

    /// <summary> 3장+2장(풀하우스) </summary>
    private bool IsLegion()
    {
        if (!IsFullSelect())
        {
            return false;
        }

        var groups = selectedCards.GroupBy(c => c.CardData.number)
                                 .Select(g => g.Count())
                                 .ToList();

        return groups.Contains(MaxTripleCount) && groups.Contains(MaxPairCount);
    }

    /// <summary> 같은 숫자 4장(포카드) </summary>
    private bool IsTetrad() =>
        selectedCount >= MaxQuadCount &&
        selectedCards.GroupBy(c => c.CardData.number)
                     .Any(g => g.Count() == MaxQuadCount);

    /// <summary> 같은 문양, 연속된 숫자 5장(스트레이트 플러시) </summary>
    private bool IsNemesis()
    {
        if (!IsFullSelect())
        {
            return false;
        }

        var firstShape = selectedCards[0].CardData.shape;

        if (IsAnyDifferentShape())
        {
            return false;
        }

        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        // 특정한 숫자 조합이 있는지 확인
        List<List<int>> validCombinations = new List<List<int>>()
        {
        new List<int> { 1, 2, 11, 12, 13 },
        new List<int> { 1, 2, 3, 12, 13 },
        new List<int> { 1, 2, 3, 4, 13 }
        };

        bool containsValidCombination = validCombinations.Any(combo => combo.All(c => numbers.Contains(c)));

        if (containsValidCombination)
        {
            return true;
        }

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
            {
                return false;
            }
        }

        return true;
    }
    /// <summary> 모두 같은 문양, 1,2,3,4,5 5장(백 스트레이트 플러쉬) </summary>
    private bool IsAtropos()
    {
        if (!IsFullSelect())
        {
            return false;
        }

        if (IsAnyDifferentShape())
        {
            return false;
        }
        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        bool isOneToFive = numbers.SequenceEqual(Enumerable.Range(1, 5));
        return isOneToFive;
    }

    /// <summary> 모두 같은 문양, 1, 10,11,12,13 5장(로얄 스트레이트 플러쉬) </summary>
    private bool IsAion()
    {
        if (!IsFullSelect())
        {
            return false;
        }

        if (IsAnyDifferentShape())
        {
            return false;
        }
        var numbers = selectedCards.Select(c => (int)c.CardData.number)
                                  .OrderBy(n => n)
                                  .ToList();

        List<int> compare = new List<int> { 1, 10, 11, 12, 13 };
        bool isTenToOne = numbers.SequenceEqual(compare);
        return isTenToOne;
    }

    private bool IsAllDifferentShape()
    {
        // 모두 다른 문양인지 확인
        bool allShapeDistinct = selectedCards.Select(c => c.CardData.shape)
                                     .Distinct()
                                     .Count() == MaxHandCount;
        return !allShapeDistinct;
    }

    private bool IsFullSelect()
    {
        if (selectedCount != MaxHandCount || selectedCards.Count == 0)
        {
            return false;
        }
        return true;
    }

    public void StartCardUpDown()
    {
        cardUpDownPanel.SetActive(true);
        UpDownCardButtonOnOff(true);
        foreach (SpriteRendererCard card in upDownCard)
        {
            card.CardData = cardSO.cards[deck[currIdx++]];
            card.Initialize();
        }
        upDownCard[currUpDownCardIdx].transform.DORotate(new Vector3(0, UpDownCardYValue, 0), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
        Shuffle();
    }

    private void GetNextCardValue(bool isUp)
    {
        if (currUpDownCardIdx == upDownCard.Length - 1)
        {
            return;
        }

        bool isNextCardValHigher = IsNextCardValueHigher(upDownCard[currUpDownCardIdx].CardData, upDownCard[currUpDownCardIdx + 1].CardData);
        bool isCorrect = false;
        UpDownCardButtonOnOff(isCorrect);
        if (isUp)
        {
            if (isNextCardValHigher)
            {
                isCorrect = true;
            }
        }
        else
        {
            if (!isNextCardValHigher)
            {
                isCorrect = true;
            }
        }
        currUpDownCardIdx++;
        upDownCard[currUpDownCardIdx].transform.DORotate(new Vector3(0, UpDownCardYValue, 0), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo).onComplete += () => UpDownCardButtonOnOff(isCorrect);
        if (!isCorrect || currUpDownCardIdx == upDownCard.Length - 1)
        {
            StartCoroutine(UpDownCardGameEnd(isCorrect));
        }
    }

    private IEnumerator UpDownCardGameEnd(bool isCorrect)
    {
        yield return new WaitForSeconds(4);
        ManagerHandler.Instance.gameManager.GetUpDownCardResult(isCorrect);
        cardUpDownPanel.SetActive(false);
    }

    private bool IsNextCardValueHigher(CardData curr, CardData next)
    {
        if (curr.number < next.number)
        {
            return true;
        }
        if (curr.number > next.number)
        {
            return false;
        }
        // 숫자가 같으면 shape 비교
        return curr.shape > next.shape;
    }

    private void UpDownCardButtonOnOff(bool isOn)
    {
        cardDownButton.interactable = isOn;
        cardUpButton.interactable = isOn;
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
