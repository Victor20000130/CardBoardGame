using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Button _button;
    public Button Button
    {
        get { return _button; }
    }
    private bool isClicked;
    public bool IsClicked => isClicked;
    public ColorBlock clickedColors;
    private CardData _cardData;
    private CardHandler cardHandler;

    public CardData CardData
    {
        get => _cardData;
        set
        {
            _cardData = value;
            _button.image.sprite = _cardData.sprite;
        }

    }
    public void OnClick()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            _button.colors = clickedColors;
            cardHandler.SelectedCards.Add(_cardData);
        }
        else
        {
            _button.colors = ColorBlock.defaultColorBlock;
            cardHandler.SelectedCards.Remove(_cardData);
        }
        cardHandler.OnSelectedCard(isClicked);
    }
    public void Initialize(CardHandler cardHandler)
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        this.cardHandler = cardHandler;
    }
}

