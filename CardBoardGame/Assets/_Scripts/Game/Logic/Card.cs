using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    public Button Button
    {
        get { return _button; }
    }
    private bool isClicked;
    public bool IsClicked => isClicked;
    private bool isInitialized = false;
    public bool IsUserCard = false;
    public ColorBlock clickedColors;
    private CardData _cardData;
    private CardHandler cardHandler;
    public Action deSelectAction;
    public CardData CardData
    {
        get => _cardData;
        set
        {
            _cardData = value;
            _button.image.sprite = _cardData.sprite;
        }

    }

    private void OnClick()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            _button.colors = clickedColors;
            cardHandler.SelectedCards.Add(this);
            if (IsUserCard)
            {
                cardHandler.SelectedUserCards.Add(this);
            }
        }
        else
        {
            _button.colors = ColorBlock.defaultColorBlock;
            cardHandler.SelectedCards.Remove(this);
            if (IsUserCard)
            {
                cardHandler.SelectedUserCards.Remove(this);
            }
        }
        cardHandler.OnSelectedCard(isClicked);
    }

    private void DeSelectAction()
    {
        cardHandler.SelectedCards.Remove(this);
        cardHandler.SelectedUserCards.Remove(this);
    }

    public void SetDefault()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            _button.colors = clickedColors;
        }
        else
        {
            _button.colors = ColorBlock.defaultColorBlock;
        }
        cardHandler.OnSelectedCard(isClicked);
    }

    public void Initialize(CardHandler cardHandler)
    {
        if (isInitialized)
        {
            return;
        }
        isInitialized = true;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        this.cardHandler = cardHandler;
        deSelectAction += DeSelectAction;
    }
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     _Canvas.sortingOrder++;
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     _Canvas.sortingOrder--;
    // }
}

