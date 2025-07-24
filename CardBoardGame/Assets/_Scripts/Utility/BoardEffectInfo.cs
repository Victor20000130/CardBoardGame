using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardEffectInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    public Sprite sprite
    {
        get => image.sprite;
        set => image.sprite = value;
    }
    public GridData gridData;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
