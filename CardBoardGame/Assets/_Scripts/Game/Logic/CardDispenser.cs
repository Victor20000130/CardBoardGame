using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDispenser : MonoBehaviour
{
    private CardSO cardSO;
    private List<int> deck;
    private int currIdx = 0;
    [SerializeField]
    private Image[] images;

    private void Awake()
    {
        cardSO = Resources.Load<CardSO>("Card/CardSO");

        deck = new List<int>();
        for (int i = 0; i < 52; i++) deck.Add(i);
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
        foreach (Image im in images)
        {
            im.sprite = cardSO.cards[deck[currIdx]];
            currIdx++;
        }
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