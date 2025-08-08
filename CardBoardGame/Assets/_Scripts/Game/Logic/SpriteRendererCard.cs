using UnityEngine;

public class SpriteRendererCard : MonoBehaviour
{
    private SpriteRenderer _sprRender;
    private CardData _cardData;

    public CardData CardData
    {
        get => _cardData;
        set => _cardData = value;

    }
    private void Awake()
    {
        _sprRender = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        _sprRender.sprite = CardData.sprite;
        print($"{CardData.shape} {CardData.number}");
    }

}
